using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Hashtable Levels;
    
    public Button[] buttons;

    private int lastLevelPlayed = 1;

    private void Awake()
    {
        Levels = new Hashtable();
    }

    private void Start()
    {
        buttons = FindObjectsOfType<Button>();
        
        foreach (var button in buttons)
        {
            String levelNumberString = button.name.ToLower().Replace("level ", "");
            int levelNumber = Int32.Parse(levelNumberString);

            if (levelNumber <= lastLevelPlayed)
            {
                button.interactable = true;
            }
            
            Levels.Add(levelNumber, button);
            
        }
    }
}
