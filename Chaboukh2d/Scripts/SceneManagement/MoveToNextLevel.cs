using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextLevel : MonoBehaviour
{
    int nextSceenLoad;
    [SerializeField] int lastLevel;
    [SerializeField] SceneFader sceneFader;
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject DeathPanel;
    // Mobile
    [SerializeField] GameObject ControlsPannel;

    bool soundPlayed;
    bool timeToNormal;

    // Start is called before the first frame update

    enum CanvasState
    {
        controlPanel,
        winPanel,
        deathPanel,
        pausePanel,
    }

    CanvasState canvasState;

    void Start()
    {
        nextSceenLoad = SceneManager.GetActiveScene().buildIndex + 1;
        soundPlayed = false;
        canvasState = CanvasState.controlPanel;
    }


    // Update is called once per frame
    void Update()
    {
        switch (canvasState)
        {
            case CanvasState.controlPanel:
                ShowControlsPannel();
                break;

            case CanvasState.pausePanel:

                break;

            case CanvasState.winPanel:
                int oldLevelReached = PlayerPrefs.GetInt("LevelReached");
                int recentLevelReached = nextSceenLoad - 1;
                if (recentLevelReached > oldLevelReached)
                {
                    PlayerPrefs.SetInt("LevelReached", nextSceenLoad - 1);
                }
                StartCoroutine(StopPlayerMovement(0.2f));
                StartCoroutine(ShowWinPanel());
                if (WinPanel.activeSelf && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)))
                {
                    LoadNextLevel();
                }
                break;

            case CanvasState.deathPanel:
                StartCoroutine(ShowDeathPannel());
                break;


        }



        if (PlayerScript.Instance.PlayerIsDead && !PlayerScript.Instance.OpenedTheDoor)
        {
            canvasState = CanvasState.deathPanel;
        }
        if (PlayerScript.Instance.OpenedTheDoor)
        {
            canvasState = CanvasState.winPanel;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }


    public void ShowPausePannel()
    {
        HideControlsPannel();
        PausePanel.SetActive(true);
        if (PausePanel.activeSelf)
        {
            if (timeToNormal)
            {
                Time.timeScale = 0;
                timeToNormal = false;
            }
        }
        canvasState = CanvasState.pausePanel;
    }
    public void HidePausePannel()
    {
        PausePanel.SetActive(false);
        if (!timeToNormal)
        {
            Time.timeScale = 1;
            timeToNormal = true;
        }
        canvasState = CanvasState.controlPanel;
    }
    IEnumerator ShowWinPanel()
    {
        yield return new WaitForSeconds(1f);
        // Mobile
        HideControlsPannel();
        WinPanel.SetActive(true);
        if (!soundPlayed)
        {
            AudioManager.instance.Play("LevelCleared");
            soundPlayed = true;
        }
    }
    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == lastLevel)
        {
        }
        else
        {
            sceneFader.FadeTo(nextSceenLoad);
        }
    }
    public void LoadSelectLevet()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelSelect");
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void Restart()
    {
        string CurrentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(CurrentScene);
    }
    IEnumerator ShowDeathPannel()
    {
        yield return new WaitForSeconds(1f);
        DeathPanel.SetActive(true);
        if (!soundPlayed)
        {
            AudioManager.instance.Play("Bass");
            soundPlayed = true;
        }

        //Mobile
        HideControlsPannel();
    }
    public void HideControlsPannel()
    {
        if (ControlsPannel != null)
        {
            ControlsPannel.SetActive(false);
        }
    }
    public void ShowControlsPannel()
    {
        if (ControlsPannel != null)
        {
            ControlsPannel.SetActive(true);
        }
        if (PausePanel.activeSelf)
        {
            HidePausePannel();
        }
    }
    IEnumerator StopPlayerMovement(float time)
    {
        yield return new WaitForSeconds(time);
        PlayerScript.Instance.playerInputs.horizontal = 0;
        PlayerScript.Instance.PlayerRigideBody.velocity = new Vector2(0, PlayerScript.Instance.PlayerRigideBody.velocity.y);
    }




}
