using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(AudioSource))]
public class AudioController : MonoBehaviour {

    public Text infoText;
    string selectDevice;
    AudioSource audioSource;
    public static float[] samples = new float[512];
    public static float[] freqBand = new float[8];
    public static float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];


    float[] freqBandHeighest = new float[8];
    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

    public static float amplitude, amplitudeBuffer;
    float amplitudeHeighest ;

    // Use this for initialization
    void Start () {

        //Get Audio sorce for processing
        audioSource = GetComponent<AudioSource>();

        // Get list of Microphone devices and print the names to the log
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
        if(Microphone.IsRecording(selectDevice)){
            infoText.text = "Recording Audio on " + selectDevice;
        }
        else if(audioSource.isPlaying){
            // Split audio source into 512 samples
            GetSpectrumAudioSource();
            // Split samples into 8 Frequency Bands
            MakeFrequencyBands();
            // Create gradual peak and rise for frequency band 
            BandBuffer();
            // Normalise bands to 0 - 1 range to use in scene as audio bands
            CreateAudioBand();
            // Get average amplitude 
            GetAmplitude();
        }
        else{
            infoText.text = "Idel";
        }
	}

    public void RecAudio(){

        if(Microphone.devices.Length > 0){
            selectDevice = Microphone.devices[0].ToString();
            audioSource.clip = Microphone.Start(selectDevice, true, 10, 44100);
            //audioSource.clip = Microphone.Start("Built-in Microphone", true, 10, 44100);
            audioSource.loop = true;
            //while(!(Microphone.GetPosition(null)>0)){}
            audioSource.Play();
            Debug.Log("Recording... ");
        }
    }

    public void StopAudio(){

        //AudioSource audioSource = GetComponent<AudioSource>();
        Microphone.End(selectDevice);
        audioSource.Stop();
        Debug.Log("Stop... ");

    }

    public void PlayAudio(){
        //AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
        Debug.Log("Playing... ");
    }

    void GetAmplitude(){
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }
        if(currentAmplitude > amplitudeHeighest)
        {
            amplitudeHeighest = currentAmplitude;
        }

        amplitude = currentAmplitude / amplitudeHeighest;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHeighest;

    }

    void CreateAudioBand()
    {
        for (int i = 0; i < 8; i++)
        {
            if(freqBand[i] > freqBandHeighest[i]){
                freqBandHeighest[i] = freqBand[i];
            }
            audioBand[i] = (freqBand[i] / freqBandHeighest[i]);
            audioBandBuffer[i] = (bandBuffer[i] / freqBandHeighest[i]);
        }
    }

    void GetSpectrumAudioSource(){
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void BandBuffer(){
        for (int g = 0; g < 8; g++)
        {
            //
            if (freqBand[g] > bandBuffer[g])
            {
                bandBuffer[g] = freqBand[g];
                bufferDecrease[g] = 0.05f;
            }
            if (freqBand[g] < bandBuffer[g])
            {
                bandBuffer[g] -= bufferDecrease[g];
                //bufferDecrease[g] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands(){
        /*
         * 22050 / 512 - 43hertz per sample
         * 
         * 20 - 60 hertz
         * 60 - 2500 hertz
         * 250 - 500 hertz
         * 500 - 200 hertz
         * 200 - 4000 hertz
         * 4000 - 6000 hertz
         * 6000 - 20000 hertz
         * 
         * 0 - 2 = 86 hertz
         * 1 - 4 = 172 hertz - 87-258
         * 2 - 8 = 344 hertz - 259-602
         * 3 - 16 = 688 hertz - 603-1290
         * 4 - 32 = 1376 hertz - 1291-2666
         * 5 - 64 = 2752 hertz - 2667-5418
         * 6 - 128 = 5504 hertz - 5419-10922
         * 7 - 256 = 11008 hertz - 10923-21930
         * 510
         * 
         */

        int count = 0;

        for (int i = 0; i < 8; i++){
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if(i == 7){
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++){
                average += samples[count] * (count + 1);
                count++;
            }
            average /= count;

            freqBand[i] = average * 10;
        }

    }

}
