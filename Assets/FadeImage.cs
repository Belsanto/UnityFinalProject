using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void FadeIn()
    {
        image.DOFade(1f, 1f);
    }


    public void FadeOut()
    {
        image.DOFade(0f, 1f);

    }

}
