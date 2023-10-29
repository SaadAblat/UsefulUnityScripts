using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    //Keys
    const string NextBotOption1 = "NextBot_option_1";
    const string NextBotOption2 = "NextBot_option_2";
    const string NextBotOption3 = "NextBot_option_3";

    const string NextBotsGenerated = "NextBots_generated";

    const string NextBotSelected1 = "NextBot_selected_1";
    const string NextBotSelected2 = "NextBot_selected_2";
    const string NextBotSelected3 = "NextBot_selected_3";

    const string NextBotsSelected = "NextBots_selected";

    const string MapOption1Index = "Map_option_1";
    const string MapOption2Index = "Map_option_2";

    const string MapsAreGenerated = "Maps_generated";

    const string SelectedMapIndex = "Map_selected";


    const string MapIsSelected = "Maps_selected";


    // Scene names 
    
    public const string MainMenu = "Main_menu";
    public const string NextBotsSelection = "NextBots_Selection";
    public const string MapsSelection = "Maps_Selection";
    public const string AbilitySelection = "Ability_selection";

    //Map Names
    public const string Map_Forest = "forest";
    public const string Map_AmongUs = "Among_us";
    public const string Map_Parking = "Parking";

    //Next Bots Names
    public const string Obunga = "Obunga";
    public const string Banana = "Banana";
    public const string Dogdog = "Dogdog";
    public const string DuckDuck = "DuckDuck";
    public const string Potatoe = "Potatoe";

    public static void Save_availabe_nextBots(string one, string two, string three)
    {
        PlayerPrefs.SetString(NextBotOption1, one);
        PlayerPrefs.SetString(NextBotOption2, two);
        PlayerPrefs.SetString(NextBotOption3, three);

        PlayerPrefs.SetInt(NextBotsGenerated, 1);
    }
    public static void Save_selected_nextBots(string one, string two, string three)
    {
        PlayerPrefs.SetString(NextBotSelected1, one);
        PlayerPrefs.SetString(NextBotSelected2, two);
        PlayerPrefs.SetString(NextBotSelected3, three);

        PlayerPrefs.SetInt(NextBotsSelected, 1);
    }

    //Maps
    public static void Save_availabe_maps(int map1Index, int map2Index)
    {
        PlayerPrefs.SetInt(MapOption1Index, map1Index);
        PlayerPrefs.SetInt(MapOption2Index, map2Index);

        PlayerPrefs.SetInt(MapsAreGenerated, 1);
    }
    public static void Save_selected_map(int selectedMapIndex)
    {
        PlayerPrefs.SetInt(SelectedMapIndex, selectedMapIndex);

        PlayerPrefs.SetInt(MapIsSelected, 1);
    }

    public static bool NextBots_Are_Generated()
    {
        int value = PlayerPrefs.GetInt(NextBotsGenerated);
        if (value == 1) return true; else return false;
    }
    public static bool NextBots_Are_Selected()
    {
        int value = PlayerPrefs.GetInt(NextBotsSelected);
        if (value == 1) return true; else return false;
    }
    public static bool Maps_Are_Generated()
    {
        int value = PlayerPrefs.GetInt(MapsAreGenerated);
        if (value == 1) return true; else return false;
    }
    public static bool Map_Is_Selected()
    {
        int value = PlayerPrefs.GetInt(MapIsSelected);
        if (value == 1) return true; else return false;
    }

    public static List<string> LoadGeneratedNextBots()
    {
        List<string> result = new List<string>();

        string one = PlayerPrefs.GetString(NextBotOption1);
        string two = PlayerPrefs.GetString(NextBotOption2);
        string three = PlayerPrefs.GetString(NextBotOption3);

        result.Add(one);
        result.Add(two);
        result.Add(three);

        return result;
    }

    public static List<string> LoadSelectedNextBots()
    {
        List<string> result = new List<string>();

        string one = PlayerPrefs.GetString(NextBotSelected1);
        string two = PlayerPrefs.GetString(NextBotSelected2);
        string three = PlayerPrefs.GetString(NextBotSelected3);

        result.Add(one);
        result.Add(two);
        result.Add(three);

        return result;
    }

    public static List<int> LoadGeneratedMaps()
    {
        List<int> result = new List<int>();

        int one = PlayerPrefs.GetInt(MapOption1Index);
        int two = PlayerPrefs.GetInt(MapOption2Index);

        result.Add(one);
        result.Add(two);

        return result;
    }
    public static int LoadSelectedMap()
    {
        return PlayerPrefs.GetInt(SelectedMapIndex);
    }

    public static void ClearAllChoices()
    {
        PlayerPrefs.SetInt(NextBotsSelected, 0);
        PlayerPrefs.SetInt(MapIsSelected, 0);
        PlayerPrefs.SetInt(NextBotsGenerated, 0);
        PlayerPrefs.SetInt(MapsAreGenerated, 0);
    }


}
