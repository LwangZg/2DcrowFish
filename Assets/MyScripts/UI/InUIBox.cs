using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InUIBox : MonoBehaviour {
    public AudioSource mysound;
    public AudioClip mymusic1;
    public AudioClip mymusic2;
    // Use this for initialization
    void Awake () {
        mysound = GameObject.Find("UIBox").GetComponent<AudioSource>();
        //缓存音效
        mymusic1 = (AudioClip)Resources.Load("换弹01", typeof(AudioClip));
        mymusic2 = (AudioClip)Resources.Load("换弹02", typeof(AudioClip));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Sound1()
    {
        mysound.clip = mymusic1;
        mysound.Play();
    }

    public void Sound2()
    {
        mysound.clip = mymusic2;
        mysound.Play();
    }
}
