using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForVipDoor : SceneItem {
    public Animator myanim;
    public GameObject Player;
    //public GameObject Center;
    public AudioSource mysound;
    public GameObject MyEbutton;
    public bool Elock;

    public GameObject MyCanvas;

    //内部同步备用
    public float Dis;

    //请在外面调整台词吧
    public string[] MyEventText;

	// Use this for initialization
	void Awake () {
        myanim = GetComponent<Animator>();
        Player = GameObject.Find("Player");
        mysound = Center.GetComponent<AudioSource>();
        MyEbutton = GameObject.Find("EButton");
        Elock = true;
        MyCanvas = GameObject.Find("Canvas");

        //读取记录 0=关闭(默认) 1=打开
        if (PlayerPrefs.GetInt("C01VIPDoor") == 1)
        {
            myanim.SetBool("open", true);
            myanim.SetTrigger("Event");
        }
        else
        {
            //打开后的记录关闭
            //默认的关闭
            //二者等同
        }

        //曾经的字符串数组--->变化时
        //MyEventText = new string[2] { "c", "c++" };

	}
	
	// Update is called once per frame
	void Update () {
        
    }

    //这里要好好地重写触发事件了，原本的作废
    public override void ItemEvent()
    {
        //接近门操作则记录位置，自动保存
        PlayerPrefs.SetInt("C", 1);
        PlayerPrefs.SetInt("C01BuildPoint", 1);
        if (myanim.GetBool("open"))
        {
            //关门
            myanim.SetBool("open", false);
            //剧情读取的规避
            myanim.SetTrigger("EventReturn");
            PlayerPrefs.SetInt("C01VIPDoor", 2);
        }
        else
        {
            //开门
            myanim.SetBool("open", true);
            PlayerPrefs.SetInt("C01VIPDoor", 1);

            //开门时事件处理
            //玩家停止活动，不能再互动物品
            Player.GetComponent<PlayerCtrl>().Online = false;
            //打开UI
            MyCanvas.GetComponent<ForUIBox>().myani.SetBool("open", true);
            //赋予文本
            MyCanvas.GetComponent<ForUIBox>().ItemString = MyEventText;
            //重置光标
            MyCanvas.GetComponent<ForUIBox>().index = 0;
        }
    }


    //Player事件处理
    //public void PlayerEvent()
    //{
    //    if ((Vector2.Distance(Center.transform.position, Player.transform.position)) < Dis)
    //    {
    //        Elock = true;
    //        MyEbutton.GetComponent<ForEButton>().Online = true;
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            //接近门操作则记录位置，自动保存
    //            PlayerPrefs.SetInt("C", 1);
    //            PlayerPrefs.SetInt("C01BuildPoint", 1);

    //            if (myanim.GetBool("open"))
    //            {
    //                //关门
    //                myanim.SetBool("open", false);
    //                //剧情读取的规避
    //                myanim.SetTrigger("EventReturn");
    //                PlayerPrefs.SetInt("C01VIPDoor", 2);
    //            }
    //            else
    //            {
    //                //开门
    //                myanim.SetBool("open", true);
    //                PlayerPrefs.SetInt("C01VIPDoor", 1);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (Elock == true)
    //        {
    //            MyEbutton.GetComponent<ForEButton>().Online = false;
    //            Elock = false;
    //        }
    //    }
    //}

    public void OpenEvent()
    {
        mysound.clip = (AudioClip)Resources.Load("卧室开门声", typeof(AudioClip));
        mysound.volume = 1.0f;
        mysound.Play();
    }

    public void CloseEvent()
    {
        mysound.clip = (AudioClip)Resources.Load("卧室关门声", typeof(AudioClip));
        mysound.volume = 1.0f;
        mysound.Play();
    }

    public void EventOpening()
    {
        myanim.SetBool("open", true);
    }

}
