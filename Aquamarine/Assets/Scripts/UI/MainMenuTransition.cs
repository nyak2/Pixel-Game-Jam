using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuTransition : MonoBehaviour
{
    [SerializeField] private AudioSource uiClickSfx;
    [SerializeField] private Image blackScreenObject;

    [SerializeField] private UITween tween;
    private bool once;
    // Start is called before the first frame update
    void Start()
    {
        blackScreenObject.CrossFadeAlpha(0, 0.3f, false);
        if (PlayerPrefs.HasKey("WaterBed") || PlayerPrefs.HasKey("AquaHop")  || PlayerPrefs.HasKey("HydroWard") || PlayerPrefs.HasKey("PuddleBuddy"))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void Play()
    {
        if(!once)
        {
            once = true;
            uiClickSfx.Play();
            StartCoroutine(TransitionToPlayScene());
        }
    }

    IEnumerator TransitionToPlayScene()
    {
        blackScreenObject.CrossFadeAlpha(1,0.3f, false);
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(1);
    }

    public void ShowControls()
    {
        if(!LeanTween.isTweening(tween.mainMenuUi) && !LeanTween.isTweening(tween.controlUi))
        {
            uiClickSfx.Play();
            tween.TweenControlsIn();
        }
    }

    public void BackToMainMenu()
    {
        if (!LeanTween.isTweening(tween.mainMenuUi) && !LeanTween.isTweening(tween.controlUi))
        {
            uiClickSfx.Play();
            tween.TweenMainMenuBack();
        }
    }

    public void Quit()
    {
        uiClickSfx.Play();
        Application.Quit();
    }
}
