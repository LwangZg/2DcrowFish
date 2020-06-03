using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveC01 : MonoBehaviour {

    // Use this for initialization
    public GameObject MyPlayer;
    public GameObject MyCamerabox;
    public GameObject BuildPoint1;
    public GameObject BuildPoint2;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("C01BuildPoint") != 0)
        {
            switch (PlayerPrefs.GetInt("C01BuildPoint"))
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
                case 2:
                    print("读档位置2");
                    MyPlayer.transform.position = BuildPoint2.transform.position;
                    MyPlayer.transform.rotation = BuildPoint2.transform.rotation;
                    MyCamerabox.transform.position = new Vector3(
                        MyPlayer.transform.position.x,
                        MyCamerabox.transform.position.y,
                        MyCamerabox.transform.position.z);
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
        //C01测试存档的内容到此关闭
        //记录中的测试按钮存留，玩家的M，怪物的N，互动E，以及剧情触发Q，请检查
        //还有意义系统数据的M

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    PlayerPrefs.SetInt("C01BuildPoint", 1);
        //    print(1);
        //}

        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    PlayerPrefs.SetInt("C01BuildPoint", 2);
        //    print(2);
        //}

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    PlayerPrefs.DeleteAll();
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    print(PlayerPrefs.GetInt("C01BuildPoint"));
        //}
    }
}
