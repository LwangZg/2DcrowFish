using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThrow : MonoBehaviour {
    public GameObject main;
    // Use this for initialization
    public float dxtime;
    private float counttime;
    public float dx;
    public bool MySwitch;
	void Awake () {
        counttime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (MySwitch == true)
        {
            counttime += Time.deltaTime;
            if (counttime >= dxtime)
            {
                counttime = 0;
                CameraMove(dx);
            }
        }
	}

    public void CameraMove(float dx)
    {
        main.transform.position = new Vector3(main.transform.position.x, main.transform.position.y + dx, main.transform.position.z);
    }
}
