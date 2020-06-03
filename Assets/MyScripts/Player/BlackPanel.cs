using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackPanel : MonoBehaviour {
    public Image mybackground;

    //外部设定
    public float value;

    //跳跃开关
    public bool JumpSwitch;
    public string CName;

	// Use this for initialization
	void Awake () {
        //value基础值是1，必须大于1
        //value = 2.0f;
        mybackground = GetComponent<Image>();
        mybackground.color = new Color(0, 0, 0, value);

        JumpSwitch = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (JumpSwitch == true)
        {
            //淡出
            if (value < 1.1f)
            {
                value += Time.deltaTime*2;
                mybackground.color = new Color(0, 0, 0, value);
            }
            else
            {
                //全屏黑暗后开始跳跃
                Application.LoadLevel(CName);
            }
        }
        else
        {
            //淡入效果
            if (value >= 0)
            {
                value -= Time.deltaTime;
                mybackground.color = new Color(0, 0, 0, value);
            }
        }

    }
}
