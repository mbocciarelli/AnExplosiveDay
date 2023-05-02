using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private String levelName = null;

    public void LoadLevel()
    {
        if (levelName != null)
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        } 
    }
}
