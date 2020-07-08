using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelDesign/InputActions/Inventory")]
public class Inventory : InputAction 
{
    public override void RespondToInput (LevelController controller, string[] separatedInputWords)
    {
        controller.interactableItems.DisplayInventory();
    }
    
}