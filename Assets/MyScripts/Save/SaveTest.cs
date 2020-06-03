using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayerPrefs.SetInt("Buildpoint", 1);
            PlayerPrefs.SetString("Name", "karas");
            print("储存1OK");

        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            PlayerPrefs.DeleteAll();
            print("已全部删除");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            print(PlayerPrefs.GetInt("Buildpoint"));
            print(PlayerPrefs.GetString("Name"));

            if (PlayerPrefs.GetString("Name") == "") 
            {
                print("内容为空");
            }

        }




    }
}
