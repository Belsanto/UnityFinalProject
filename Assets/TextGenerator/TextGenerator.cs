using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextGenerator : MonoBehaviour
{
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private List<string> displayTextList = new List<string>();
    [SerializeField] private float characterDelay = 0.1f;
    [SerializeField] private int buildIndex;

    private bool showAllText = false;
    private int currentIndex = 0;

    private string currentString = ""; 
    private string cursor = "|"; 

    void Start()
    {
        StartCoroutine(AnimateTextList());
    }

    void Update()
    {
        // Si se da clic
        if (Input.GetMouseButtonDown(0))
        {
            //Si se esta mostrando todo el string mustra todo el texto
            if (showAllText)
            {
                //mientras el index este dentro de el largo de la lista
                if (currentIndex < displayTextList.Count - 1)
                {
                    currentIndex++;
                    StopAllCoroutines();
                    showAllText = false; 
                    currentString = ""; 
                    StartCoroutine(AnimateTextList());
                } else
                {
                    fadePanel.GetComponent<Image>().DOFade(1, 1f).OnComplete(() =>
                    {
                        SceneManager.LoadScene(buildIndex);

                    });
                }

            }
            else
            {
                // Mostrar todo el string
                showAllText = true;
                // Detener la corutina de mostrar el texto
                StopAllCoroutines();
                // Mostrar el cursor al final del string letra 
                textComponent.text = displayTextList[currentIndex] + cursor;
                currentString = displayTextList[currentIndex];
            }
        }
    }

    IEnumerator AnimateTextList()
    {
        textComponent.text = "";
        currentString = displayTextList[currentIndex];

        foreach (char c in currentString)
        {
            // si no se ha dado clic, se muestra progresivamente el texto añadiendo uno por uno y eliminando el cursor cada vez
            if (!showAllText)
            {
                textComponent.text = currentString.Substring(0, textComponent.text.Length) + cursor;
                yield return new WaitForSeconds(characterDelay);
                textComponent.text = currentString.Substring(0, textComponent.text.Length - 1); // Se elilmina el cursor.
                textComponent.text += c; // se agrega la siguiente letra
                yield return new WaitForSeconds(characterDelay);
            }
        }
        // Cuando termine o se de clic se muestra todo el string
        showAllText = true;
    }
}