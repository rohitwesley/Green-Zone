using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class PickUpController : GlobalObjectManager
{
    [Tooltip("Pick Up UI Text")]
    [SerializeField] Text _PickUpText;

    void Update()
    {
        //Update PickUP UI
        string pickupTextNew = _PickUpsCount+"/"+_PickUpsTotal;
        if(_PickUpText.text != pickupTextNew){
            _PickUpText.text = pickupTextNew;
        }
        
    }

}
