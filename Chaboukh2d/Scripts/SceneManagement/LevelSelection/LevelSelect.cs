using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] SceneFader sceneFader;
    public Button[] levelButons;
    // Start is called before the first frame update

    private void Start()
    {
        int levelReached = PlayerPrefs.GetInt("LevelReached", 1);
        
        for (int i = 0; i < levelButons.Length; i++)
        {
            if (i+1 > levelReached)
            {
              levelButons[i].interactable = false;
            }
        }
    }
    public void Select(int levelBuildIndex)
    {
        Time.timeScale = 1;
        sceneFader.FadeTo(levelBuildIndex+1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteKey("LevelReached");
    }

    public void UnlockAllLevels()
    {
        PlayerPrefs.SetInt("LevelReached", 50);
    }
}
