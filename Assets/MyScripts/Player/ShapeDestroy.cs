using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeDestroy : MonoBehaviour {
    //一帧后消失
    public float count = 1;
    //传递伤害量
    public float dmg;
    //攻击发起者
    public GameObject Attacker;
    //获取不会消失的音响
    public AudioSource Sound;
    //音效文件
    public string hit;
    //打击效能
    public string breakpower;
    //打击特效
    public GameObject MyEffect;
    //打击卡顿时间
    public float hitbacktime;
    //权重,用于计算特效生成位置
    public float enemyweight;

    // Use this for initialization
    void Awake () {
        Attacker = GameObject.Find("Player");
        Sound = Attacker.GetComponent<PlayerCtrl>().Sound;
	}

    void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (count == 0)
        {
            Destroy(this.gameObject);
        }
        count -= 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            //播放击中音效
            Sound.clip = (AudioClip)Resources.Load(hit, typeof(AudioClip));
            //Sound.volume = 1f;
            Sound.Play();
            //造成伤害，判断是否大打击
            collision.GetComponent<Enemy>().Beattack(dmg, breakpower);
            //生成打击效果位置,距离是二者之间的权重,方向涉及到身前身后的范围攻击，应重新判断
            float x1 = collision.transform.position.x;
            float x2 = Attacker.transform.position.x;
            float x = (x1 * enemyweight + x2 * (2 - enemyweight)) / 2;
            Vector3 point = new Vector3(x, collision.transform.position.y, collision.transform.position.z);
            Quaternion rot;
            if (x1 > x2)
            {
                rot = new Quaternion(0, 0, 0, 0);
            }
            else
            {
                rot = new Quaternion(0, 180, 0, 0);
            }
            Instantiate(MyEffect, point, rot);
            //击中后卡顿
            Attacker.GetComponent<PlayerCtrl>().hitback(hitbacktime);
            collision.GetComponent<Enemy>().hitback(hitbacktime);
        }
    }
}
