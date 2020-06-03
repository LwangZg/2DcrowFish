using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProCtrl : Enemy {
    public Animator myani;
    public Rigidbody2D mybody;

    //攻击目标，即玩家
    public GameObject target;

    //击退后座力
    public Vector2 backwardForce;

    //跳跃力
    public float jumppower;

    //内置状态机记录标签
    public State mystate;

    //内置状态机
    public enum State
    {
        Moveable,
        Moveunable,
        Damaged,
    }

    // Use this for initialization
    void Start () {
        myani = GetComponent<Animator>();
        mybody = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {

        //mybody.velocity = new Vector2(0, mybody.velocity.y);

        if (Input.GetKeyDown(KeyCode.N))
        {
            myani.SetTrigger("tjump");
            mybody.AddForce(Vector2.up * jumppower);
        }


        BeDestroy();
	}

    public override void Beattack(float dmg, string breakpower)
    {
        if (breakpower == "heavy")
        {
            //震动
            Camera.main.GetComponent<Animator>().SetTrigger("theavy");
            //破除硬直度后击飞
            myani.SetTrigger("tdamage");
            //确定被攻击方向
            Myturn();
            //击飞
            mybody.velocity = new Vector2(transform.right.x * backwardForce.x, transform.up.y * backwardForce.y);
        }
        else
        {
            Camera.main.GetComponent<Animator>().SetTrigger("tlight");
        }
        hp -= dmg;
    }

    //斩击回馈，带参数调用StopAnimation
    public override void hitback(float time)
    {
        StartCoroutine(StopAnimation(time));
    }

    //斩击卡顿效果
    IEnumerator StopAnimation(float time)
    {
        myani.speed = 0;
        yield return new WaitForSeconds(time);
        myani.speed = 1;
    }


    //转向函数，请在各种地方尽情使用吧
    public void Myturn()
    {
        if (target.transform.position.x > transform.position.x)
        {
            Quaternion rot = transform.rotation;
            transform.rotation = Quaternion.Euler(rot.x, 0, rot.z);
        }
        else
        {
            Quaternion rot = transform.rotation;
            transform.rotation = Quaternion.Euler(rot.x, 180, rot.z);
        }
    }

    //碰撞伤害
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerCtrl>().BeAttack();
        }
    }

}
