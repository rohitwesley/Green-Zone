using GameLogic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        // If Agent Collided
        if (other.gameObject.GetComponent<Agents>())
        {
            Debug.Log("PickUp Triggered by " + other.gameObject.GetComponent<Agents>().GetAgentName());
            //Get Pick Up if player and add points to player score
            if (other.gameObject.GetComponent<Agents>().unitType == UnitType.Player)
            {
                Debug.Log("Player PickUp");
                gameObject.SetActive(false);
                Destroy(gameObject);
                FindObjectsOfType<GameManager>()[0].UpdateCheckpoints(other.gameObject.GetComponent<Agents>());
            }
        }
    }

}
