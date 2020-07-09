
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
        Person,
        Building,
        LiquorStore,
        Restaurant,
        Shop,
        CoffeeShop,
    }
    
    /// <summary>
    /// All agents have there properties in this class
    /// Should be added to the part of the Agent that has the collider 
    /// </summary>
    public class Agents : MonoBehaviour
    {

        public UnitType unitType = UnitType.Building;
        public GameObject unitIcon;

        [Tooltip("Points")]
        [SerializeField]
        public int pointsValue = 10;                 // The points value of the object

        /// <summary>
        /// Hide Icons on start
        /// </summary>
        private void Start()
        {
            //unitIcon.SetActive(false);

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
                case "Building":
                    return UnitType.Building;
                case "Person":
                    return UnitType.Person;
                case "LiquorStore": 
                    return UnitType.LiquorStore;
                case "Restaurant": 
                    return UnitType.Restaurant;
                case "Shop":
                    return UnitType.Shop;
                case "CoffeeShop":
                    return UnitType.CoffeeShop;
                default:
                    return UnitType.Floor;
            }
        }

        public string GetAgentName()
        {
            switch (unitType)
            {
                case UnitType.Building:
                    return "Building";
                case UnitType.Person:
                    return "Person";
                case UnitType.LiquorStore:
                    return "LiquorStore";
                case UnitType.Restaurant: 
                    return "Restaurant";
                case UnitType.Shop:
                    return "Shop";
                case UnitType.CoffeeShop:
                    return "CoffeeShop";
                default:
                    return "Floor";
            }
        }
    
    }

}
