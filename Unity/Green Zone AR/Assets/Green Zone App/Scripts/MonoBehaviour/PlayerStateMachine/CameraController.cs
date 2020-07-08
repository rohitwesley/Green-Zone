using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Player To Set Camera Position")]
    [SerializeField] private GameObject _player;

    private Vector3 _offset;

    void Start ()
    {
        _offset = transform.position - _player.transform.position;
    }

    void LateUpdate ()
    {
        Vector3 newPosition = _player.transform.position + _offset;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }

}
