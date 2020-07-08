using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementTools
{
    public class MapGenerator : MonoBehaviour
    {
        [HideInInspector]
        public bool foldout;

        [Tooltip("Map Settings")]
        public MapSettings mapSettings;
        [Tooltip("Local Up Axis")]
        [SerializeField] private Vector3 localUp;
        [Tooltip("Renderer to draw map texture too")]
        [SerializeField] private Renderer textureRenderMap;
        [Tooltip("Renderer to draw map view texture too")]
        [SerializeField] private Renderer textureRenderMapView;
        [Tooltip("Camera/Players View Cliping Range")]
        [SerializeField] private Vector2 viewScale;
        [Tooltip("Camera/Players View Zoom")]
        [Range(-20.0f,20.0f)]
        [SerializeField] private float viewZoom;
        [Tooltip("Camera/Players View Position")]
        [SerializeField] private Vector2 viewPosition;

        MinMax elevation3DMinMax;
        Texture2D textureMap;
        Texture2D textureMapView;
        Texture2D textureEcoMap;
        Texture2D textureEcoMapPawns;

        private void Start()
        {
            GenerateMap();
        }

        private void Update()
        {
            OnObjectSettingsUpdated();
        }

        public void OnObjectSettingsUpdated()
        {
            if (mapSettings.autoUpdateMap)
            {
                UpdateMapView();
            }
        }

        private void GetEleviationRange()
        {
            textureRenderMap.sharedMaterial = Instantiate(mapSettings.mapTerrainMaterial);
            textureRenderMapView.sharedMaterial = Instantiate(mapSettings.mapTerrainMaterial);

            Vector3[] _map3D = ShapeGenerator.Generate3DMapArray(mapSettings, mapSettings.meshResolution, localUp);
            elevation3DMinMax = ShapeGenerator.GetElivationRange(_map3D, mapSettings.meshResolution);
            Debug.Log("elevation3DMinMax:" + elevation3DMinMax.Min + ":" + elevation3DMinMax.Max);
        }

        public void GenerateMap()
        {
            GetEleviationRange();
            UpdateMap();
            UpdateMapView();
        }

        public void UpdateMap()
        {
            int width = mapSettings.mapResolution;
            int height = mapSettings.mapResolution;
            Vector2 mapPosition = new Vector2();
            mapPosition.Set(0.0f, 0.0f);
            
            if (mapSettings.mapMode.HasFlag(MapModes.DetailMap))
            {
                textureRenderMap.sharedMaterial.color = mapSettings.mapColor;
                Texture2D textureMap = ShapeGenerator.GenerateMap(mapSettings, localUp, width, height, mapPosition, 1.0f);
                textureRenderMap.sharedMaterial.mainTexture = textureMap;
            }
            if (mapSettings.mapMode.HasFlag(MapModes.BiomeMap))
            {
                textureRenderMap.sharedMaterial.color = Color.white;
                Texture2D textureMap = ShapeGenerator.GenerateBiomeMap(mapSettings, localUp, width, height, mapPosition, 1.0f);
                textureRenderMap.sharedMaterial.mainTexture = textureMap;
            }
            

            //textureRenderMap.sharedMaterial.SetTexture("_BaseColorMap", textureMap);
            textureRenderMap.transform.localScale = new Vector3(width, 1, height);
        }

        public void UpdateMapView()
        {
            int width = mapSettings.mapResolution / (int)viewScale.x;
            int height = mapSettings.mapResolution / (int)viewScale.y;
            Vector2 currentViewPosition = new Vector2((viewPosition.x) + width/2, (viewPosition.y) + height/2);
            if (mapSettings.mapMode.HasFlag(MapModes.DetailMap))
            {
                textureRenderMapView.sharedMaterial.color = mapSettings.mapColor;
                Texture2D textureMapView = ShapeGenerator.GenerateMap(mapSettings, localUp, width, height, currentViewPosition, viewZoom);
                textureRenderMapView.sharedMaterial.mainTexture = textureMapView;
            }
            if (mapSettings.mapMode.HasFlag(MapModes.BiomeMap))
            {
                textureRenderMapView.sharedMaterial.color = Color.white;
                Texture2D textureBiomeMapView = ShapeGenerator.GenerateBiomeMap(mapSettings, localUp, width, height, currentViewPosition, viewZoom);
                textureRenderMapView.sharedMaterial.mainTexture = textureBiomeMapView;
            }

            textureRenderMapView.transform.localScale = new Vector3(width, 1, height);
            textureRenderMapView.transform.localPosition = new Vector3(currentViewPosition.x, 1, currentViewPosition.y);

        }


    }
}