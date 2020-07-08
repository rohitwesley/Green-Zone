using UnityEngine;

namespace MovementTools
{
    [CreateAssetMenu]
    public class MapSettings : ScriptableObject
    {

        public MeshModes meshMode = MeshModes.Plane;
        public MapModes mapMode = MapModes.DetailMap;
        [Range(2, 256)]
        public int meshResolution = 128;
        public int mapResolution = 512;
        public bool autoUpdateMap = true;
        public bool autoUpdateMesh = true;
        public bool autoUpdateMaterial = true;
        public bool isGeneraterMesh = false;
        public bool isNavMeshSurfaceSet = false;

        public float planetRadius = 1;
        public NoiseLayer[] noiseLayers;
        public Vector3[] planeFacesDirections;
        public MinMax elevation3DMinMax;

        public Color mapColor;
        public BiomeSettings biomeSettings;
        public Material mapPlanetMaterial;
        public Material mapTerrainMaterial;

    }

    [System.Serializable]
    public enum MeshModes
    {
        Plane,
        Cube,
        Sphere,
    }

    [System.Serializable]
    public enum MapModes
    {
        DetailMap,
        BiomeMap,
        EcoSystemMap,
        AgentsMap,
    }

    [System.Serializable]
    public enum BiomeModes
    {
        BiomeTypes,
        BiomeElevation,
        BiomeColor,
    }

    [System.Serializable]
    public class NoiseLayer
    {
        public bool enabled = true;
        public bool useFirstLayerAsMask;
        public NoiseSettings noiseSettings;
    }

    [System.Serializable]
    public class BiomeSettings
    {
        public NoiseSettings noiseSettings;
        public float noiseOffset;
        public float noiseStrength;
        public float blendAmount;
        public BiomeModes biomeModes = BiomeModes.BiomeColor;
        public Biome[] biomes;

        [System.Serializable]
        public class Biome
        {
            public enum BiomeType
            {
                Aquatic,
                Snowy,
                Sand,
                Forest,
            }
            public BiomeType biomeType;
            public Gradient mapBiomes;
            public AnimationCurve biomeHeightCurve;
            public Color tint;
            [Tooltip("starting range for this biome")]
            [Range(0, 1)]
            public float startElevation = 0.0f;
            [Tooltip("ending range for this biome\n(always keep greater then startElevation for best results)")]
            [Range(0, 1)]
            public float endElevation = 0.25f;
            [Range(0, 1)]
            public float startLatitude;
            [Range(0, 1)]
            public float tintPercent;

        }
    }

    [System.Serializable]
    public class TerrainTypes
    {
        public Texture2D baseMap;
        public Texture2D maskMap;
        public Texture2D normalMap;
        public Texture2D detailHeightMap;

    }

    [System.Serializable]
    public class TerrainInfo
    {
        public int size;
        public Texture2D textureWalkableMat;
        public TerrainTypes terrainMaterialTextures;
        public Texture2D textureHeightMap;
        public Texture2D textureBiomeMap;
        public Vector3[,] tileCentres;
        public bool[,] walkable;
        public bool[,] shore;
        public int numTiles;
        public int numLandTiles;
        public int numWaterTiles;
        public float waterPercent;
        public bool centralize = true;
        public float waterDepth = .2f;
        public float edgeDepth = .2f;

        public TerrainInfo(int size)
        {
            this.size = size;
            tileCentres = new Vector3[size, size];
            walkable = new bool[size, size];
            shore = new bool[size, size];
            numLandTiles = 0;
            numWaterTiles = 0;
            Debug.Log("Map Initialised");
        }
    }

    [System.Serializable]
    public class MinMax
    {

        public float Min { get; private set; }
        public float Max { get; private set; }

        public MinMax()
        {
            // TODO better way to initialise the min max with starting values
            Min = 0;//float.MinValue;
            Max = 0;//float.MaxValue;
        }

        public void AddValue(float v)
        {
            if (v > Max)
            {
                Max = v;
                //Debug.Log("Max:" + Max);
            }
            if (v < Min)
            {
                Min = v;
                //Debug.Log("Min:" + Min);
            }
        }

        public void ResetValue(float magnitude)
        {
            Min = magnitude;
            Max = magnitude;

        }
    }

}
