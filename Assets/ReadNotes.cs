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
    [SerializeField] private GameObject hudCard;
    [SerializeField] private GameObject interact;
    [SerializeField] private bool isWin;
    [SerializeField] private bool isNote;

    [SerializeField] private AudioSource pickUpSound;
    [SerializeField] private AudioSource dropSound;

    private bool inReach;
    private bool reading;
    private PlayerMovement playerMovement;

    void Start()
    {
        if (isWin == false)
        {
            noteUI.SetActive(false);
        }
        hud.SetActive(true);
        interact.SetActive(false);
        reading = false;
        inReach = false;
        playerMovement = FindObjectOfType<PlayerMovement>(); // referencia al script ImprovedPlayerMovement
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
            if (isWin == false)
            {
                playerMovement.SetIsAbleToLook(false);
                noteUI.SetActive(true);
            }
            else
            {
                noteUI.GetComponent<MenuController>().SetWinScreen();
            }
            pickUpSound.Play();
            hud.SetActive(false);
            
            
            StartCoroutine(CloseOpenNote(0.1f));
            if (!(selectedItem == ItemOptions.Note))
            {
                GameManager gameManager = GameManager.Instance;
                gameManager.AcquireItem(selectedItem.ToString());
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && inReach && Time.timeScale == 0 && reading && isWin == false)
        {
            reading = false;
            playerMovement.SetIsAbleToLook(true);
            noteUI.SetActive(false);
            dropSound.Play();
            hud.SetActive(true);
            if (isNote == false)
            {
                hudCard.SetActive(true);
            }
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
        if (isWin)
        {
            // Make the cursor visible and unlock it
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
