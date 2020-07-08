using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "LevelDesign/InputActions/Use")]
public class Use : InputAction 
{
    public override void RespondToInput (LevelController controller, string[] separatedInputWords)
    {
        controller.interactableItems.UseItem (separatedInputWords);
    }
}