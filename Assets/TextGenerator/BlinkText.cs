using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{

    private TextMeshProUGUI text;
    private bool fadingIn;
    private bool fadingOut;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        fadingIn = true;
        fadingOut = false;
    }


    private void Update()
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
