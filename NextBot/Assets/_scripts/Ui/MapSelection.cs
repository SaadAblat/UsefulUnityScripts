using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class MapSelection : MonoBehaviour
{


    int map1Index;
    int map2Index;


    [SerializeField] MapInfo[] allMaps;

    internal MapInfo map1;
    internal MapInfo map2;

    private MapInfo selectedMap;

    [SerializeField] Image MapImageSlot1;
    [SerializeField] Image MapImageSlot2;

    [SerializeField] TextMeshProUGUI MapNameSlot1;
    [SerializeField] TextMeshProUGUI MapNameSlot2;

    [SerializeField] Button LoadSelectedMapButton;




    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Maps_Are_Generated())
        {
            LoadGeneratedMapsIndex();
        }
        else
        {
            GenerateTwoMapsIndex();
          
        }
        PopulateMapsData();
        PopulateMapsSlots();
    }

    void LoadGeneratedMapsIndex()
    {
        map1Index = allMaps.FirstOrDefault(x => x.MapIndex == DataManager.LoadGeneratedMaps()[0]).MapIndex;
        Debug.Log($"map1 index loaded{map1Index}");
        map2Index = allMaps.FirstOrDefault(x => x.MapIndex == DataManager.LoadGeneratedMaps()[1]).MapIndex;
        Debug.Log($"map2 index loaded{map1Index}");
    }
    private void Update()
    {
        ManageButtons();
    }




    void GenerateTwoMapsIndex()
    {
        
        int minMapIndex = allMaps[0].MapIndex;
        int maxMapIndex = allMaps[allMaps.Length - 1].MapIndex;

        map1Index = Random.Range(minMapIndex, maxMapIndex+1);
        Debug.Log($"map1 index Generated {map1Index}");
        map2Index = Random.Range(minMapIndex, maxMapIndex+1);
        
        while (map1Index == map2Index)
        {
            map2Index = Random.Range(minMapIndex, maxMapIndex);
        }
        Debug.Log($"map2 index Generated {map2Index}");

        DataManager.Save_availabe_maps( map1Index, map2Index );
        Debug.Log($"map 1 index and map 2 index Saved{map1Index},{map2Index} ");

    }
    void PopulateMapsData()
    {
        //Todo You need to make sure map scene are placed together otherwise it can throw an error
        map1 = allMaps.FirstOrDefault(x=>x.MapIndex == map1Index);
        Debug.Log($"map1 Found {map1.MapIndex}");
        map2 = allMaps.FirstOrDefault(x => x.MapIndex == map2Index);
        Debug.Log($"map2 Found {map2.MapIndex}");
    }
    void PopulateMapsSlots()
    {
        MapImageSlot1.sprite = map1.Image;
        MapImageSlot2.sprite = map2.Image;

        MapNameSlot1.text = map1.Name;
        MapNameSlot2.text = map2.Name;
    }

    public void SelectMap1()
    {
        selectedMap = map1;
    }
    public void SelectMap2()
    {
        selectedMap = map2;
    }
    public void LoadselectedMap()
    {
        DataManager.Save_selected_map(selectedMap.MapIndex);
        GameManager.LoadScene(DataManager.AbilitySelection);
    }

    void ManageButtons()
    {
        if (selectedMap == null)
        {
            LoadSelectedMapButton.interactable = false;
        }
        else
        {
            LoadSelectedMapButton.interactable = true;
        }
    }
    public void ClearSelections()
    {
        DataManager.ClearAllChoices();
        Debug.Log("Selections Cleared");
    }
    // Update is called once per frame
    
}
