using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(AudioSource))]
public class VideoController : MonoBehaviour {

    VideoPlayer videoPlayer;
    VideoClip videoClip;
    AudioSource audioSource;
    [SerializeField] GameObject webCam;
    static WebCamTexture webcamTexture;
    string selectDevice;
    [SerializeField] Text infoText;

    // Use this for initialization
    void Start () {

        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();

        videoClip = videoPlayer.clip;

        // Play on awake defaults to true. Set it to false to avoid the url set
        // below to auto-start playback since we're in Start().
        videoPlayer.playOnAwake = false;

        // Skip the first 100 frames.
        videoPlayer.frame = 1;

        // Restart from beginning when done.
        videoPlayer.isLooping = false;

        // Get list of Microphone devices and print the names to the log
        foreach (var device in WebCamTexture.devices)
        {
            Debug.Log("Name: " + device);
        }
        if (WebCamTexture.devices.Length > 0) {
            webcamTexture = new WebCamTexture();
        }

    }
	
	// Update is called once per frame
	void Update () {

        if (webcamTexture.isPlaying)
        {
            infoText.text = "Recording Video on " + selectDevice;
            Debug.Log("Recording Video on " + selectDevice);
        }
        else
        {
            infoText.text = "Idel";
        Debug.Log("Idel");
        }

    }

    public void RecVideo(){

        if (WebCamTexture.devices.Length > 0)
        {
            selectDevice = WebCamTexture.devices[0].ToString();
            webCam.GetComponent<Renderer>().material.mainTexture = webcamTexture;
            webCam.GetComponent<Renderer>().material.SetTexture("_EmissionMap", webcamTexture);
            webcamTexture.Play();
            Debug.Log("Recording... ");
        }

    }

    public void playVideo(){

        if (webcamTexture.isPlaying)
        {
            webcamTexture.Pause();
        }
        // Start playback. This means the VideoPlayer may have to prepare (reserve
        // resources, pre-load a few frames, etc.). To better control the delays
        // associated with this preparation one can use videoPlayer.Prepare() along with
        // its prepareCompleted event.
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            videoPlayer.Play();
        }
        Debug.Log("Play... ");
    }

    public void stopVideo(){
        if (webcamTexture.isPlaying)
        {
            webcamTexture.Stop();
        }
        videoPlayer.Stop();
        Debug.Log("Stop... ");
    }

}
