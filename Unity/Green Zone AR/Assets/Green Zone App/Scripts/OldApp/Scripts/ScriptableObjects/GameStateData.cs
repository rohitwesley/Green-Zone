using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "MUDTools/PlayerData", order = 1)]
public class GameStateData : ScriptableObject
{
    public int level = 0;

    public int checkpointInLevel = 0;

    public int pickUpsCollected = 0;

    public int health = 0;

    public float timeInLevel = 0;

    [Tooltip("Score")]
    [SerializeField]
    public int scoreValue = 10;                 // The score value of the object
}
