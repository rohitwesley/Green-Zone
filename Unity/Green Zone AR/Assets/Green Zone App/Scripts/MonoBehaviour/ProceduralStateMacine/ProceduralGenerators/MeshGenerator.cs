using UnityEngine;

namespace MovementTools
{
    public class MeshGenerator : MonoBehaviour
    {

        [HideInInspector]
        public bool foldout;

        public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back }

        public FaceRenderMask faceRenderMask = FaceRenderMask.All;

        public string LayerMaskName = "Maps";

        public MapSettings mapSettings;
        [HideInInspector]
        [SerializeField] GameObject[] meshObjects;
        

        void Start()
        {
            GenerateObject();
        }

        private void Update()
        {
            OnObjectSettingsUpdated();
        }

        // draw/update object
        public void OnObjectSettingsUpdated()
        {
            if (mapSettings.autoUpdateMesh)
            {
                GenerateMesh();
            }
            if (mapSettings.autoUpdateMaterial)
            {
                GenerateMaterials();
            }

        }

        // draw/update mesh and material
        public void GenerateObject()
        {
            Initialize();
            GenerateMesh();
            GenerateMaterials();
        }
        
        
        private void Initialize()
        {
            mapSettings.planeFacesDirections = ShapeGenerator.GetlookUpDirections(mapSettings);

            int TotalSides = mapSettings.planeFacesDirections.Length;
            if (faceRenderMask != FaceRenderMask.All)
                TotalSides = 1;

            if (meshObjects != null)
            {
                // destroy object in editor and at run time
                foreach (GameObject obj in meshObjects)
                    DestroyImmediate(obj);

                Debug.Log("destroyed objects");
            }
            meshObjects = new GameObject[TotalSides];

            for (var i = 0; i < TotalSides; i++)
            {
                // if mesh not already created create it else just update it
                // if(_meshFilters[i] == null){
                //create new component called mesh
                GameObject meshObj = new GameObject("meshFace");
                //parent it to this gameobject
                meshObj.gameObject.transform.parent = transform;
                meshObj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                meshObj.transform.localRotation = Quaternion.identity;
                // assign default material to the gameobject
                MeshFilter meshFilters = meshObj.AddComponent<MeshFilter>();
                // meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("HDRP/LitTessellation"));
                meshObj.AddComponent<MeshRenderer>();
                meshFilters.sharedMesh = new Mesh();
                meshFilters.GetComponent<MeshRenderer>().sharedMaterial = Instantiate(mapSettings.mapPlanetMaterial);
                //meshFilters.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_BaseColor", mapSettings.mapColor);
                //Texture2D _meshTexture = new Texture2D(mapSettings.meshResolution, mapSettings.meshResolution);
                //meshFilters.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_BaseColorMap", _meshTexture);
                meshObjects[i] = meshObj;
                MeshCollider meshCollider = meshObjects[i].AddComponent<MeshCollider>();
                // _meshFilters.sharedMesh = meshCollider;
                // assign map settings material material to the gameobject
                // }

                meshObjects[i].layer = LayerMask.NameToLayer(LayerMaskName);

            }
            ResetMesh();
            Debug.Log("Initialized");
        }

        void UpdateRenderMask()
        {
            int TotalSides = 6;
            for (var i = 0; i < TotalSides; i++)
            {
                bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
                meshObjects[i].SetActive(renderFace);
            }
        }

        void ResetMesh()
        {
            foreach (GameObject faceMesh in meshObjects)
            {
                faceMesh.transform.localPosition = Vector3.zero;
                faceMesh.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            UpdateRenderMask();

        }

        void GenerateMesh()
        {
            int TotalSides = mapSettings.planeFacesDirections.Length;
            if (faceRenderMask != FaceRenderMask.All)
                TotalSides = 1;
            for (int i = 0; i < TotalSides; i++)
            {
                if (meshObjects[i].activeSelf)
                {
                    ShapeGenerator.ConstructMesh(meshObjects[i].GetComponent<MeshFilter>().sharedMesh, mapSettings, mapSettings.planeFacesDirections[i]);
                }
            }
            mapSettings.isGeneraterMesh = true;
            Debug.Log("Generated Mesh");

        }

        void GenerateMaterials()
        {
            int TotalSides = mapSettings.planeFacesDirections.Length;
            if (faceRenderMask != FaceRenderMask.All)
                TotalSides = 1;
            for (int i = 0; i < TotalSides; i++)
            {
                meshObjects[i].GetComponent<MeshFilter>().sharedMesh.uv2 = ShapeGenerator.UpdateUVsToBioms(mapSettings, mapSettings.planeFacesDirections[i]);
                ShapeGenerator.UpdateShaderGraphMaterial(meshObjects[i].GetComponent<MeshRenderer>(), mapSettings, mapSettings.planeFacesDirections[i]);
            }
            Debug.Log("Generate Material");
        }

    }

}

