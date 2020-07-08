using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelDesign/ActionResponses/ChangeRoom")]
public class ChangeRoomResponse : ActionResponse 
{
    public Room roomToChangeTo;

    public override bool DoActionResponse (LevelController controller)
    {
        if (controller.roomNavigation.currentRoom.roomName == requiredString) 
        {
            controller.roomNavigation.currentRoom = roomToChangeTo;
            controller.DisplayRoomText ();
            return true;
        }

        return false;
    }
    
}