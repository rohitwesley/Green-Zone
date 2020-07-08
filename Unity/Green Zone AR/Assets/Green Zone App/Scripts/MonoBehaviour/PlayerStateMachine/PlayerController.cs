using System.Collections;
using UnityEngine;

public class PlayerController : GlobalObjectManager
{
    [Tooltip("Total Speed of the Player")]
    [SerializeField] private float _PlayerSpeed;
    [Tooltip("Player Rigid Body")]
    [SerializeField] private Rigidbody _PlayerRigidBody;
    
    [SerializeField] private KeyCode _SprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _JumpKey = KeyCode.Space;

    void Start ()
    {
        // rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate ()
    {
        // get user input
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        // move player
        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        _PlayerRigidBody.AddForce (movement * _PlayerSpeed);

    }

    void OnTriggerEnter(Collider other) 
    {
        //Get Pick Ups
        if (other.gameObject.CompareTag ("Pick Ups"))
        {
            other.gameObject.SetActive (false);
            Destroy(other.gameObject);
            _PickUpsCount++;
        }
    }


}
