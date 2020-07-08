using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelDesign/InputActions/Take")]
public class Take : InputAction 
{
    public override void RespondToInput (LevelController controller, string[] separatedInputWords)
    {
        Dictionary<string, string> takeDictionary = controller.interactableItems.Take (separatedInputWords);

        if (takeDictionary != null) 
        {
            controller.LogStringWithReturn (controller.TestVerbDictionaryWithNoun (takeDictionary, separatedInputWords [0], separatedInputWords [1]));
        }
    }
}