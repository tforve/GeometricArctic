using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuControll : MonoBehaviour
{
    [SerializeField] private GameObject startButton, exitButton;
    [SerializeField] private CanvasGroup title;

    [SerializeField] private float showMenuIn = 7.0f;
    [SerializeField] private float lerpTime = 0.5f;

    public void StartNewGame()
    {
        SceneManager.LoadScene("Level00_Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Start()
    {
        startButton.SetActive(false);
        exitButton.SetActive(false);
        StartCoroutine(ShowTitel());
    }

    public IEnumerator ShowTitel()
    {
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(FadeTitle(title, title.alpha, 1.0f, lerpTime));
        yield return new WaitForSeconds(showMenuIn);
        StartCoroutine(FadeTitle(title, title.alpha, 0.0f, lerpTime));
        yield return new WaitForSeconds(3.0f);
        exitButton.SetActive(true);
        startButton.SetActive(true);
    }

    public IEnumerator FadeTitle(CanvasGroup title, float start, float end, float lerpTime)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            title.alpha = currentValue;

            if (percentageComplete >= 1) break;
            yield return new WaitForEndOfFrame();
        }

   
    }

    // public void FadeIn()
    // {
    //     StartCoroutine(FadeTitle(title, title.alpha, 1.0f, lerpTime));
    // }

    // public void FadeOut()
    // {
    //     StartCoroutine(FadeTitle(title, title.alpha, 0.0f, lerpTime));
    // }
}
