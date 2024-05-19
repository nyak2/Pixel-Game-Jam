using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITween : MonoBehaviour
{
    [Header ("MainMneu UI")]
    public GameObject mainMenuUi;
    [SerializeField] private float mainMenutime;
    [SerializeField] private float mainMenuDestination;
    [SerializeField] private LeanTweenType mainMenutypeIN;
    [SerializeField] private LeanTweenType mainMenutypeOUT;
    private Vector3 oriMainMenuPos;

    [Header ("Control UI")]
    public GameObject controlUi;
    [SerializeField] private float controltime;
    [SerializeField] private float controldestination;
    [SerializeField] private LeanTweenType controltypeIN;
    [SerializeField] private LeanTweenType controltypeOUT;
    private Vector3 oriControlPos;
    private void Start()
    {
        oriMainMenuPos = mainMenuUi.transform.position;
        oriControlPos= controlUi.transform.position;

        LeanTween.moveLocalX(mainMenuUi, mainMenuDestination , mainMenutime).setEase(mainMenutypeIN).setDelay(0.3f);
    }

    public void TweenControlsIn()
    {
        LeanTween.moveLocalX(mainMenuUi, -1369, 0.4f).setEase(mainMenutypeOUT).setOnComplete(ReturnOriMainMenuPos);
        LeanTween.moveLocalX(controlUi, controldestination, controltime).setEase(controltypeIN).setDelay(0.35f);
    }

    public void TweenMainMenuBack()
    {
        LeanTween.moveLocalX(controlUi, -1549, 0.4f).setEase(controltypeOUT).setOnComplete(ReturnOriControlPos);
        LeanTween.moveLocalX(mainMenuUi, mainMenuDestination, mainMenutime).setEase(mainMenutypeIN).setDelay(0.35f);
    }

    void ReturnOriMainMenuPos()
    {
        mainMenuUi.transform.position = oriMainMenuPos;
    }
    void ReturnOriControlPos()
    {
        controlUi.transform.position = oriControlPos;
    }
}
