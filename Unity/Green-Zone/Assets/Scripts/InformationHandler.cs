using UnityEngine.Networking;
using UnityEngine;
using TMPro;
using System.Collections;
using Newtonsoft.Json;

public class InformationHandler : MonoBehaviour
{
    // Public:
        // Text and images.
    public GameObject returnButton;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI hoursText;
    public TextMeshProUGUI phoneText;
    public TextMeshProUGUI statusText;
    public GameObject openImage;
    public GameObject closedImage;
    public GameObject takeOutImage;

    // Serialized but private:
    [SerializeField] private Camera arCamera = null;
    [SerializeField] private GameObject frame;

    // Private:
    private BuildingInformation currentBuildingInformation;
    private BusinessInformation businessInformation = new BusinessInformation();
    private string currentID;
    private string currentType;

    private Transform buildingPosition = null;


    /// <summary>
    /// Every frame, the game checks if the user is touching the screen.
    /// </summary>
    private void Update()
    {
        DetectObjets();
    }

    /// <summary>
    /// 1. Detects touch on screen
    /// 2. If User clicks on building go to database
    /// </summary>
    private void DetectObjets()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hitObject;
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.transform.CompareTag("Building"))
                    {
                        buildingPosition = hitObject.transform;
                        currentBuildingInformation = hitObject.transform.GetComponent<BuildingInformation>();

                        currentID = currentBuildingInformation.id.ToString();
                        currentType = currentBuildingInformation.type;

                        StartCoroutine(GetInformation());
                    }

                }
            }
        }
    }

    /// <summary>
    /// Depending on the type of building and ID, get request in the database
    /// in order to get the information for the specific business.
    /// Once we have the information it is parsed into a local class to
    /// display it on UI.
    /// </summary>
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

    // Display Information:
    private void SetInformation(string json)
    {
        businessInformation = JsonConvert.DeserializeObject<BusinessInformation>(json);

        nameText.text = businessInformation.name;
        typeText.text = businessInformation.type;
        hoursText.text = "Regular hours: " + businessInformation.openHour + " - " + businessInformation.closeHour;
        phoneText.text = "Phone: " + businessInformation.phoneNumber;

        switch (businessInformation.status)
        {
            case "open":
                statusText.text = "COVID Status: Open";
                openImage.SetActive(true);
                break;
            case "closed":
                statusText.text = "COVID Status: Closed";
                closedImage.SetActive(true);
                break;
            default:
                statusText.text = "COVID Status: Take Out";
                takeOutImage.SetActive(true);
                break;
        }

        frame.SetActive(true);
        returnButton.SetActive(true);
        frame.transform.position = new Vector3(buildingPosition.position.x, buildingPosition.position.y + 3.0f, buildingPosition.position.z);
        returnButton.SetActive(true);
    }
}