using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterGenerator
{
    public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
    {
        switch(settings.filterType)
        {
            case NoiseSettings.FilterType.SimplexNoise :
                return new SimplexNoiseFilter(settings.simplexNoiseSettings);
            case NoiseSettings.FilterType.RigidNoise :
                return new RigidNoiseFilter(settings.rigidNoiseSettings);
        }
        return null;
        
    }
}
