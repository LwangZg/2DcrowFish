using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForSmoke : MonoBehaviour {
    //外部设置最大透明度
    public float maxalpha;
    //透明化速度倍率
    public float percent;
    //云雾移动速度
    public float movespeed;
    //云雾移动方向
    public float direction;


    //内部的
    public bool OnSmoke;
    public float alphavalue;
    public bool rising;
    public SpriteRenderer mysprite;
    Vector3 pos0;

    // Use this for initialization
	void Start () {
        OnSmoke = false;
        alphavalue = 0;
        rising = true;
        pos0 = transform.position;
        mysprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (OnSmoke)
        {
            //渐变度调节代码，由浅变深再变浅
            if (rising == true)
            {
                alphavalue += Time.deltaTime * percent;
                if (alphavalue > maxalpha)
                {
                    rising = !rising;
                }
            }
            else
            {
                alphavalue -= Time.deltaTime * percent;
                if (alphavalue <= 0)
                {
                    rising = !rising;
                }
            }
            mysprite.material.color = new Color32(255, 255, 255, (byte)alphavalue);
            //飘动速度
            transform.Translate(movespeed * Time.deltaTime * direction, 0, 0);
            //飘动归零
            if (mysprite.material.color.a <= 0)
            {
                transform.position = pos0;
            }
        }
        else
        {
            mysprite.material.color = new Color32(255, 255, 255, 0);
        }
    }
}
