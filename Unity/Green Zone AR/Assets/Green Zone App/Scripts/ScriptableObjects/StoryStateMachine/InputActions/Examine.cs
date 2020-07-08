using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelDesign/InputActions/Examine")]
public class Examine : InputAction 
{
    public override void RespondToInput (LevelController controller, string[] separatedInputWords)
    {
        controller.LogStringWithReturn (controller.TestVerbDictionaryWithNoun (controller.interactableItems.examineDictionary, separatedInputWords [0], separatedInputWords [1]));
    }

}