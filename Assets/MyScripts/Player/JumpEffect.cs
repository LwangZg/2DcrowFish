using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEffect : MonoBehaviour {
    public Animator myani;

    // Use this for initialization
    private void Awake()
    {
        //myani = GetComponent<Animator>();
    }


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EndingEvent()
    {
        Destroy(this.gameObject);
    }
}
