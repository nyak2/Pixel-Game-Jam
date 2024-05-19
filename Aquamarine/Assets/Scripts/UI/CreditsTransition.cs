using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsTransition : MonoBehaviour
{
    [SerializeField] private GameObject creditObject;
    [SerializeField] private Image creditUiObject;
    [SerializeField] private Image blackscreen;
    [SerializeField] private float time;
    [SerializeField] private float pos;
    // Start is called before the first frame update
    void Start()
    {
        blackscreen.CrossFadeAlpha(0, 0.3f, false);
        LeanTween.moveLocalY(creditObject, pos, time).setOnComplete(BackToMainMenu);
    }


    void BackToMainMenu()
    {
        StartCoroutine(StartBackToMainMenu());
    }

    IEnumerator StartBackToMainMenu()
    {
        blackscreen.CrossFadeAlpha(1, 0.3f, false);
        creditUiObject.CrossFadeAlpha(0, 0.3f, false);
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(0);
    }

}
