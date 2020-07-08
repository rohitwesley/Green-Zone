using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplexNoiseFilter : INoiseFilter
{

    NoiseSettings.SimplexNoiseSettings _settings;
    SimplexNoise _noiseSimplex = new SimplexNoise();
    Noise _noise = new Noise();

    public SimplexNoiseFilter(NoiseSettings.SimplexNoiseSettings settings)
    {
        this._settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _settings.baseRoughness;
        float amplitude = 1;
        for(var i = 0; i < _settings.numLayers; i++)
        {
            Vector3 noisePoint = point * frequency + _settings.centre;
            // float v = (float)noise.Evaluate(noisePoint.x,noisePoint.y,noisePoint.z);
            float v = _noiseSimplex.Evaluate(noisePoint);
            // float v =  Mathf.PerlinNoise(noisePoint.x,noisePoint.z);
            
            noiseValue += (v + 1) * 0.5f * amplitude;
            
            frequency *= _settings.roughness;
            amplitude *= _settings.persistence;

        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.minValue); 
        return noiseValue * _settings.strength;
    }

    //TODO find a better place to put this
    public float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int octaves, float persistance, float lacunarity)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        if(scale <= 0)
        {
            scale = 0.0001f;
        }
        for(var y = 0; y < mapHeight; y++)
        {
            for(var x = 0; x < mapWidth; x++)
            {
                // int i = x + y * _mapSettings.resolution;
                // float sampleX = x / scale;
                // float sampleY = y / scale;
                // float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                // noiseMap[x, y] = perlinValue;
                // float simplexValue = (float)noise.Evaluate(sampleX,sampleY);
                // noiseMap[x, y] = simplexValue;

                Vector3 sample = new Vector3 (x / scale, y / scale);
                noiseMap[x, y] = Evaluate(sample);
            }
        }
        return noiseMap;

    }

}
