using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITween : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUi;
    [SerializeField] private float time;
    [SerializeField] private float destination;
    [SerializeField] private LeanTweenType type;
    private void Start()
    {
        LeanTween.moveLocalX(mainMenuUi,destination , time).setEase(type);
    }
}
