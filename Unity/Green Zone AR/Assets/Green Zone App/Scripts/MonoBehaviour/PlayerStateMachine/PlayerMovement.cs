using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// WASD Inputs
    /// </summary>
    [SerializeField] private KeyCode moveForward = KeyCode.W;
    [SerializeField] private KeyCode moveLeft = KeyCode.A;
    [SerializeField] private KeyCode moveBackward = KeyCode.S;
    [SerializeField] private KeyCode moveRight = KeyCode.D;
    [SerializeField] private Transform body;
    [Range(0,10)]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float cellStep = 50.0f;

    bool isMoving = false;

    void Update()
    {
        // if player is not moving yet check if player has been asked to move based on WASD input from user
        if(!isMoving)
        {
            if(Input.GetKey(moveForward))
            {
                LookAtDirection(new Vector3(0.0f, 0.0f, 0.0f));
                if(!isMoving)UpdatePlayerTarget(Vector3.forward);
            }
            if(Input.GetKey(moveLeft))
            {
                LookAtDirection(new Vector3(0.0f, -90.0f, 0.0f));
                if (!isMoving) UpdatePlayerTarget(Vector3.left);

            }
            if(Input.GetKey(moveBackward))
            {
                LookAtDirection(new Vector3(0.0f, 180.0f, 0.0f));
                if (!isMoving) UpdatePlayerTarget(Vector3.back);

            }
            if(Input.GetKey(moveRight))
            {
                LookAtDirection(new Vector3(0.0f, 90.0f, 0.0f));
                if (!isMoving) UpdatePlayerTarget(Vector3.right);
            }
        }

    }

    /// <summary>
    /// activate player movement 
    /// </summary>
    /// <param name="direction">direction to move in</param>
    private void UpdatePlayerTarget(Vector3 direction)
    {
        
        Vector3 targetDirection = transform.position + (direction * cellStep);
        IEnumerator coroutine = MoveTowards(targetDirection);
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// Player movement Coroutine to pause other activity while player moves
    /// can be a normal function if we dont want to stop other activity
    /// </summary>
    /// <param name="targetDirection">direction to move in</param>
    /// <returns>wait a sec after moving and then continue game</returns>
    IEnumerator MoveTowards(Vector3 targetDirection)
    {
        isMoving = true;
        // Move our position a step closer to the target.
        float step =  speed * Time.deltaTime; // calculate distance to move
        while(transform.position != targetDirection)
        {
            yield return new WaitForSeconds(0.001f);
            transform.position = Vector3.MoveTowards(transform.position, targetDirection, step);
        }
        //yield return new WaitForSeconds(0.001f);
        isMoving = false;
    } 

    /// <summary>
    /// Rotate player to look in the the direction it is moving
    /// </summary>
    /// <param name="direction">direction player is moving</param>
    private void LookAtDirection(Vector3 direction)
    {   
        body.localRotation = Quaternion.Euler(direction);
    }

}
