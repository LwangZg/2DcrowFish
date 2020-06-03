using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForEButton : MonoBehaviour {
    public GameObject MyPlayer;
    public SpriteRenderer myimage;
    // Use this for initialization

    public bool Online;

    private void Awake()
    {
        MyPlayer = GameObject.Find("Player");
        myimage = GetComponent<SpriteRenderer>();
        Online = false;
        //0 = 隐藏 255 = 出现
        myimage.color = new Color32(255, 255, 255, 0);
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        transform.position = new Vector3(
            MyPlayer.transform.position.x + 0.15f,
            MyPlayer.transform.position.y + 0.1f,
            transform.position.z
            );
        if (Online)
        {
            myimage.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            myimage.color = new Color32(255, 255, 255, 0);
        }
    }
}
