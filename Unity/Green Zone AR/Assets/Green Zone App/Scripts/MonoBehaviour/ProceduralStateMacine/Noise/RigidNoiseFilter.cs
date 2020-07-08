using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    NoiseSettings.RigidNoiseSettings _settings;
    SimplexNoise _noiseSimplex = new SimplexNoise();
    Noise _noise = new Noise();

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        this._settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for(var i = 0; i < _settings.numLayers; i++)
        {
            Vector3 noisePoint = point * frequency + _settings.centre;
            // float v = (float)noise.Evaluate(noisePoint.x,noisePoint.y,noisePoint.z);
            float v = _noiseSimplex.Evaluate(noisePoint);
            // float v =  Mathf.PerlinNoise(noisePoint.x,noisePoint.z);
            
            v = 1 - Mathf.Abs(v);
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * _settings.weightMultiplier);
            noiseValue += v * amplitude;

            frequency *= _settings.roughness;
            amplitude *= _settings.persistence;

        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.minValue); 
        return noiseValue * _settings.strength;
    }

}
