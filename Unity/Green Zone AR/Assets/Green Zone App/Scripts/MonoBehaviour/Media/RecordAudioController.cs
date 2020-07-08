using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordAudioController : MonoBehaviour {

    public Animator anim;
    public Animator playButton;

    public Animator RecButton;

    public Text Countdown;
    public Slider RecSlider;

    int recLength = 20;
    int count;
    bool startRec = false;
    public AudioSource audioSource;

    // Use this for initialization
    void Start () {
        playButton.SetTrigger("Disabled");
        count = recLength;
    }
	
	// Update is called once per frame
	void Update () {

        if(startRec)
        {
            count--;

            audioSource = GetComponent<AudioSource>();
            audioSource.clip = Microphone.Start("Built-in Microphone", false, recLength, 44100);

            anim.SetTrigger("animateReel");
            int sec = count/60;
            Countdown.text = sec.ToString();
            RecSlider.value = 1.0f - ((float)sec / (float)recLength);
            Debug.Log(RecSlider.value);
        }

        if(count<=0 && startRec){
            anim.ResetTrigger("animateReel");
            playButton.SetTrigger("Normal");

            startRec = false;
            Microphone.End("Built-in Microphone");
        }
    }

    public void rec(){
        startRec = true;
        count = recLength * 60;
        Debug.Log("StartRec");
    }

    public void audioPlay(){
        audioSource.Play();
        Debug.Log("PlayRec");
    }

    public void audioStop(){
        audioSource.Stop();
        Debug.Log("StopRec");
    }

}
