using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafallow : MonoBehaviour {
    //请把这个组件挂在测试关卡的摄像机外壳上
    public GameObject player;

    // Use this for initialization
    //因为配合火车上下摇晃，所以提前固定Y轴
    float Posy0;
    float Posz;

    public float Outside_Y;

    public float Delta_Y;

    void Start()
    {
        //初始化
        Outside_Y = 0;
        player = GameObject.Find("Player");
        Posz = transform.position.z;
        //记录Y轴原点
        Posy0 = transform.position.y + Delta_Y;

        //在黑幕期间0.1f后，摄像头瞬间同步
        Invoke("InScene", 0.1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //实时更新人物的x轴坐标
        float Posx = player.transform.position.x;

        //假如加入更新Y，Y轴的更新速度太快了会眩晕

        Posy0 = player.transform.position.y + Delta_Y;
        //float Posynow = Mathf.Lerp(transform.position.y, Posy0, Time.deltaTime * 5);


        //差值移动
        //X
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(Posx, transform.position.y, Posz),
            Time.deltaTime * 4
            );
        //Y
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(transform.position.x, Posy0 + Outside_Y, Posz),
            Time.deltaTime * 1.5f
            );
    }

    public void InScene()
    {
        transform.position = new Vector3(player.transform.position.x, Posy0, Posz);
    }
}
