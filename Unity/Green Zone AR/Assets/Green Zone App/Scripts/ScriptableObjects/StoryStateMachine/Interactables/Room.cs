
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelDesign/Interactables/Room")]
public class Room : ScriptableObject
{
    [TextArea]
    public string description;
    public string roomName;
    public Exit[] exits;
    public InteractableObject[] interactableObjectsInRoom;

}
