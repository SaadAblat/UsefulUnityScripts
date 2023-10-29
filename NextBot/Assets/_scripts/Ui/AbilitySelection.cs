using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AbilitySelection : MonoBehaviour
{

    [SerializeField] AbilityInfo[] allAbilities;
    AbilityInfo ability1;
    AbilityInfo ability2;

    [SerializeField] TextMeshProUGUI ability1Text;
    [SerializeField] TextMeshProUGUI ability2Text;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRandomAbilities();
        PopulateAbilitiesSlots();
    }

    void GenerateRandomAbilities()
    {

        if (allAbilities.Length >= 2)
        {
            // Randomly pick two abilities
            int randomIndex1 = Random.Range(0, allAbilities.Length);
            int randomIndex2;

            do
            {
                randomIndex2 = Random.Range(0, allAbilities.Length);
            } while (randomIndex2 == randomIndex1);

            // Assign the selected abilities
            ability1 = allAbilities[randomIndex1];
            ability2 = allAbilities[randomIndex2];

        }
        else
        {
            Debug.LogWarning("There are not enough abilities in the allAbilities array to select two.");
        }

    }


    void PopulateAbilitiesSlots()
    {
        ability1Text.text = ability1.AbilityName;
        ability2Text.text = ability2.AbilityName;
    }

    public void LoadMap()
    {
        int selectedMapIndex = DataManager.LoadSelectedMap();
        GameManager.ability1 = ability1;
        GameManager.ability2 = ability2;
        GameManager.LoadScene(selectedMapIndex);
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
