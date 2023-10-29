using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    internal static AbilityInfo ability1;
    internal static AbilityInfo ability2;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }


    }



    public static void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public static void LoadScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
