using UnityEngine.Networking;
using UnityEngine;
using TMPro;
using System.Collections;
using Newtonsoft.Json;

public class InformationHandler : MonoBehaviour
{
    [SerializeField] private Camera arCamera = null;
    [SerializeField] private TextMeshProUGUI typeText = null;
    [SerializeField] private TextMeshProUGUI idText = null;

    private BuildingInformation currentBuildingInformation;
    private BusinessInformation businessInformation = new BusinessInformation();
    private string currentID;
    private string currentType;

    void Update()
    {
        DetectObjets();
    }

    private void DetectObjets()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.transform.CompareTag("Building"))
                    {
                        currentBuildingInformation = hitObject.transform.GetComponent<BuildingInformation>();
                        typeText.text = currentBuildingInformation.type;
                        idText.text = currentBuildingInformation.id.ToString();

                        currentID = currentBuildingInformation.id.ToString();
                        currentType = currentBuildingInformation.type;

                        StartCoroutine(GetInformation());
                    }

                }
            }
        }
    }

    IEnumerator GetInformation()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://green-zone-18d1c.firebaseio.com/buildings/" + currentType + "/" + currentID + ".json");
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        while (!www.downloadHandler.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.LogError(www.error);
        }

        else
        {
            string locationsString = www.downloadHandler.text;
            SetInformation(locationsString);
        }

        yield break;
    }

    private void SetInformation(string json)
    {
        businessInformation = JsonConvert.DeserializeObject<BusinessInformation>(json);
        typeText.text = businessInformation.name;
    }
}