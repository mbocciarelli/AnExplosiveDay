using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private GameManager _gameManager;

    private Vector3 offset = new Vector3(0, 60, -10);
    private Vector3 offsetTuto = new Vector3(0, 43, -20);

    private float smoothSpeed = 0.125f;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        Vector3 desiredPosition;
        
        if(_gameManager.isTuto) 
            desiredPosition = _player.transform.position + offsetTuto;
        else
            desiredPosition = _player.transform.position + offset;
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
