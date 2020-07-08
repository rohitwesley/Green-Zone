using UnityEngine;

namespace MovementTools
{
    public class ShapeGenerator
    {

        // [Header ("Info")]
        const int biomeTextureResolution = 50;

        /// <summary>
        /// get Axis A
        /// </summary>
        /// <param name="localUp">to set the normals of the map</param>
        public static Vector3 GetAxisA(Vector3 localUp)
        {
            //swap cordinates of local up to get axis a on the plane
            Vector3 axisA = new Vector3(localUp.y, localUp.z, localUp.x);
            return axisA;
        }

        /// <summary>
        /// get Axis B
        /// </summary>
        /// <param name="localUp">to set the normals of the map</param>
        public static Vector3 GetAxisB(Vector3 localUp)
        {
            //swap cordinates of local up to get axis a on the plane
            Vector3 axisA = new Vector3(localUp.y, localUp.z, localUp.x);
            // axisB is on the plane perpendicular to axisA and lookup using cross product 
            Vector3 axisB = Vector3.Cross(localUp, axisA);
            return axisB;

        }

        /// <summary>
        /// Create a Plane of triangles with UV's and normals
        /// </summary>
        public static void ConstructMesh(Mesh mesh, MapSettings mapSettings, Vector3 localUp)
        {
            // _mapTesselation3D = new Vector3[_mapSettings.resolution * _mapSettings.resolution];
            // total no. of triangle to form the plane (each plane is made up resolution - 1 rows and collumns which are made up of 2 triangles having 3 vertices each)
            int[] triangles = new int[(mapSettings.meshResolution - 1) * (mapSettings.meshResolution - 1) * 2 * 3];
            int triIndex = 0;

            Vector3[] map3D = Generate3DMapArray(mapSettings, mapSettings.meshResolution, localUp);
            mapSettings.elevation3DMinMax = GetElivationRange(map3D, mapSettings.meshResolution);

            //run through each point on the plane row by row
            for (var y = 0; y < mapSettings.meshResolution; y++)
            {
                for (var x = 0; x < mapSettings.meshResolution; x++)
                {
                    // Vertex Index across the plane from top left to bottom right.
                    int i = x + y * mapSettings.meshResolution;

                    if (x != mapSettings.meshResolution - 1 && y != mapSettings.meshResolution - 1)
                    {
                        // firts trinagle of the plane
                        triangles[triIndex] = i;
                        triangles[triIndex + 1] = i + mapSettings.meshResolution + 1;
                        triangles[triIndex + 2] = i + mapSettings.meshResolution;

                        // second triangle of the plane
                        triangles[triIndex + 3] = i;
                        triangles[triIndex + 4] = i + 1;
                        triangles[triIndex + 5] = i + mapSettings.meshResolution + 1;
                        triIndex += 6;
                    }

                }
            }

            Debug.Log("map3D ready");
            mesh.Clear();
            
            mesh.vertices = map3D;
            mesh.triangles = triangles;
            Vector2[] uvs = new Vector2[map3D.Length];
            for (int i = 0; i < uvs.Length; i++)
            {
                if (localUp == Vector3.up) uvs[i] = new Vector2((map3D[i].x - 1.0f) / 2, 1 - (map3D[i].z - 1.0f) / 2);
                if (localUp == Vector3.down) uvs[i] = new Vector2(1 - (map3D[i].x - 1.0f) / 2, 1 - (map3D[i].z - 1.0f) / 2);
                if (localUp == Vector3.left) uvs[i] = new Vector2((map3D[i].x - 1.0f) / 2, (1.0f - map3D[i].z) / 2);
                if (localUp == Vector3.right) uvs[i] = new Vector2((map3D[i].x - 1.0f) / 2, (1.0f - map3D[i].z) / 2);
                if (localUp == Vector3.forward) uvs[i] = new Vector2((map3D[i].x - 1.0f) / 2, (1.0f - map3D[i].z) / 2);
                if (localUp == Vector3.back) uvs[i] = new Vector2((map3D[i].x - 1.0f), (1.0f - map3D[i].z));
                // uvs[i] = new Vector2((map3D[i].x-1.0f)/2, (1.0f - map3D[i].z)/2);
            }
            mesh.uv = uvs;
            mesh.uv2 = UpdateUVsToBioms(mapSettings, localUp);
            mesh.RecalculateNormals();

        }

        /// <summary>
        /// get 3d map data
        /// </summary>
        /// <param name="mapSettings"></param>
        /// <param name="_localUp"></param>
        /// <returns></returns>
        public static Vector3[] Generate3DMapArray(MapSettings mapSettings, int resolution, Vector3 _localUp)
        {
            Vector3[] map3D = new Vector3[resolution * resolution];
            INoiseFilter[] _noiseFilter = GetNoiseFilters(mapSettings);
            //run through each point on the plane row by row
            for (var y = 0; y < resolution; y++)
            {
                for (var x = 0; x < resolution; x++)
                {
                    // Vertex Index across the plane from top left to bottom right.
                    int i = x + y * resolution;
                    // get map data
                    map3D[i] = GetPointOnHeightMap(x, y, mapSettings, resolution, _noiseFilter, _localUp);
                }
            }

            return map3D;
        }

        /// <summary>
        /// Scan through terrain face and get the max min elevation
        /// </summary>
        /// <param name="map3D"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public static MinMax GetElivationRange(Vector3[] map3D, int resolution)
        {
            MinMax _elevation3DMinMax = new MinMax();
            //run through each point on the plane row by row
            for (var y = 0; y < resolution; y++)
            {
                for (var x = 0; x < resolution; x++)
                {
                    // Vertex Index across the plane from top left to bottom right.
                    int i = x + y * resolution;
                    // set elevation range
                    if (i == 0)
                    {
                        _elevation3DMinMax.ResetValue(map3D[i].magnitude);
                        Debug.Log(" ResetElevation3DMinMax:" + _elevation3DMinMax.Min + ":" + _elevation3DMinMax.Max);
                    }
                    _elevation3DMinMax.AddValue(map3D[i].magnitude);
                }
            }
            return _elevation3DMinMax;
        }

        /// <summary>
        /// Get lookup axis for shape
        /// </summary>
        /// <param name="mapSettings"></param>
        /// <returns></returns>
        public static Vector3[] GetlookUpDirections(MapSettings mapSettings)
        {
            int TotalSides = 0;
            if (mapSettings.meshMode == MeshModes.Plane)
            {
                TotalSides = 1;
            }
            else
            {
                TotalSides = 6;
            }
            Vector3[] planeFacesDirections = new Vector3[TotalSides];

            if (mapSettings.meshMode == MeshModes.Plane)
            {
                planeFacesDirections = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up };
            }
            else
            {
                planeFacesDirections = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
            }

            return planeFacesDirections;
        }

        /// <summary>
        /// get point on 3d map to scale and shape 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="mapSettings"></param>
        /// <param name="resolution"></param>
        /// <param name="noiseFilter"></param>
        /// <param name="localUp"></param>
        /// <returns></returns>
        public static Vector3 GetPointOnHeightMap(float x, float y, MapSettings mapSettings, float resolution, INoiseFilter[] noiseFilter, Vector3 localUp)
        {
            Vector3 point3D;
            //percent of vertex per row completed
            Vector2 percent = new Vector2(x, y) / (resolution - 1);
            //move planes points 1 unit up on the localUP axis
            Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * GetAxisA(localUp) + (percent.y - .5f) * 2 * GetAxisB(localUp);
            point3D = pointOnUnitCube;
            Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
            point3D = pointOnUnitSphere;

            // scale map
            point3D *= mapSettings.planetRadius;

            
            point3D *= (1 + CalculatePointOnScaledSphere(pointOnUnitSphere, mapSettings, noiseFilter));

            return point3D;
        }

        /// <summary>
        /// get noise layer for point on 3d map
        /// </summary>
        /// <param name="pointOnUnitSphere"></param>
        /// <param name="_mapSettings"></param>
        /// <param name="_noiseFilter"></param>
        /// <returns></returns>
        public static float CalculatePointOnScaledSphere(Vector3 pointOnUnitSphere, MapSettings _mapSettings, INoiseFilter[] _noiseFilter)
        {
            float firstLayerValue = 0;
            float elevation = 0;

            
            // evaluate for first noise filter
            if (_noiseFilter.Length > 0)
            {
                firstLayerValue = _noiseFilter[0].Evaluate(pointOnUnitSphere);
                if (_mapSettings.noiseLayers[0].enabled)
                    elevation = firstLayerValue;
            }

            //evaluate for the rest of the noise filters
            for (var i = 1; i < _noiseFilter.Length; i++)
            {
                if (_mapSettings.noiseLayers[i].enabled)
                {
                    // check to use first layer as mask or not
                    float mask = (_mapSettings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
                    elevation += _noiseFilter[i].Evaluate(pointOnUnitSphere) * mask;
                }
            }

            //elevation = planetRadius * (1 + elevation);

            return elevation;
        }

        /// <summary>
        /// get noise filters
        /// </summary>
        /// <param name="_mapSettings"></param>
        /// <returns></returns>
        public static INoiseFilter[] GetNoiseFilters(MapSettings _mapSettings)
        {
            // calculate noise layers
            INoiseFilter[] _noiseFilter = new INoiseFilter[_mapSettings.noiseLayers.Length];
            for (var i = 0; i < _noiseFilter.Length; i++)
            {
                if (_noiseFilter[i] == null)
                    _noiseFilter[i] = NoiseFilterGenerator.CreateNoiseFilter(_mapSettings.noiseLayers[i].noiseSettings);

            }
            return _noiseFilter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapSettings"></param>
        /// <param name="localUp"></param>
        /// <returns></returns>
        public static Vector2[] UpdateUVsToBioms(MapSettings mapSettings, Vector3 localUp)
        {
            Vector2[] uvsBiomes = new Vector2[mapSettings.meshResolution * mapSettings.meshResolution];
            for (int y = 0; y < mapSettings.meshResolution; y++)
            {
                for (int x = 0; x < mapSettings.meshResolution; x++)
                {
                    int i = x + y * mapSettings.meshResolution;
                    Vector2 percent = new Vector2(x, y) / (mapSettings.meshResolution - 1);
                    Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * GetAxisA(localUp) + (percent.y - .5f) * 2 * GetAxisB(localUp);
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                    uvsBiomes[i] = new Vector2(BiomePercentFromPoint(pointOnUnitSphere, mapSettings), 0);
                }
            }

            return(uvsBiomes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="mapSettings"></param>
        public static void UpdateShaderGraphMaterial(Renderer renderer, MapSettings mapSettings, Vector3 localUp)
        {

            /*Texture2D textureTerrainElevation = new Texture2D(biomeTextureResolution, 1);
            textureTerrainElevation.filterMode = FilterMode.Point;
            textureTerrainElevation.wrapMode = TextureWrapMode.Clamp;
            Color[] elevationColorMap = new Color[biomeTextureResolution];

            Texture2D textureTerrainBioms = new Texture2D(biomeTextureResolution, mapSettings.biomeSettings.biomes.Length);
            Texture2D textureTerrainBiomIndex = new Texture2D(biomeTextureResolution, mapSettings.biomeSettings.biomes.Length);
            textureTerrainBioms.filterMode = FilterMode.Point;
            textureTerrainBioms.wrapMode = TextureWrapMode.Clamp;
            Color[] biomColorMap = new Color[textureTerrainBioms.width * textureTerrainBioms.height];
            Color[] biomMatMap = new Color[textureTerrainBioms.width * textureTerrainBioms.height];

            int biomColourIndex = 0;
            foreach (var biome in mapSettings.biomeSettings.biomes)
            {
                for (int i = 0; i < biomeTextureResolution; i++)
                {
                    Color gradientCol = biome.mapBioms.Evaluate(i / (biomeTextureResolution - 1f));
                    Color tintCol = biome.tint;
                    Color indexColor = Color.Lerp(Color.black, Color.white, biomColourIndex / mapSettings.biomeSettings.biomes.Length);
                    biomColorMap[biomColourIndex] = gradientCol * (1 - biome.tintPercent) + tintCol * biome.tintPercent;
                    biomMatMap[biomColourIndex] = indexColor * (1 - biome.tintPercent) + indexColor * biome.tintPercent;
                    biomColourIndex++;
                }
            }

            textureTerrainBioms.SetPixels(biomColorMap);
            textureTerrainBioms.Apply();
            textureTerrainBiomIndex.SetPixels(biomMatMap);
            textureTerrainBiomIndex.Apply();

            for (int i = 0; i < biomeTextureResolution; i++)
            {
                elevationColorMap[i] = mapSettings.biomeRegions.Evaluate(i / (biomeTextureResolution - 1f));
            }

            textureTerrainElevation.SetPixels(elevationColorMap);
            textureTerrainElevation.Apply();*/

            Vector2 mapPosition = new Vector2();
            mapPosition.Set(0.0f, 0.0f);

            renderer.sharedMaterial.mainTexture = GenerateMap(mapSettings, localUp, mapSettings.mapResolution, mapSettings.mapResolution, mapPosition, 1.0f);
            //renderer.sharedMaterial.mainTexture = textureTerrainElevation;
            renderer.sharedMaterial.color = mapSettings.mapColor;

            //renderer.sharedMaterial.SetColor("_BaseColor", mapSettings.mapColor);
            //renderer.sharedMaterial.SetVector("_elevationMinMax", new Vector4(mapSettings.elevation3DMinMax.Min, mapSettings.elevation3DMinMax.Max, 0, 0));
            //renderer.sharedMaterial.SetTexture("_textureTerrainElevation", textureTerrainElevation);
            //renderer.sharedMaterial.SetTexture("_textureTerrainBioms", textureTerrainBioms);
            // _renderer.transform.localScale = new Vector3(_mapSettings.resolution, 1, _mapSettings.resolution);
            // _renderer.transform.localScale = new Vector3(1, 1, 1);
            Debug.Log("mapTexture ready");

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointOnUnitSphere"></param>
        /// <param name="mapSettings"></param>
        /// <returns></returns>
        public static float BiomePercentFromPoint(Vector3 pointOnUnitSphere, MapSettings mapSettings)
        {
            INoiseFilter biomnoiseFilter = NoiseFilterGenerator.CreateNoiseFilter(mapSettings.biomeSettings.noiseSettings);

            float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
            heightPercent += (biomnoiseFilter.Evaluate(pointOnUnitSphere) - mapSettings.biomeSettings.noiseOffset) * mapSettings.biomeSettings.noiseStrength;
            float biomeIndex = 0;
            int numBiomes = mapSettings.biomeSettings.biomes.Length;
            //add 0.001f to prevent blendamount from being zero
            float blendRange = mapSettings.biomeSettings.blendAmount / 2f + 0.001f;
            for (int i = 0; i < numBiomes; i++)
            {
                float dist = heightPercent - mapSettings.biomeSettings.biomes[i].startLatitude;
                float weight = Mathf.InverseLerp(-blendRange, blendRange, dist);
                //make biome index staye in limits
                biomeIndex *= 1 - weight;
                biomeIndex += i * weight;
                // if(_mapSettings.biomeSettings.biomes[i].startLatitude < heightPercent)
                // {
                //     biomeIndex = i;
                // }
                // else
                // {
                //     break;
                // }
            }

            return biomeIndex / Mathf.Max(1, numBiomes - 1);
        }

        /// <summary>
        /// Draw map on a texture
        /// </summary>
        /// <param name="mapSettings"></param>
        /// <param name="localUp"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="viewPosition"></param>
        /// <param name="viewZoom"></param>
        /// <returns></returns>
        public static Texture2D GenerateMap(MapSettings mapSettings, Vector3 localUp, int width, int height, Vector2 viewPosition, float viewZoom)
        {
            Texture2D textureMap = new Texture2D(width, height);
            Color[] colorMap = new Color[width * height];

            INoiseFilter[] noiseFilter = ShapeGenerator.GetNoiseFilters(mapSettings);

            float scale = 5;
            Debug.Log("width:" + width);
            Debug.Log("height:" + height);

            //run through each point on the plane row by row
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {

                    float sampleX = viewPosition.x + x;
                    float sampleY = viewPosition.y + y;
                    Vector3 pointHeightOnMap = ShapeGenerator.GetPointOnHeightMap(sampleX, sampleY, mapSettings, mapSettings.mapResolution * viewZoom, noiseFilter, localUp);
                    float normalisedPointHeightonMap = (pointHeightOnMap.magnitude - mapSettings.elevation3DMinMax.Min) / (mapSettings.elevation3DMinMax.Max - mapSettings.elevation3DMinMax.Min);
                    colorMap[(int)y * width + (int)x] = Color.Lerp(Color.black, Color.white, normalisedPointHeightonMap);


                }
            }
            textureMap.SetPixels(colorMap);
            textureMap.Apply();
            return textureMap;
        }


        /// <summary>
        /// Draw bioms on a texture
        /// </summary>
        /// <param name="mapSettings"></param>
        /// <param name="localUp"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="viewPosition"></param>
        /// <param name="viewZoom"></param>
        /// <returns></returns>
        public static Texture2D GenerateBiomeMap(MapSettings mapSettings, Vector3 localUp, int width, int height, Vector2 viewPosition, float viewZoom)
        {
            Texture2D textureMap = new Texture2D(width, height);
            Color[] colorMap = new Color[width * height];

            INoiseFilter[] noiseFilter = ShapeGenerator.GetNoiseFilters(mapSettings);

            float scale = 5;
            Debug.Log("width:" + width);
            Debug.Log("height:" + height);

            //run through each point on the plane row by row
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {

                    float sampleX = viewPosition.x + x;
                    float sampleY = viewPosition.y + y;
                    Vector3 pointHeightOnMap = ShapeGenerator.GetPointOnHeightMap(sampleX, sampleY, mapSettings, mapSettings.mapResolution * viewZoom, noiseFilter, localUp);
                    
                    // map elevation
                    float normalisedPointHeightonMap = (pointHeightOnMap.magnitude - mapSettings.elevation3DMinMax.Min) / (mapSettings.elevation3DMinMax.Max - mapSettings.elevation3DMinMax.Min);
                    colorMap[(int)y * width + (int)x] = Color.Lerp(Color.black, Color.white, normalisedPointHeightonMap);

                    foreach (var biome in mapSettings.biomeSettings.biomes)
                    { 
                        if (normalisedPointHeightonMap >= biome.startElevation && normalisedPointHeightonMap <= biome.endElevation)
                        {
                            BiomeModes mode = mapSettings.biomeSettings.biomeModes;
                            // get index on biome Map
                            float pointOnBiomeMap = (normalisedPointHeightonMap - biome.startElevation) / (biome.endElevation - biome.startElevation);
                            // biome Type
                            if (mode == BiomeModes.BiomeTypes)
                            {
                                colorMap[(int)y * width + (int)x] = biome.tint;
                            }
                            // biome elevation
                            if (mode == BiomeModes.BiomeElevation)
                            {
                                float elevationPointOnBiomeMap = normalisedPointHeightonMap * biome.biomeHeightCurve.Evaluate(pointOnBiomeMap);
                                colorMap[(int)y * width + (int)x] = Color.Lerp(Color.black, Color.white, elevationPointOnBiomeMap);
                            }
                            // biome color
                            if(mode == BiomeModes.BiomeColor)
                            {
                                colorMap[(int)y * width + (int)x] = biome.mapBiomes.Evaluate(pointOnBiomeMap);
                            }

                        }
                    }
                    
                }
            }
            textureMap.SetPixels(colorMap);
            textureMap.Apply();
            return textureMap;
        }

        /// <summary>
        /// Not Used
        /// </summary>
        public static Vector3[] CalculateNormals(int[] triangles, Vector3[] vertices)
        {
            Vector3[] vertexNormals = new Vector3[vertices.Length];
            int triangleCount = triangles.Length / 3;
            for (int i = 0; i < triangleCount; i++)
            {
                int normalTriangleIndex = i * 3;
                int vertexIndexA = triangles[normalTriangleIndex];
                int vertexIndexB = triangles[normalTriangleIndex + 1];
                int vertexIndexC = triangles[normalTriangleIndex + 2];

                Vector3 trinagleNormal = SurfaceNormalFromIndices(vertices, vertexIndexA, vertexIndexB, vertexIndexC);
                vertexNormals[vertexIndexA] += trinagleNormal;
                vertexNormals[vertexIndexB] += trinagleNormal;
                vertexNormals[vertexIndexC] += trinagleNormal;

            }
            for (int i = 0; i < vertexNormals.Length; i++)
            {
                vertexNormals[i].Normalize();
            }

            return vertexNormals;

        }

        public static Vector3 SurfaceNormalFromIndices(Vector3[] vertices, int indexA, int indexB, int indexC)
        {
            Vector3 pointA = vertices[indexA];
            Vector3 pointB = vertices[indexB];
            Vector3 pointC = vertices[indexC];

            Vector3 sideAB = pointB - pointA;
            Vector3 sideAC = pointC - pointA;
            return Vector3.Cross(sideAB, sideAC).normalized;
        }

        public static Texture2D getBiomeTexture(int rez, MapSettings mapSettings)
        {

            int biomColourIndex = 0;
            Color[] biomColorMap = new Color[biomeTextureResolution * mapSettings.biomeSettings.biomes.Length];
            foreach (var biome in mapSettings.biomeSettings.biomes)
            {
                for (int i = 0; i < biomeTextureResolution; i++)
                {
                    Color gradientCol = biome.mapBiomes.Evaluate(i / (biomeTextureResolution - 1f));
                    Color tintCol = biome.tint;
                    biomColorMap[biomColourIndex] = gradientCol * (1 - biome.tintPercent) + tintCol * biome.tintPercent;
                    biomColourIndex++;
                }
            }
            Texture2D biomTexture = new Texture2D(biomeTextureResolution, mapSettings.biomeSettings.biomes.Length);
            biomTexture.SetPixels(biomColorMap);
            biomTexture.Apply();
            return biomTexture;
            // return textureTerrainBioms.GetPixel((int)(height*biomeTextureResolution),(int)(uvBiomes.x * _mapSettings.biomeSettings.biomes.Length));
        }

        public static Vector2Int getBiomeIndexOnPlanet(float height, int IndexX, int IndexY, int rez, MapSettings mapSettings, Vector3 localUp)
        {
            Vector2 percent = new Vector2(IndexX, IndexY) / (rez);
            Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * GetAxisA(localUp) + (percent.y - .5f) * 2 * GetAxisB(localUp);
            Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
            Vector2Int uvBiomes;
            //convert UV float values from 0 - 1 into UV array index 0 - 100
            uvBiomes = new Vector2Int((int)(height * biomeTextureResolution), (int)(BiomePercentFromPoint(pointOnUnitSphere, mapSettings) * (mapSettings.biomeSettings.biomes.Length - 1)));
            return uvBiomes;
        }


    }


}
