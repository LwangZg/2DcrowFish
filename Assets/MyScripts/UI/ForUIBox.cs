using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForUIBox : MonoBehaviour {
    public Animator myani;
    public GameObject Player;

    //显示对话框...的组件
    public GameObject UIBoxText;
    //对话框的传送对话
    public string[] ItemString;
    //光标
    public int index;
    public int len;

    // Use this for initialization
    void Awake () {
        //得到组件
        myani = GameObject.Find("UIBox").GetComponent<Animator>();
        Player = GameObject.Find("Player");


        UIBoxText = GameObject.Find("UIBoxText");
        ItemString = null;
        index = 0;


    }
	
	// Update is called once per frame
    //按键控制
	void Update () {
        //触发对话框时即开始，从这里开始界面的操作权全部交给Canvas了
        if (Player.GetComponent<PlayerCtrl>().Online == false)
        {
            //因为存在时差触发性，这里要留意观察一下
            //究竟是传递数据速度的更快
            //还是这里读取 文字 的速度更快

            len = ItemString.Length;
            //不用循环的原因是！！！update本来就是一个天然的循环！！！
            
            //提前显示但是数组不要越界
            if (index < len)
            {
                UIBoxText.GetComponent<Text>().text = ItemString[index];
            }

            //前进对话
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (index < len - 1) 
                {
                    //下一句
                    index++;
                }
                else
                {
                    //退出
                    //关闭UI
                    myani.SetBool("open", false);
                    //角色再次开始活动
                    Invoke("PlayerAwake", 0.3f);
                }
            }
        }
	}

    //动画跟随或者移动（备用）
    private void FixedUpdate()
    {
        
    }

    //延时角色活动，避免冲突
    public void PlayerAwake()
    {
        Player.GetComponent<PlayerCtrl>().Online = true;
    }

}
