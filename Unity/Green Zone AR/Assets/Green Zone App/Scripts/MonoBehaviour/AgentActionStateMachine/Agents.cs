
using System.Collections;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// Agent Type ID to recognise type of Agent
    /// </summary>
    public enum UnitType
    {
        Floor,
        Wall,
        Path,
        Flock,
        Player,
        Pawns,
        PickUp,
        SpawnPoint,
        CheckPoint,
        Start,
        Exit
    }
    
    /// <summary>
    /// All agents have there properties in this class
    /// Should be added to the part of the Agent that has the collider 
    /// </summary>
    public class Agents : MonoBehaviour
    {

        public UnitType unitType = UnitType.Floor;
        public GameObject unitIcon;

        [Tooltip("Points")]
        [SerializeField]
        public int pointsValue = 10;                 // The points value of the object

        /// <summary>
        /// Hide Icons on start
        /// </summary>
        private void Start()
        {
            unitIcon.SetActive(false);

            Debug.Log("Agent : " + GetAgentName() + "Initialised ");
        }

        /// <summary>
        /// used to activat Icon coroutine
        /// </summary>
        /// <param name="sec"></param>
        public void ShowIcon(float sec)
        {
            StartCoroutine(ShowIconAnimation(sec));
        }

        /// <summary>
        /// Show Icon for given time using Coroutine
        /// </summary>
        /// <param name="sec">seconds to show icons</param>
        /// <returns>pause</returns>
        IEnumerator ShowIconAnimation(float sec)
        {
    
            yield return new WaitForSeconds(0.1f);
            unitIcon.SetActive(true);
            yield return new WaitForSeconds(sec);
            unitIcon.SetActive(false);
        }
        

        public static UnitType GetAgentFromString(string agentType)
        {
            switch (agentType)
            {
                case "Floor": 
                    return UnitType.Floor;
                case "Wall": 
                    return UnitType.Wall;
                case "Path": 
                    return UnitType.Path;
                case "Flock":
                    return UnitType.Flock;
                case "Player": 
                    return UnitType.Player;
                case "Pawns":
                    return UnitType.Pawns;
                case "PickUp":
                    return UnitType.PickUp;
                case "SpawnPoint":
                    return UnitType.SpawnPoint;
                case "CheckPoint":
                    return UnitType.CheckPoint;
                case "Start":
                    return UnitType.Start;
                case "Exit":
                    return UnitType.Exit;
                default:
                    return UnitType.Floor;
            }
        }

        public string GetAgentName()
        {
            switch (unitType)
            {
                case UnitType.Floor: 
                    return "Floor";
                case UnitType.Wall: 
                    return "Wall";
                case UnitType.Path: 
                    return "Path";
                case UnitType.Flock:
                    return "Flock";
                case UnitType.Player: 
                    return "Player";
                case UnitType.Pawns:
                    return "Pawns";
                case UnitType.SpawnPoint:
                    return "SpawnPoint";
                case UnitType.PickUp:
                    return "PickUp";
                case UnitType.CheckPoint:
                    return "CheckPoint";
                case UnitType.Start:
                    return "Start";
                case UnitType.Exit:
                    return "Exit";
                default:
                    return "Floor";
            }
        }
    
    }

}
