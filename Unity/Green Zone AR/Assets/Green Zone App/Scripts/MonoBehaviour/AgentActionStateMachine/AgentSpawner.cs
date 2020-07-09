using System.Collections.Generic;
using GameLogic;
using UnityEngine;
using TMPro;
using System.Collections;

public class AgentSpawner : MonoBehaviour
{

    /// <summary>
    /// Spawn Inputs
    /// </summary>
    [SerializeField] private KeyCode spawnReset = KeyCode.Space;

    /// <summary>
    /// Object CRUD (Create,Read,Update,Delete)
    /// </summary>
    [SerializeField] private List<Agents> unitTypes;
    [SerializeField] private int unitsCapacityTotal = 10;
    [SerializeField] private float unitsRateInSec = 1.0f;
    [SerializeField] private bool isSpawnUnits = true;
    [SerializeField] private TextMeshProUGUI textInfo;

    private void Start()
    {
        // check to start spawning coroutine on start
        if(isSpawnUnits)
        {
            StartCoroutine(SpawnAgent());
        }
    }

    void Update()
    {
        // start spawning coroutine if reset spawning key pressed
        if(!isSpawnUnits)
        {
            if(Input.GetKeyDown(spawnReset))
                StartCoroutine(SpawnAgent());
        }
    }

    /// <summary>
    /// Spawn Agent Creation Menu
    /// </summary>
    IEnumerator SpawnAgent()
    {
        isSpawnUnits = true;
        for (int unitsCapacity = 0; unitsCapacity < unitsCapacityTotal; unitsCapacity++)
        {
            int Id = Random.Range(0, unitTypes.Count);
            Instantiate(unitTypes[Id].gameObject, transform.position, transform.rotation);
            yield return new WaitForSeconds(unitsRateInSec);
        }
        isSpawnUnits = false;
        
    }


}
