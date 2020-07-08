using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioOscillator : MonoBehaviour {

    public double bpm = 140.0F;
    private double gain = 0.0F;
    public double volume = 0.5F;
    public double noteFrequency = 440.0F;
    public int signatureHi = 4;
    public int signatureLo = 4;

    private double nextTick = 0.0F;
    public double amp = 1.0F;
    public double phase = 0.0F;
    public double sampleRate = 48000.0F;
    private int accent;
    private bool running = false;

    public double[] noteOctive;
    public int thisnoteFrequency;

    void Start(){

        accent = signatureHi;
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick * sampleRate;
        running = true;

        noteOctive = new double[8];
        noteOctive[0] = 440.0F;
        noteOctive[1] = 494.0F;
        noteOctive[2] = 554.0F;
        noteOctive[3] = 587.0F;
        noteOctive[4] = 659.0F;
        noteOctive[5] = 740.0F;
        noteOctive[6] = 831.0F;
        noteOctive[7] = 880.0F;

    }

    void Update(){

        if (gain == volume && Input.GetKeyDown(KeyCode.A))
        {
            thisnoteFrequency = 0;
            noteFrequency = noteOctive[thisnoteFrequency];
        }
        if (gain == volume && Input.GetKeyDown(KeyCode.S))
        {
            thisnoteFrequency = 1;
            noteFrequency = noteOctive[thisnoteFrequency];
        }
        if (gain == volume && Input.GetKeyDown(KeyCode.D))
        {
            thisnoteFrequency = 2;
            noteFrequency = noteOctive[thisnoteFrequency];
        }
        if (gain == volume && Input.GetKeyDown(KeyCode.F))
        {
            thisnoteFrequency = 3;
            noteFrequency = noteOctive[thisnoteFrequency];
        }
        if (gain == volume && Input.GetKeyDown(KeyCode.G))
        {
            thisnoteFrequency = 4;
            noteFrequency = noteOctive[thisnoteFrequency];
        }
        if (gain == volume && Input.GetKeyDown(KeyCode.H))
        {
            thisnoteFrequency = 5;
            noteFrequency = noteOctive[thisnoteFrequency];
        }
        if (gain == volume && Input.GetKeyDown(KeyCode.J))
        {
            thisnoteFrequency = 6;
            noteFrequency = noteOctive[thisnoteFrequency];
        }
        if (gain == volume && Input.GetKeyDown(KeyCode.K))
        {
            thisnoteFrequency = 7;
            noteFrequency = noteOctive[thisnoteFrequency];
        }

    }

    void OnAudioFilterRead(float[] data, int channels){

        if (!running)
            return;

        double samplesPerTick = sampleRate * 60.0F / bpm * 4.0F / signatureLo;
        double sample = AudioSettings.dspTime * sampleRate;
        int dataLen = data.Length / channels;

        int n = 0;
        double note = noteFrequency * 2.0 * Mathf.PI / sampleRate;

        while (n < dataLen)
        {
            double sinWave = gain * (double)Mathf.Sin((float)phase) * amp;

            double squareWave = -gain * amp;
            if(Mathf.Sin((float)phase) >= 0){
                squareWave = gain * amp;
            }

            double triangleWave = gain * (double)Mathf.PingPong((float)phase, 1.0F) * amp;

            int i = 0;
            while (i < channels)
            {
                data[n * channels + i] += (float)sinWave;
                i++;
            }

            while (sample + n >= nextTick)
            {
                nextTick += samplesPerTick;
                amp = 1.0F;
                if (++accent > signatureHi)
                {
                    accent = 1;
                    //amp *= 0.0F;
                    amp = 1.0F;
                }
                Debug.Log("Tick: " + accent + "/" + signatureHi);
            }

            phase += note * amp;// * 0.3F;
            //amp *= 0.993F;
            if (phase > (Mathf.PI * 2))
                phase = 0.0f;
            n++;
        }


    }

    public void playNote(){
        if(gain==volume){
            gain = 0.0;
            noteFrequency = noteOctive[thisnoteFrequency];
            thisnoteFrequency++;
            thisnoteFrequency = thisnoteFrequency % noteOctive.Length;
        }
        else{
            gain = volume;
        }
    }

}
