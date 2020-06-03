using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForMP : MonoBehaviour {
    //用来显示MP的文本框
    public Text MPValue;

    //过度作用MP
    public float MidMP;

    //实际MP
    public float MP;


    // Use this for initialization
    private void Awake()
    {
        //赋予一个初始值
        MP = 17500;
        MidMP = 0;        
    }


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //按键永远测试在update
        if (Input.GetKeyDown(KeyCode.M))
        {
            MP -= 100;
        }
    }


    //所以更新也是永远放在fixedupdate
    //实际上依然能使用线性差值来表示，就如之前用来移动镜头的函数
    private void FixedUpdate()
    {
        //显示，实时更新
        //初始数值，目标数值，变化速度
        MidMP = Mathf.Lerp(MidMP, MP, Time.deltaTime);
        MPValue.text = ((int)MidMP).ToString();

        //变化
        MP -= Time.deltaTime;
    }
}
