using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    //基类中包含的元素、hp
    public float hp;


	void Start () {
        hp = 200;
	}
	

	void Update () {
        BeDestroy();
	}

    //用于重写的被攻击函数
    public virtual void Beattack(float dmg, string breakpower)
    {       
        hp -= dmg;
        //print("我受伤了！还剩 HP：" + hp);
        //同时还有打击效能呢
    }

    //用于检测死亡消灭的函数，于Update中调用
    public virtual void BeDestroy()
    {
        if (hp <= 0)
        {
            //print("我已死亡！");
            Destroy(this.gameObject);
        }
    }

    public virtual void hitback(float time)
    {

    }
}
