using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveC02 : MonoBehaviour {
    public GameObject MyPlayer;
    public GameObject MyCamerabox;

    public GameObject BuildPoint1;

    // Use this for initialization
    private void Awake()
    {
        //得到玩家和摄像机盒子
        MyPlayer = GameObject.Find("Player");
        MyCamerabox = GameObject.Find("MainCameraBox");

        //读取记录
        if (PlayerPrefs.GetInt("C02BuildPoint") != 0)
        {
            switch (PlayerPrefs.GetInt("C02BuildPoint"))
            {
                case 1:
                    print("读档位置1");
                    MyPlayer.transform.position = BuildPoint1.transform.position;
                    MyPlayer.transform.rotation = BuildPoint1.transform.rotation;
                    MyCamerabox.transform.position = new Vector3(
                        MyPlayer.transform.position.x,
                        MyCamerabox.transform.position.y,
                        MyCamerabox.transform.position.z);
                    break;
                    break;
            }
        }
        else
        {
            print("玩家起始地点无记录自动初始化");
            MyPlayer.transform.position = BuildPoint1.transform.position;
            MyPlayer.transform.rotation = BuildPoint1.transform.rotation;
            MyCamerabox.transform.position = new Vector3(
                MyPlayer.transform.position.x,
                MyCamerabox.transform.position.y,
                MyCamerabox.transform.position.z);
        }

    }


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
