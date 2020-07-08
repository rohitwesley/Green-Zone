using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour {

    public bool portrait = true;

    public CanvasScaler canvas;

    public Camera MainCamera;
    public Camera SceneCamera;
    RenderTexture SceneTargetTexture;
    bool sceneView;

    // Use this for initialization
    void Start () {

        if(portrait){

            // Switch to 1080 x 1920 windowed at 60 hz
            Screen.SetResolution(1080, 1920, false, 60);

            // Set Scene Orentation to Portrate
            Screen.orientation = ScreenOrientation.Portrait;

            // Set Canvas Orentation to Portrate
            canvas.matchWidthOrHeight = 0;

        }
        else {


            // Switch to 1280 x 720 windowed at 60 hz
            Screen.SetResolution(1280, 720, false, 60);

            // Set Scene Orentation to Landscape
            Screen.orientation = ScreenOrientation.Landscape;

            // Set Canvas Orentation to Landscape
            canvas.matchWidthOrHeight = 1;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeCamera(Camera CurrentCamera)
    {
        if(CurrentCamera == SceneCamera){
            SceneCamera.targetTexture = SceneTargetTexture;
            MainCamera.enabled = true;//!MainCamera.enabled;
            SceneCamera = null;

        }
        else {
            SceneCamera = CurrentCamera;
            SceneTargetTexture = SceneCamera.targetTexture;
            SceneCamera.targetTexture = null;
            MainCamera.enabled = false;//!MainCamera.enabled;
        }

    }



    public void ChangeUISetting(Canvas SceneCanvas)
    {
        //If Scene Camera is on render to screen on texture
        if (SceneCamera == null)
        {
            SceneCanvas.enabled = false;
        }
        else
        {
            SceneCanvas.enabled = true;

        }



    }


}
