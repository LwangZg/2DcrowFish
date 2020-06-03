using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForMainDoor : SceneItem
{

    public Animator myanim;
    public GameObject Player;
    //public GameObject Center;
    public AudioSource mysound;
    public GameObject MyEButton;
    public bool Elock;

    //内部同步备用
    public float Dis;

    public GameObject[] mysmoke;

    //请在外面调整台词吧
    public string[] MyEventText;


    // Use this for initialization
    void Awake()
    {
        myanim = GetComponent<Animator>();
        Player = GameObject.Find("Player");
        mysound = Center.GetComponent<AudioSource>();
        MyEButton = GameObject.Find("EButton");
        Elock = true;

        //读取是否有打开，基本开一次了就永远打开了,由剧情开或者有过场开，基本不经过手动操作
        if (PlayerPrefs.GetInt("C01MainDoor") == 1)
        {
            myanim.SetBool("open", true);
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerEvent();
    }

    public override void ItemEvent()
    {
        //检查门是否打开了
        if (myanim.GetBool("open"))
        {
            //在开门状态下进入下一个场景
            //记录进入下一个场景C02
            PlayerPrefs.SetInt("C", 2);
            //记录下一个场景的出生位置
            PlayerPrefs.SetInt("C02BuildPoint", 1);
            //场景转移
            GameObject tmp = GameObject.Find("BlackPanel");
            tmp.GetComponent<BlackPanel>().JumpSwitch = true;
            tmp.GetComponent<BlackPanel>().CName = "C02";
        }
        else
        {
            if (Elock == true)
            {
                //只执行一次
                GameObject.Find("RunScripts").GetComponent<backgroundmove>().DoorEvent();
                //触发对话框
                //玩家停止活动，不能再互动物品
                Player.GetComponent<PlayerCtrl>().Online = false;
                GameObject MyCanvas = GameObject.Find("Canvas");
                //打开UI
                MyCanvas.GetComponent<ForUIBox>().myani.SetBool("open", true);
                //赋予文本
                MyCanvas.GetComponent<ForUIBox>().ItemString = MyEventText;
                //重置光标
                MyCanvas.GetComponent<ForUIBox>().index = 0;
                Elock = false;
            }

        }
    }

    //Player事件处理
    //public void PlayerEvent()
    //{
    //    //检查操作者是否在附近
    //    if ((Vector2.Distance(Center.transform.position, Player.transform.position)) < Dis
    //        &&myanim.GetBool("open")
    //        )
    //    {
    //        Elock = true;
    //        MyEButton.GetComponent<ForEButton>().Online = true;
    //        //按E触发
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            //在开门状态下进入下一个场景
    //            //记录进入下一个场景C02
    //            PlayerPrefs.SetInt("C", 2);
    //            //记录下一个场景的出生位置
    //            PlayerPrefs.SetInt("C02BuildPoint", 1);
    //            //场景转移
    //            GameObject tmp = GameObject.Find("BlackPanel");
    //            tmp.GetComponent<BlackPanel>().JumpSwitch = true;
    //            tmp.GetComponent<BlackPanel>().CName = "C02";
    //        }
    //    }
    //    else
    //    {
    //        if (Elock == true)
    //        {
    //            MyEButton.GetComponent<ForEButton>().Online = false;
    //            Elock = false;
    //        }
    //    }
    //}


    public void OpenEvent()
    {
        mysound.clip = (AudioClip)Resources.Load("蒸汽喷发", typeof(AudioClip));
        mysound.volume = 1.0f;
        mysound.Play();
    }

    public void OpenEvent2()
    {
        mysmoke[0].GetComponent<ForSmoke>().OnSmoke = true;
        mysmoke[1].GetComponent<ForSmoke>().OnSmoke = true;
        mysmoke[2].GetComponent<ForSmoke>().OnSmoke = true;
    }
}
