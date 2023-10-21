
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] TMP_Text _scoreText;
    [SerializeField] TMP_Text _coinText;
    [SerializeField] Button _showLevelUpMenu;
    bool _isLevelUpMenuActive;
    [SerializeField] GameObject _levelUpMenu;
    [SerializeField] TMP_Text _levelUpFireRateText;
    [SerializeField] TMP_Text _levelUpFireRatePriceText;
    [SerializeField] CanonScript _canonScript;
    int fireRateLvl;
    [SerializeField] int fireRateUpgradePrice;
    [SerializeField] Button _upgradeLevelUp;

    public bool GameOver;
    // Start is called before the first frame update
    void Start()
    {
        fireRateLvl = 1;

    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = Score.GameScore.ToString();
        _coinText.text = Score.Coins.ToString();
        _levelUpFireRateText.text = "Fire rate lvl" + fireRateLvl;
        _levelUpFireRatePriceText.text = fireRateUpgradePrice.ToString();

        ManageLevelUpMenu();

        if (Score.Coins >= fireRateUpgradePrice)
        {
            _upgradeLevelUp.interactable = true;
        }
        else
        {
            _upgradeLevelUp.interactable = false;
        }

        if (GameOver)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
            Score.Coins = 0;
            Score.GameScore = 0;
            // Add a best score logic
        }


    }

    public void ShowLevelUpMenu()
    {
        _isLevelUpMenuActive = !_isLevelUpMenuActive;
    }
    void ManageLevelUpMenu()
    {
        if (_isLevelUpMenuActive)
        {
            _levelUpMenu.SetActive(true);
        }
        else
        {
            _levelUpMenu.SetActive(false);
        }
    }

    public void LevelUpFireRate()
    {
        _canonScript.fireDelay -= 0.1f;
        fireRateLvl++;
        fireRateUpgradePrice *= 2;
        Score.Coins -= fireRateUpgradePrice;
    }
}
