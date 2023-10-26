using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using LeanTween;

public class ReadNotes : MonoBehaviour
{
    private enum ItemOptions { Red, Blue, Gold, Note }
    [SerializeField]
    private ItemOptions selectedItem = ItemOptions.Red; // Valor azul predeterminado

    [SerializeField] private GameObject noteUI;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pause;

    [SerializeField] private GameObject interact;

    [SerializeField] private AudioSource pickUpSound;
    [SerializeField] private AudioSource dropSound;

    private bool inReach;
    private bool reading;

    void Start()
    {
        noteUI.SetActive(false);
        hud.SetActive(true);
        pause.SetActive(false);
        interact.SetActive(false);
        reading = false;
        inReach = false;
    }

    void OnMouseEnter()
    {
        if (reading == false)
        {
            inReach = true;
            interact.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (reading == false)
        {
            inReach = false;
            interact.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inReach && reading == false)
        {
            interact.SetActive(false);
            noteUI.SetActive(true);
            pickUpSound.Play();
            hud.SetActive(false);
            pause.SetActive(false);
            StartCoroutine(CloseOpenNote(0.1f));
            if (!(selectedItem == ItemOptions.Note))
            {
                GameManager gameManager = GameManager.Instance;
                gameManager.AcquireItem(selectedItem.ToString());
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && inReach && Time.timeScale == 0 && reading)
        {
            reading = false;
            noteUI.SetActive(false);
            dropSound.Play();
            hud.SetActive(true);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor en el centro de la pantalla
            Cursor.visible = false; // Hacer el cursor pauseisible
            // Encoger y destruir
            /* LeanTween.scale(gameObject, Vector3.zero, 0.5f).setOnComplete(() => {
                Destroy(gameObject);
            }); */
            if (!(selectedItem == ItemOptions.Note))
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator CloseOpenNote(float time)
    {
        yield return new WaitForSeconds(time);
        reading = !reading;
        Time.timeScale = reading ? 0f : 1f;
        Cursor.lockState = reading ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
