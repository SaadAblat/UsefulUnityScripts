using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public Image Img;
    public AnimationCurve curve;

    private void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(FadeIn());
        
    }
    public void FadeTo(int Scene)
    {
        StartCoroutine(FadeOut(Scene));
    }
    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            Img.color = new Color(0,0,0,a);
            yield return 0;

       }
    }
    IEnumerator FadeOut(int Scene)
    {
        
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            Img.color = new Color(0, 0, 0, a);
            yield return 0;
            //Loading the scene
            
            SceneManager.LoadScene(Scene);
        }
    }

    public void VibrateButton()
    {
        Handheld.Vibrate();
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.Play("ButtonClick");
    }

    public void Quit()
    {
        StartCoroutine(quitAfterSoundBeenPlayed());
        
    }
    IEnumerator quitAfterSoundBeenPlayed()
    {
        yield return new WaitForSeconds(0.2f);
        Application.Quit();

    }
}
