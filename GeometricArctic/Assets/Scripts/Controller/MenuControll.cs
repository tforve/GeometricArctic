using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuControll : MonoBehaviour
{
    [SerializeField] private GameObject startButton, exitButton;
    [SerializeField] private Image title;
    [SerializeField] private Animator titelAnim;
    [SerializeField] private Animator blendAnim;


    [SerializeField] private float showMenuIn = 7.5f;
    [SerializeField] private float lerpTime = 0.5f;

    public void StartNewGame()
    {
        StartCoroutine(StartNewGameRoutine());
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

    public IEnumerator StartNewGameRoutine()
    {
        yield return new WaitForSeconds(0.7f);
        blendAnim.SetBool("blendIn", true);
        yield return new WaitForSeconds(5.0f);

        SceneManager.LoadScene("Level00_Tutorial");
    }

    public IEnumerator ShowTitel()
    {
        yield return new WaitForSeconds(2.0f);
        //StartCoroutine(FadeTitle(title, title.color.a, 1.0f, lerpTime));
        titelAnim.SetBool("fadeIn", true);
        yield return new WaitForSeconds(showMenuIn);
       // StartCoroutine(FadeTitle(title, title.color.a, 0.0f, lerpTime));
        yield return new WaitForSeconds(3.0f);
        exitButton.SetActive(true);
        startButton.SetActive(true);
    }

    public IEnumerator FadeTitle(Image title, float start, float end, float lerpTime)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);
            float tmp = this.title.color.a;
            tmp = currentValue;

            if (percentageComplete >= 1) break;
            yield return new WaitForEndOfFrame();
        }

   
    }


}
