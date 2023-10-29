
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
public class NextBotSelect : MonoBehaviour
{
    [SerializeField] NextBotInfo[] allBots;
    NextBotInfo bot1;
    NextBotInfo bot2;
    NextBotInfo bot3;

    [SerializeField] TextMeshProUGUI bot1Text;
    [SerializeField] TextMeshProUGUI bot2Text;
    [SerializeField] TextMeshProUGUI bot3Text;
    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.NextBots_Are_Generated())
        {
            LoadGeneratedRandomBots();
        }
        else
        {
            GenerateRandomBots();
        }
        PopulateMapsSlots();
    }

    void GenerateRandomBots()
    {
        int minBotIndex = 0;
        int maxBotIndex = allBots.Length - 1;
        bot1 = allBots[Random.Range(minBotIndex, maxBotIndex+1)];
        bot2 = allBots[Random.Range(minBotIndex, maxBotIndex+1)];
        bot3 = allBots[Random.Range(minBotIndex, maxBotIndex + 1)];
        while (bot1 == bot2)
        {
            bot2 = allBots[Random.Range(minBotIndex, maxBotIndex + 1)];
        }
      
        while (bot3 == bot1 || bot3 == bot2)
        {
            bot3 = allBots[Random.Range(minBotIndex, maxBotIndex + 1)];
        }

        DataManager.Save_availabe_nextBots(bot1.name, bot2.name, bot3.name);
    }
    void LoadGeneratedRandomBots()
    {
        bot1 = allBots.FirstOrDefault(x => x.BotName == DataManager.LoadGeneratedNextBots()[0]);
        bot2 = allBots.FirstOrDefault(x => x.BotName == DataManager.LoadGeneratedNextBots()[1]);
        bot3 = allBots.FirstOrDefault(x => x.BotName == DataManager.LoadGeneratedNextBots()[2]);
    }

    void PopulateMapsSlots()
    {
       bot1Text.text = bot1.BotName;
       bot2Text.text = bot2.BotName;
       bot3Text.text = bot3.BotName;
    }

    public void LoadMapSelectionMenu()
    {
        DataManager.Save_selected_nextBots(bot1.name, bot2.name, bot3.name);
        if (!DataManager.Map_Is_Selected())
        {
            GameManager.LoadScene(DataManager.MapsSelection);
        }
        else
        {
            GameManager.LoadScene(DataManager.AbilitySelection);
        }
       
    }

    public void ClearSelections()
    {
        DataManager.ClearAllChoices();
        Debug.Log("Selections Cleared");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
