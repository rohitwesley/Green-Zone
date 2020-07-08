using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "LevelDesign/InputActions/Go")]
public class Go : InputAction
{
    public override void RespondToInput (LevelController controller, string[] separatedInputWords)
    {
        controller.roomNavigation.AttemptToChangeRooms (separatedInputWords [1]);
    }
}
