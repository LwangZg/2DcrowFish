using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public Rigidbody2D mybody;
    public Animator myani;
    public float speedx;
    public float speedy;
    public Vector2 backwardForce;

    public float autospeed;
    public float autospeedpro;

    public float atk;


    //我的摄像机
    public GameObject mycamera;


    //声音
    public AudioSource Sound;
    public AudioSource ActionSound;

    public AudioClip Voice_Walk;
    public float HitVoiceValue;

    //攻击判定框
    //普通三段攻击
    public GameObject Attack01shape;
    public GameObject Attack02shape;
    public GameObject Attack03shape;

    //跳跃特效
    public GameObject JumpE;

    //记录空中停留时间
    public float airtime;


    //地面触发
    public bool isground;

    //手动选择作为判定地面的层级
    public LayerMask groundLayer;

    //落地点判定射线长度
    public float distance;

    //内置状态机记录标签
    public State mystate;

    //内置状态机
    public enum State
    {
        //可移动的，包括进行能进行位移和转向
        Moveable,
        //自动移动动作
        MoveableAuto,
        //不可移动的，不能进行位移，也不能转向
        Moveunable,
        //受伤的，不可操纵，失去除击退力以外的全部力的作用
        Damaged,
    }

    //【人物开关】
    public bool Online;
    //互动物品数组
    public GameObject[] Items;
    //当前接近物品
    public GameObject MyItem;

    //互动提示图标
    public GameObject MyEbutton;


    void Awake() {
        mybody = GetComponent<Rigidbody2D>();
        myani = GetComponent<Animator>();
        //两个不同的音响播放器，一个是外部互动Sound，一个是自身动作ActionSound
        Sound = GetComponent<AudioSource>();
        //ActionSound = GameObject.Find("ActionSound").GetComponent<AudioSource>();
        //击退后座力初始化
        //backwardForce = new Vector2(-100f, 50f);
        //移动参数
        speedx = 1.0f;
        //跳跃参数
        speedy = 150f;
        //设置状态机
        mystate = State.Moveable;
        //默认的基础攻击力
        atk = 20.0f;
        //用于定向移动时的量
        autospeedpro = 2.0f;
        autospeed = autospeedpro;
        //空中记录的停留时间
        airtime = 0;
        //我的摄像机盒子中的摄像机
        mycamera = GameObject.Find("MainCameraBox");


        //开关视情况而定
        Online = true;
        //寻找这个场景所有的互动物品
        Items = null;
        Items = GameObject.FindGameObjectsWithTag("SceneItem");
        MyItem = null;
        MyEbutton = GameObject.Find("EButton");
    }

    //射线检查判定isground函数
    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        //查看用
        //Debug.DrawRay(position, direction, Color.green, distance);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            //print(hit.collider.name);
            return true;
        }
        return false;
    }


    //固定帧刷新 包括但不限于
    //1、高度检测 2、地面检测 3、内置状态机更新 4、内置状态机状态处理
    //5、设置无敌帧 6、额外、持续的声音处理
    private void FixedUpdate()
    {
        //空中停留时间
        if (myani.GetCurrentAnimatorStateInfo(0).IsName("jumptop")
            || myani.GetCurrentAnimatorStateInfo(0).IsName("jumpdown"))
        {
            airtime += Time.deltaTime;
        }
        else
        {
            airtime = 0;
        }
        myani.SetFloat("airtime", airtime);



        //声音处理
        if (myani.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            if (ActionSound.clip == Voice_Walk)
            {

            }
            else
            {
                ActionSound.clip = Voice_Walk;
                ActionSound.volume = 0.5f;
                ActionSound.pitch = 1.1f;
                ActionSound.loop = true;
                ActionSound.Play();
            }
        }
        else
        {
            if (ActionSound.clip == Voice_Walk)
            {
                ActionSound.clip = null;
            }
            ActionSound.pitch = 1;
            ActionSound.loop = false;
        }

        //设置无敌帧
        if (myani.GetCurrentAnimatorStateInfo(0).IsName("roll")
        || myani.GetCurrentAnimatorStateInfo(0).IsName("backup")
        || myani.GetCurrentAnimatorStateInfo(1).IsName("Invincible"))
        {
            this.gameObject.layer = LayerMask.NameToLayer("Invincible");
        }
        else
        {
            this.gameObject.layer = LayerMask.NameToLayer("Player");
        }
        //更新高度
        myani.SetFloat("heightvalue", mybody.velocity.y);
        //新增防止跳跃落下滑步的检测量
        if (mybody.velocity.y == 0)
        {
            myani.SetBool("step_heightvalue", true);
        }
        else
        {
            myani.SetBool("step_heightvalue", false);
        }
        //一般地面检测，见函数
        //isground = IsGrounded();
        //地面检测后，如果仍然有高低差，以高低差为准
        if (mybody.velocity.y != 0)
        {
            isground = false;
        }
        //同步
        myani.SetBool("isground", isground);

        //更新内置状态机，没找到更好的写法
        //|| myani.GetCurrentAnimatorStateInfo(0).IsName("flashdown")
        if (myani.GetCurrentAnimatorStateInfo(0).IsName("run")
            || myani.GetCurrentAnimatorStateInfo(0).IsName("idle")
            || myani.GetCurrentAnimatorStateInfo(0).IsName("jumpup")
            || myani.GetCurrentAnimatorStateInfo(0).IsName("jumptop")
            || myani.GetCurrentAnimatorStateInfo(0).IsName("jumpdown")
            || myani.GetCurrentAnimatorStateInfo(0).IsName("flash")
            )
        {
            mystate = State.Moveable;
        }
        else if (myani.GetCurrentAnimatorStateInfo(0).IsName("roll") || myani.GetCurrentAnimatorStateInfo(0).IsName("backup")
            || myani.GetCurrentAnimatorStateInfo(0).IsName("flashdown"))
        {
            mystate = State.MoveableAuto;
        }
        else if (myani.GetCurrentAnimatorStateInfo(0).IsName("hurt"))
        {
            mystate = State.Damaged;
        }
        else
        {
            mystate = State.Moveunable;
        }

        //对应内置状态机施加力
        switch (mystate)
        {
            //可移动、可转向，由玩家操纵
            case State.Moveable:
                if (Online == true)
                {
                    float dx = Input.GetAxis("Horizontal");
                    Mymove(dx);
                }
                else
                {
                    mybody.velocity = new Vector2(0, mybody.velocity.y);
                }
                break;
            //自动移动
            case State.MoveableAuto:
                mybody.velocity = new Vector2(autospeed * speedx, mybody.velocity.y);
                break;
            //不可移动不可转向
            case State.Moveunable:
                mybody.velocity = new Vector2(0, mybody.velocity.y);
                break;
            //受伤状态，凌空击飞
            case State.Damaged:
                break;
        }

        //互动物品的遍历,两个的时候J = 2
        int i = 0;
        int j = Items.Length;
        //这个循环可能有错误，总之是能用了，实在太久没写了
        do
        {
            if (j == 0)
            {
                MyItem = null;
                break;
            }
            if ((Vector2.Distance(Items[i].GetComponent<SceneItem>().Center.transform.position, transform.position)) < 0.4f)
            {
                MyItem = Items[i];
                break;
            }
            else
            {
                MyItem = null;
                i++;
            }
        }while (i < j);
        //同步提示
        if (MyItem == null)
        {
            MyEbutton.GetComponent<ForEButton>().Online = false;
        }
        else
        {
            MyEbutton.GetComponent<ForEButton>().Online = true;               
        }

    }

    void Update() {
        if (Online == true)
        {
            //视角控制
            if (Input.GetKey(KeyCode.W))
            {
                mycamera.GetComponent<Camerafallow>().Outside_Y = 0.25f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                mycamera.GetComponent<Camerafallow>().Outside_Y = -0.25f;
            }
            else
            {
                mycamera.GetComponent<Camerafallow>().Outside_Y = 0;
            }

            //J = 攻击，生效于待机、奔跑、连击一、二阶段
            if (Input.GetKeyDown(KeyCode.J) &&
                (myani.GetCurrentAnimatorStateInfo(0).IsName("attack01") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("attack02") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("roll") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("rollstanding") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("idle") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("run")))
            {
                myani.SetTrigger("tattack");
            }


            //K = 特殊攻击，生效于连击二阶段
            if (Input.GetKeyDown(KeyCode.K) && (myani.GetCurrentAnimatorStateInfo(0).IsName("attack02")
                || myani.GetCurrentAnimatorStateInfo(0).IsName("backready")
                || myani.GetCurrentAnimatorStateInfo(0).IsName("backup")
                ))
            {
                myani.SetTrigger("spattack");
            }

            //同时K作为特殊攻击，发动向后架势
            if (Input.GetKeyDown(KeyCode.K) && (
                myani.GetCurrentAnimatorStateInfo(0).IsName("idle") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("run") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("roll") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("standby") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("rollstanding")
                ))
            {
                myani.SetTrigger("tback");
            }


            //新增空中K
            //转换啊！这个超级要注意啊啊啊啊啊啊啊啊啊啊啊啊啊啊，窝草啊啊啊啊啊啊啊啊！
            if (!myani.IsInTransition(0) &&
                Input.GetKeyDown(KeyCode.K) && (
                myani.GetCurrentAnimatorStateInfo(0).IsName("jumpup") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("jumptop") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("jumpdown")
                ))
            {
                myani.SetTrigger("tflash");
            }

            //L = 跳跃,生效于待机、奔跑、下落后缓冲，并且需要判定接触地面
            //因为已经限制了起跳状态，所以我尝试取消了落地判定isground == true &&，落地判定仅作为动画控制变量
            if (Input.GetKeyDown(KeyCode.L) &&
                (myani.GetCurrentAnimatorStateInfo(0).IsName("run")
                || myani.GetCurrentAnimatorStateInfo(0).IsName("idle")
                || myani.GetCurrentAnimatorStateInfo(0).IsName("attack01")
                || myani.GetCurrentAnimatorStateInfo(0).IsName("attack02")
                || myani.GetCurrentAnimatorStateInfo(0).IsName("rollstanding")
                || myani.GetCurrentAnimatorStateInfo(0).IsName("standby")))
            {
                myani.SetTrigger("jump");
                JumpingVoice();
                mybody.AddForce(Vector2.up * speedy);
            }

            //AD = 跑步变量，生效于待机、奔跑
            if (Input.GetAxis("Horizontal") != 0 &&
                (myani.GetCurrentAnimatorStateInfo(0).IsName("idle") || myani.GetCurrentAnimatorStateInfo(0).IsName("run"))
                )
            {
                myani.SetBool("isrun", true);
            }
            else
            {
                myani.SetBool("isrun", false);
            }

            //翻滚
            if (Input.GetKeyDown(KeyCode.I) &&
                (myani.GetCurrentAnimatorStateInfo(0).IsName("idle") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("run") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("standby") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("attack01") ||
                myani.GetCurrentAnimatorStateInfo(0).IsName("attack02")))
            {
                myani.SetTrigger("troll");
            }

            //物品互动
            if (MyItem != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //只触发物品事件，其他都不做处理，是否要停止互动是否要打开UI都由物品事件本身决定
                    MyItem.GetComponent<SceneItem>().ItemEvent();
                }
            }
        }
        else
        {
            //WS视角重置
            mycamera.GetComponent<Camerafallow>().Outside_Y = 0;
            //各种状态重置
            myani.SetBool("isrun", false);
        }


        ////////////////测试部分
        //M = 测试受伤
        if (Input.GetKeyDown(KeyCode.M))
        {
            BeAttack();
        }

        //每帧得到动画名字，用于更新模块
        myani.GetCurrentAnimatorStateInfo(1).IsName("Invincible");
    }

    public void BeAttack()
    {
        myani.SetTrigger("tdamage");
        mybody.velocity = new Vector2(transform.right.x * backwardForce.x, transform.up.y * backwardForce.y);
        Hurt_Reset();
    }

    public void Mymove(float move)
    {
        //判断转向，处理精灵
        if (Mathf.Abs(move) > 0)
        {
            Quaternion rot = transform.rotation;
            transform.rotation = Quaternion.Euler(rot.x, Mathf.Sign(move) == 1 ? 0 : 180, rot.z);
        }
        mybody.velocity = new Vector2(move * speedx, mybody.velocity.y);
    }


    //动画事件函数
    //初始动作时需要规避的错误，重置各种特殊攻击触发器
    public void Idle_Reset()
    {
        myani.ResetTrigger("spattack");
        myani.ResetTrigger("tflash");
    }

    //第三段攻击动作需要重置普通攻击的变量，以防止连按
    public void SpAttack_Reset()
    {
        myani.ResetTrigger("tattack");
    }

    //取消后摇跳跃需要避免的错误
    public void Jump_Reset()
    {
        myani.ResetTrigger("spattack");
        myani.ResetTrigger("tattack");
    }

    //受伤事件
    public void Hurt_Reset()
    {
        myani.ResetTrigger("tattack");
        myani.ResetTrigger("spattack");
        myani.ResetTrigger("troll");
        myani.ResetTrigger("tback");
        myani.ResetTrigger("tflash");
        //恢复autospeed
        autospeed = autospeedpro;
        ActionSound.clip = (AudioClip)Resources.Load("击飞", typeof(AudioClip));
        ActionSound.volume = 1f;
        ActionSound.Play();
    }

    //声音部分
    //落地声音动画事件
    public void Jumpstanding()
    {
        ActionSound.clip = (AudioClip)Resources.Load("落地", typeof(AudioClip));
        ActionSound.volume = 1f;
        ActionSound.Play();
    }
    //跳跃动画是一个特殊的单帧循环动画，所以发声调整到按键事件执行
    public void JumpingVoice()
    {
        ActionSound.clip = (AudioClip)Resources.Load("跳跃", typeof(AudioClip));
        ActionSound.volume = 0.2f;
        ActionSound.Play();
    }


    //闪现、二段跳的事件
    public void Flash()
    {
        //重置重力牵引
        mybody.velocity = new Vector2(0, 0);
        //二次起跳力量
        mybody.AddForce(Vector2.up * speedy * 0.7f);
        //声音
        ActionSound.clip = (AudioClip)Resources.Load("后撤步", typeof(AudioClip));
        ActionSound.volume = 0.2f;
        ActionSound.Play();
        //特效
        Instantiate(JumpE, 
            new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), 
            transform.rotation);
    }




    //后撤步的动画
    public void Backup_Start()
    {
        //声音
        ActionSound.clip = (AudioClip)Resources.Load("后撤步", typeof(AudioClip));
        ActionSound.volume = 0.2f;
        ActionSound.Play();
        //规避错误
        myani.ResetTrigger("tattack");
        myani.ResetTrigger("spattack");
        myani.ResetTrigger("jump");
        myani.ResetTrigger("troll");
        //在开始时改变滑行方向,同时调节自动移动速度
        autospeed = autospeedpro / 2;
        if (transform.rotation.y == 0)
        {
            autospeed *= -1;
        }
        else
        {
            
        }
        //最后按照输入为准
        float move = Input.GetAxis("Horizontal");
        if (Mathf.Abs(move) > 0)
        {
            Quaternion rot = transform.rotation;
            transform.rotation = Quaternion.Euler(rot.x, Mathf.Sign(move) == 1 ? 0 : 180, rot.z);
        }
        if (move == 0)
        {

        }
        else if (move > 0)
        {
            autospeed = Mathf.Abs(autospeed) * -1;
        }
        else
        {
            autospeed = Mathf.Abs(autospeed);
        }
    }
    public void Backup_End()
    {
        //恢复autospeed
        autospeed = autospeedpro;
    }



    //翻滚使用的动画事件
    public void Roll_Start()
    {
        //翻滚声、短羽毛拍打声
        ActionSound.clip = (AudioClip)Resources.Load("翻滚", typeof(AudioClip));
        ActionSound.volume = 1f;
        ActionSound.Play();
        //规避错误
        myani.ResetTrigger("tattack");
        myani.ResetTrigger("spattack");
        myani.ResetTrigger("jump");
        myani.ResetTrigger("tback");
        //在开始时改变翻滚方向，默认人物朝向翻滚
        if (transform.rotation.y == 0)
        {

        }
        else
        {
            autospeed *= -1;
        }
        //最后按照输入为准,另外要为动画准备转向
        float move = Input.GetAxis("Horizontal");
        if (Mathf.Abs(move) > 0)
        {
            Quaternion rot = transform.rotation;
            transform.rotation = Quaternion.Euler(rot.x, Mathf.Sign(move) == 1 ? 0 : 180, rot.z);
        }
        if (move == 0)
        {

        }
        else if (move > 0)
        {
            autospeed = Mathf.Abs(autospeed);
        }
        else
        {
            autospeed = Mathf.Abs(autospeed) * -1;
        }
    }
    public void Roll_Stand()
    {
        //恢复autospeed
        autospeed = autospeedpro;
    }


    //留一个空白，关于翻滚后直接攻击的方向控制问题
    //因为使用的是trigger，只有set和reset
    //想要像bool一样使用该怎么办呢？ 
    public void Roll_StandEnd()
    {
        if (true)
        {

        }

    }



    public void Flashdown_Start()
    {
        //翻滚声、短羽毛拍打声
        ActionSound.clip = (AudioClip)Resources.Load("翻滚", typeof(AudioClip));
        ActionSound.volume = 1f;
        ActionSound.Play();
        //规避错误
        myani.ResetTrigger("tattack");
        myani.ResetTrigger("spattack");
        myani.ResetTrigger("jump");
        myani.ResetTrigger("tback");
        //在开始时改变翻滚方向，默认人物朝向翻滚
        autospeed = autospeedpro / 3;
        if (transform.rotation.y == 0)
        {
            autospeed *= 1;
        }
        else
        {
            autospeed *= -1;
        }
    }



    public void Falling_Reset()
    {
        //恢复autospeed
        autospeed = autospeedpro;
        myani.ResetTrigger("tback");
    }

    //斩击回馈，带参数调用StopAnimation
    public void hitback(float time)
    {
        StartCoroutine(StopAnimation(time));
    }

    //斩击卡顿效果
    IEnumerator StopAnimation(float time)
    {
        myani.speed = 0;
        yield return new WaitForSeconds(time);
        myani.speed = 1;
    }

    //第一段普通攻击
    public void Attack01Event()
    {
        //自身发出破空声
        Sound.clip = (AudioClip)Resources.Load("攻击破空", typeof(AudioClip));
        Sound.volume = HitVoiceValue;
        Sound.Play();
        //生成攻击区域
        GameObject myshape = Instantiate(Attack01shape, this.transform.position, transform.rotation);
        //设置攻击力
        myshape.GetComponent<ShapeDestroy>().dmg = atk;
        //设置命中时的效果音
        myshape.GetComponent<ShapeDestroy>().hit = "攻击命中";
        //设置攻击的打击效能
        myshape.GetComponent<ShapeDestroy>().breakpower = "light";
        //设置打击卡顿时间
        myshape.GetComponent<ShapeDestroy>().hitbacktime = 0.08f;
    }

    //第二段普通攻击
    public void Attack02Event()
    {
        Sound.clip = (AudioClip)Resources.Load("攻击破空", typeof(AudioClip));
        Sound.volume = HitVoiceValue;
        Sound.Play();
        GameObject myshape = Instantiate(Attack02shape, this.transform.position, transform.rotation);
        myshape.GetComponent<ShapeDestroy>().dmg = atk;
        myshape.GetComponent<ShapeDestroy>().hit = "攻击命中";
        myshape.GetComponent<ShapeDestroy>().breakpower = "light";
        myshape.GetComponent<ShapeDestroy>().hitbacktime = 0.08f;
    }

    //第三段普通攻击预发声
    public void Attack03EventVoice()
    {
        Sound.clip = (AudioClip)Resources.Load("攻击破空", typeof(AudioClip));
        Sound.volume = HitVoiceValue;
        Sound.Play();
    }

    //第三段普通攻击
    public void Attack03Event()
    {
        GameObject myshape = Instantiate(Attack03shape, this.transform.position, transform.rotation);
        myshape.GetComponent<ShapeDestroy>().dmg = atk;
        myshape.GetComponent<ShapeDestroy>().hit = "攻击命中";
        myshape.GetComponent<ShapeDestroy>().breakpower = "light";
        myshape.GetComponent<ShapeDestroy>().hitbacktime = 0.12f;
    }

    //第三段攻击终结
    public void Attack03FinalEvent()
    {
        GameObject myshape = Instantiate(Attack03shape, this.transform.position, transform.rotation);
        myshape.GetComponent<ShapeDestroy>().dmg = atk;
        myshape.GetComponent<ShapeDestroy>().hit = "攻击命中";
        myshape.GetComponent<ShapeDestroy>().breakpower = "heavy";
        myshape.GetComponent<ShapeDestroy>().hitbacktime = 0.18f;
    }


}
