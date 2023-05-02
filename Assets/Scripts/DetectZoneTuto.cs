using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectZoneTuto : MonoBehaviour
{
    private GameManager _gameManager;
    
    private void Start()
    {
        _gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_gameManager != null)
            _gameManager.isPossibleToExplodeInTuto = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (_gameManager != null)
            _gameManager.isPossibleToExplodeInTuto = false;
    }
}
