using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
     
    private void Start()
    {
        int highscore = PlayerPrefs.GetInt("Highscore", -1);
        if (highscore == -1)
        {
            PlayerPrefs.SetInt("Highscore", 0);
            highscore = 0;
        }

        score.text = highscore.ToString();
    }

    public void Play()
    {
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
    }
    
    public void Tuto()
    {
        SceneManager.LoadScene("Tuto", LoadSceneMode.Single);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        
        Application.Quit();
    }

    public void Parameters()
    {
        //SceneManager.LoadScene("Parameters", LoadSceneMode.Single);
    }
}
