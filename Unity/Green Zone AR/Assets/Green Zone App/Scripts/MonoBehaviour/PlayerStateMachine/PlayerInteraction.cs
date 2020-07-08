using System.Collections.Generic;
using UnityEngine;
using GameLogic;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    /// <summary>
    /// Object CRUD (Create,Read,Update,Delete)
    /// TODO Select/Read,Edit/Update,Delete object.
    /// </summary>
    [SerializeField] private List<Agents> units;
    [SerializeField] private GameObject creationMenu;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    Vector3 newAgentPosition; 
    Quaternion newAgentRotation;

    void Update()
    {
        // if Left Click create and Object
        if (Input.GetMouseButtonDown(0))   
        {
            textMeshPro.text = "Lets Build";
            CallMenuAtRay();    
        }
        else
        {
            // View Selected Object
            ViewObjectAtRay();
        }
    }

    /// <summary>
    /// Launch Creatin menu if ray hit the floor and update position and rotations
    /// </summary>
    private void CallMenuAtRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            // Debug.DrawRay(Camera.main.transform.position, hit.point.normalized, Color.red, 3.0f);
            if(hit.collider.gameObject.GetComponent<Agents>())
            {
                //Draw an object if ray hit the floor.
                // Debug.Log("Hit a " + hit.collider.gameObject.GetComponent<Agents>());
                int unityType = UnityEngine.Random.Range(0, units.Count) ;
                if(hit.collider.gameObject.GetComponent<Agents>().unitType == UnitType.Floor)
                {
                    UpdateNewAgentPlacment(hit.point,Quaternion.identity);
                    ShowCreationMenu();
                }
            }
        }

    }

    private void UpdateNewAgentPlacment(Vector3 agentPos, Quaternion agentRot)
    {
        newAgentPosition = agentPos;
        newAgentRotation = agentRot;
    }

    /// <summary>
    /// Show Agent Stats
    /// </summary>
    private void ViewObjectAtRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            if(hit.collider.gameObject.GetComponent<Agents>())
            {
                //Show Agent Stats
                // Debug.Log("Hit a " + hit.collider.gameObject.GetComponent<Agents>());
                int unityType = UnityEngine.Random.Range(0, units.Count) ;
                if(hit.collider.gameObject.GetComponent<Agents>().unitType != UnitType.Floor)
                    hit.collider.gameObject.GetComponent<Agents>().ShowIcon(2.0f);
            }
        }

    }

    /// <summary>
    /// Show Agent Creation Menu
    /// </summary>
    private void ShowCreationMenu()
    {
        creationMenu.SetActive(true);
    }
    
    /// <summary>
    /// Hide Agent Creation Menu
    /// </summary>
    public void SpawnAgent(string agentType)
    {
        // Debug.Log("Spawning Agent");
        foreach (Agents agent in units)
        {
            if(agent.unitType == Agents.GetAgentFromString(agentType))
            {
                Instantiate(agent.gameObject, newAgentPosition, newAgentRotation);
            }
        }
        creationMenu.SetActive(false);
    }


}
