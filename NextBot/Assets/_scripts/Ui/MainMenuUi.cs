using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        if (DataManager.NextBots_Are_Selected())
        {
            if (DataManager.Map_Is_Selected()) 
            {
                GameManager.LoadScene(DataManager.AbilitySelection);
            }
            else
            {
                GameManager.LoadScene(DataManager.MapsSelection);
            }
           
        }
        else
        {
            GameManager.LoadScene(DataManager.NextBotsSelection);
        }
    }

    public void ClearData()
    {
        DataManager.ClearAllChoices();
    }
}
