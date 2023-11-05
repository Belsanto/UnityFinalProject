using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{

    private TextMeshProUGUI text;
    private TextMeshPro textMeshPro;
    private bool fadingIn;
    private bool fadingOut;
    private bool usingTMP;

    private void Start()
    {
        usingTMP = false;
        text = GetComponent<TextMeshProUGUI>();
        if (text == null )
        {
            textMeshPro = GetComponent<TextMeshPro>();
            usingTMP = true;
        }
        fadingIn = true;
        fadingOut = false;
    }


    private void Update()
    {

        if (usingTMP)
        {
            if (fadingIn)
            {
                fadingIn = false;
                textMeshPro.DOFade(0, 1f).OnComplete(() =>
                fadingOut = true
                );
            }

            if (fadingOut)
            {
                fadingOut = false;
                textMeshPro.DOFade(1, 1f).OnComplete(() =>
                fadingIn = true
                );
            }
        } else
        {
            if (fadingIn)
            {
                fadingIn = false;
                text.DOFade(0, 1f).OnComplete(() =>
                fadingOut = true
                );
            }

            if (fadingOut)
            {
                fadingOut = false;
                text.DOFade(1, 1f).OnComplete(() =>
                fadingIn = true
                );
            }

        }

    }


}
