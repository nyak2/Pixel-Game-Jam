using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public TMP_Text skipObjectText;
    public Image blackScreenObject;
    
    public TMP_Text dialogueText;
    public List<string> dialogues;
    public float writingSpeed;

    [Header("Don't add anything here. This is used for changing scenes")]
    [SerializeField] private UnityEvent onEnd;

    private int index;
    private int charIndex;
    [HideInInspector]
    public bool started;
    private bool waitForNext;
    private bool isWritting;

    [SerializeField] private AudioSource dialogoueSfx;
    [SerializeField] private GameObject showAbilityText = null;

    private void Awake()
    {
        ToggleWindow(false);
    }

    public void ToggleWindow(bool show)
    {
        StartCoroutine(FadeBlackScreen(show));
    }


    private IEnumerator FadeBlackScreen(bool show)
    {
        if(show)
        {
            //textObject.SetActive(show);
            //blackScreenObject.gameObject.SetActive(show);

            blackScreenObject.CrossFadeAlpha(1, 0.5f, false);
            yield return new WaitForSeconds(0.4f);
            dialogueText.CrossFadeAlpha(1,0.4f,false);
        }
        else
        {
            dialogueText.CrossFadeAlpha(0,0.4f,false);
            yield return new WaitForSeconds(0.5f);
            if (onEnd.GetPersistentEventCount() == 0)
            {
                blackScreenObject.CrossFadeAlpha(0, 0.4f, false);
            }
            //textObject.SetActive(show);
            //blackScreenObject.gameObject.SetActive(show);

        }
    }
    public void StartDialogue()
    {
        started = true;
        isWritting = true; //IDK why but yes
        ToggleWindow(true); //Show the window
        GetDialogue(0); //Start with first dialogue
    }
    private void GetDialogue(int i)
    {
        index = i;
        charIndex = 0;
        StartCoroutine(Writing());
        dialogueText.text = string.Empty;
    }

    public IEnumerator EndDialogue()
    {
        started = false;
        waitForNext = false;
        ToggleWindow(false);
        yield return new WaitForSeconds(0.5f);
        if (onEnd.GetPersistentEventCount() > 0)
        {
           onEnd.Invoke();
        }
        if (showAbilityText != null) StartCoroutine(ShowSaveText());
    }

    private IEnumerator ShowSaveText()
    {
        Vector3 tempos = showAbilityText.transform.position;
        LeanTween.moveLocalX(showAbilityText, 0, 0.5f).setEaseOutBack();
        yield return new WaitForSeconds(2.5f);
        LeanTween.moveLocalX(showAbilityText, -1367, 0.5f).setEaseInBack();
        yield return new WaitForSeconds(1.0f);
        showAbilityText.transform.position = tempos;
    }

    IEnumerator Writing()
    {
        yield return new WaitForSeconds(writingSpeed);

        string currentDialogue = dialogues[index];
        if (isWritting)
        {
            dialogueText.text += currentDialogue[charIndex];
            charIndex++;
        }

        if (charIndex < currentDialogue.Length && isWritting)
        {
            yield return new WaitForSeconds(writingSpeed);
            StartCoroutine(Writing());
        }
        else
        {
            waitForNext = true; //End this sentence and wait for the next one
            isWritting = false;
            skipObjectText.gameObject.SetActive(true);

        }
    }
    private void Update()
    {
        if (!started)
            return;


        if (Input.GetMouseButtonDown(0) && isWritting)
        {
            // Insert SFX for continuing dialogue
            StopCoroutine(Writing());
            dialogueText.text = dialogues[index].ToString();
            isWritting = false;
        }

        if (waitForNext && Input.GetMouseButtonDown(0))
        {
            dialogoueSfx.Play();
            isWritting = true;
            waitForNext = false;
            index++;
            skipObjectText.gameObject.SetActive(false);
            if (index < dialogues.Count)
            {
                GetDialogue(index);
            }
            else
            {
                StartCoroutine(EndDialogue());
            }
        }
    }

    public void TransitionScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1) ;
    }

}
