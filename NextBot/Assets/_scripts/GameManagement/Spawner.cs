using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lvlInfo;
    // Start is called before the first frame update
    void Start()
    {
        lvlInfo.text = $"Map : {GetMapName()}, \n" +
            $"next bots : {GetBotsNames()[0]},{GetBotsNames()[1]},{GetBotsNames()[2]} \n" +
            $"abilities : {ab1()}, {ab2()} ";
    }

    string GetMapName()
    {
        return SceneManager.GetActiveScene().name;
    }
    List<string> GetBotsNames()
    {
        return DataManager.LoadSelectedNextBots();
    }
    string ab1()
    {
        return GameManager.ability1.AbilityName;
    }
    string ab2()
    {
        return GameManager.ability2.AbilityName;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
