using UnityEngine;

[System.Serializable]
public class NoiseSettings {
    public enum FilterType {
        SimplexNoise,
        RigidNoise,
        HeightMap
    }
    public FilterType filterType;
    [ConditionalHide("filterType, 0")]
    public SimplexNoiseSettings simplexNoiseSettings;
    [ConditionalHide("filterType, 1")]
    public RigidNoiseSettings rigidNoiseSettings;
        
    [System.Serializable]
    public class SimplexNoiseSettings
    {
        public int seed;
        public float strength = 1;
        //TODO give a better name confuses with the number  of noise layers added
        [Range (1, 8)]
        public int numLayers = 4;
        public float baseRoughness = 1;
        public float roughness = 2;
        public float persistence = 0.5f;
        public Vector3 centre;
        public float minValue = 0.5f;

    }
        
    [System.Serializable]
    public class RigidNoiseSettings : SimplexNoiseSettings
    {
        public float weightMultiplier = 0.8f;
    }


    // public float lacunarity = 2;
    // public float scale = 1;
    // public Vector2 offset;

}