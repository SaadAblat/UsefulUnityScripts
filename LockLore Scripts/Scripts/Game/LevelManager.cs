
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    PlayerScript player;
    [SerializeField] GameObject GameOverMenu;

    private void Awake()
    {
        player = FindObjectOfType<PlayerScript>();
    }
    // Update is called once per frame
    void Update()
    {
        if (player.hasOpenedTheDoor)
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
        if (player.IsDead)
        {
            Invoke(nameof(ShowGameOverMenu), 2f);
        }
    }

    public void LoadNextLevel()
    {
        int curSceneIndx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(curSceneIndx + 1);
    }
    public void RestartLevel()
    {
        string curSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(curSceneName);
    }

    void ShowGameOverMenu()
    {
        GameOverMenu.SetActive(true);
    }
}
