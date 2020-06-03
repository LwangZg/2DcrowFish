using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {


	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.J))
        {
            NewGame();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            ContinueGame();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.DeleteAll();
        }
	}

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        Application.LoadLevel("C01");
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.GetInt("C") != 0)
        {
            string tmp = "C0" + PlayerPrefs.GetInt("C").ToString();
            print(tmp);
            Application.LoadLevel(tmp);
        }
        else
        {
            print("没有存档");
        }

    }
}
