using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceInGame : MonoBehaviour
{
    private PlayerController _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }


    public void Resume()
    {
        _player.ContinueGame();
    }
    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        
    }
}
