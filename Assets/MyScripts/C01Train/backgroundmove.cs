using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundmove : MonoBehaviour {
    public GameObject sky01;
    Vector3 p1;
    public GameObject sky02;
    Vector3 p2;
    public float speed;

    //完全进站
    public bool OnStation;
    //开始停靠
    public bool SlowDown;


    //音箱获取，来自主摄像机
    public AudioSource mysound;
    //停车音效储存
    public float musiclength;
    //运行的代码组

    private AudioClip mymusic;
	// Use this for initialization
	void Start () {
        OnStation = false;
        SlowDown = false;
        musiclength = 0;

        GameObject tmp = GameObject.Find("Main Camera");
        mysound = tmp.GetComponent<AudioSource>();

        //记录位置
        p1 = sky01.transform.position;
        p2 = sky02.transform.position;

        //预先加载
        if (PlayerPrefs.GetInt("C01MainDoor") == 1)
        {
            OnStation = true;
            transform.GetComponent<CameraThrow>().MySwitch = false;
        }
        else
        {
            //开启抖动
            transform.GetComponent<CameraThrow>().MySwitch = true;
            //播放背景音乐
            mysound.Play();
            //预先加载停车音乐
            mymusic = (AudioClip)Resources.Load("火车停车2", typeof(AudioClip));
        }
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    //这个是控制外面布景的开关
    public void DoorEvent()
    {
        SlowDown = true;
        mysound.Stop();
        mysound.loop = false;
        mysound.clip = mymusic;
        mysound.volume = 0.5f;
        musiclength = mysound.clip.length;
        //开始播放停车音效
        mysound.Play();

        //存档处理，为防止动画未播放完退出游戏，提前储存
        //关卡记录第一关
        PlayerPrefs.SetInt("C", 1);
        //记录门的状态为打开
        PlayerPrefs.SetInt("C01MainDoor", 1);

        //同时阻止门的物品提示
        //因为那个寻找是一开始只找一次，所以这个方法是行不通了
        //GameObject.Find("train_maindoor").tag = "Untagged";
    }



    private void FixedUpdate()
    {
        if (OnStation)
        {
            //完全停靠后不执行

        }
        else
        {
            //只要没完全停靠，请一直执行位移
            if (sky02.transform.position.x <= p1.x)
            {
                sky01.transform.position = p1;
                sky02.transform.position = p2;
            }
            sky01.transform.Translate(speed * Time.deltaTime, 0, 0);
            sky02.transform.Translate(speed * Time.deltaTime, 0, 0);
            if (SlowDown)
            {
                speed += Time.deltaTime * 0.1f;
                if (speed > 0)
                {
                    speed = 0;
                    //关闭背景横向移动
                    OnStation = true;
                    //关闭镜头上下晃动
                    transform.GetComponent<CameraThrow>().MySwitch = false;
                    //不按整数移动后摄像机出现像素拉伸,因此再次对坐标进行四舍五入
                    //(float)(Mathf.Round(number * 1000)) / 1000
                    sky01.transform.position = new Vector2(
                        (float)(Mathf.Round(sky01.transform.position.x * 100)) / 100,
                        (float)(Mathf.Round(sky01.transform.position.y * 100)) / 100);
                    sky02.transform.position = new Vector2(
                        (float)(Mathf.Round(sky02.transform.position.x * 100)) / 100,
                        (float)(Mathf.Round(sky02.transform.position.y * 100)) / 100);
                    //秒后打开闸门
                    Invoke("AfterEvent", 2.0f);
                }
            }
        }
    }

    public void AfterEvent()
    {
        //自动打开闸门
        GameObject.Find("train_maindoor").GetComponent<ForMainDoor>().myanim.SetBool("open", true);
    }
}
