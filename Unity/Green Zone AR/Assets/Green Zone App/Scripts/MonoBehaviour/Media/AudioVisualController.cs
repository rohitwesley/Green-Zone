using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualController : MonoBehaviour {

    public GameObject audioSampleObject;
    GameObject[] audioSampleList = new GameObject[512];
    public GameObject[] audioBandList = new GameObject[8];
    public float maxScale = 10;
    public float maxBandScale = 10;
    public float circleRadious = 0.1F;
    public bool useBandBuffer;


    // Use this for initialization
    void Start () {

        for (int i = 0; i < 512; i++)
        {
            GameObject instanceaudioSample = (GameObject)Instantiate(audioSampleObject);
            instanceaudioSample.transform.position = this.transform.position;
            instanceaudioSample.transform.parent = this.transform;
            instanceaudioSample.name = "AudioSample" + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            instanceaudioSample.transform.position = Vector3.forward * 150;
            //instanceaudioSample.transform.localScale = audioSampleObject.transform.localScale;
            audioSampleList[i] = instanceaudioSample;
        }

        Vector3 locscale = this.transform.localScale;
        this.transform.localScale = new Vector3(locscale.x*circleRadious, locscale.y*circleRadious, locscale.z*circleRadious);
        audioSampleObject.SetActive(false);

        //for (int i = 0; i < 8; i++)
        //{
        //    GameObject instanceaudioSample = (GameObject)Instantiate(audioSampleObject);
        //    instanceaudioSample.transform.position = this.transform.position;
        //    instanceaudioSample.transform.parent = this.transform;
        //    instanceaudioSample.name = "AudioBand" + i;
        //    this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
        //    //Vector3 pos = instanceaudioSample.transform.position;
        //    //instanceaudioSample.transform.position.Set(pos.x + (i * 20), pos.y, pos.z);

        //    instanceaudioSample.transform.position = Vector3.forward * 50;
        //    audioBandList[i] = instanceaudioSample;
        //}

    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < 512; i++)
        {
            if (audioSampleList[i] != null)
            {
                Vector3 locscale = audioSampleList[i].transform.localScale;
                audioSampleList[i].transform.localScale = new Vector3(locscale.x, (AudioController.samples[i] * maxScale) + 2, locscale.z);
            }
        }
        for (int i = 0; i < 8; i++){
            if (audioBandList[i] != null)
            {
                Vector3 locscale = audioBandList[i].transform.localScale;
                if(useBandBuffer){
                    audioBandList[i].transform.localScale = new Vector3(locscale.x, (AudioController.bandBuffer[i] * maxBandScale) + 2, locscale.z);
                    Color bandColor = new Color(AudioController.audioBandBuffer[i], AudioController.audioBandBuffer[i], AudioController.audioBandBuffer[i]);
                    Renderer bandObjectRenderer;
                    bandObjectRenderer = audioBandList[i].GetComponent<Renderer>();
                    Material bandMaterial = bandObjectRenderer.material;
                    //bandMaterial.SetColor("EmissionColor", bandColor);
                    bandMaterial.color = Color.Lerp(Color.green, Color.blue, AudioController.audioBandBuffer[i]);
                }
                else
                {
                    audioBandList[i].transform.localScale = new Vector3(locscale.x, (AudioController.freqBand[i] * maxBandScale) + 2, locscale.z);
                    Color bandColor = new Color(AudioController.audioBand[i], AudioController.audioBand[i], AudioController.audioBand[i]);
                    Renderer bandObjectRenderer;
                    bandObjectRenderer = audioBandList[i].GetComponent<Renderer>();
                    Material bandMaterial = bandObjectRenderer.material;
                    //bandMaterial.SetColor("EmissionColor", bandColor);
                    bandMaterial.color = Color.Lerp(Color.green, Color.blue, AudioController.audioBand[i]);
                }

            }
        }
    }
       
}
