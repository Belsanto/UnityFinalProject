using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ReadNotes : MonoBehaviour
{
    private enum ItemOptions { Red, Blue, Gold, Note }

    [SerializeField] private ItemOptions selectedItem = ItemOptions.Red; // Valor azul predeterminado

    [SerializeField] private GameObject noteUI;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject hudCard;
    [SerializeField] private GameObject interact;
    [SerializeField] private MenuController menuController;
    [SerializeField] private bool isWin;
    [SerializeField] private bool isNote;

    [SerializeField] private GameObject pickUpSound;
    [SerializeField] private GameObject dropSound;
    [SerializeField] private AudioSource pickUpCardSound;
    [SerializeField] private AudioSource dropCardSound;

    [SerializeField] private GameObject playableDirector;

    private Renderer render;

    private float maxRange = 1f;
    private bool inReach;
    private bool reading;
    private bool isInteracting;
    private bool cardCollected;
    private PlayerMovement playerMovement;

    void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        if (!isWin)
        {
            noteUI.SetActive(false);
        }



        hud.SetActive(true);
        interact.SetActive(false);
        reading = false;
        inReach = false;
        isInteracting = false;
        playerMovement = FindObjectOfType<PlayerMovement>();
        menuController = FindObjectOfType<MenuController>();
    }

    private void OnMouseEnter()
    {
        CheckChildObjectAndSetActiveRenderer();
    }

    private void CheckChildObjectAndSetActiveRenderer()
    {
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    Transform activeInteractable = child;
                    render = activeInteractable.GetComponent<Renderer>();
                }
            }
        }
    }

    private void OnMouseOver()
    {
        CheckMouseHover();
    }

    private void CheckMouseHover()
    {
        if (!menuController.isPaused)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxRange) && !isInteracting && !reading && cardCollected !=true)
            {
                inReach = true;
                interact.SetActive(true);
                if (gameObject.CompareTag("Cards") || gameObject.CompareTag("Book"))
                {
                    render.materials[1].SetInt("_Show_Outline", 1);
                }
            }
        }
    }

    private void OnMouseExit()
    {
        HandleMouseExit();
    }

    private void HandleMouseExit()
    {
        if (!reading)
        {
            inReach = false;
            interact.SetActive(false);
        }

        if (gameObject.CompareTag("Cards") || gameObject.CompareTag("Book"))
        {
            render.materials[1].SetInt("_Show_Outline", 0);
        }
    }

    void Update()
    {
        CheckInputAndUpdateState();
    }

    private void CheckInputAndUpdateState()
    {
        if (Input.GetKeyDown(KeyCode.E) && inReach && !reading && cardCollected !=true)
        {
            StartInteraction();
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.timeScale == 0 && reading && !isWin)
        {
            EndInteraction();
        }
    }

    private void StartInteraction()
    {
        isInteracting = true;
        interact.SetActive(false);

        if (!isWin)
        {
            playerMovement.SetIsAbleToLook(false);
            noteUI.SetActive(true);
            StartCoroutine(CloseOpenNote(0.1f));

        }
        else
        {
            playableDirector.SetActive(true);
            Time.timeScale = 1;
            //noteUI.GetComponent<MenuController>().SetWinScreen();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }

        HandlePickUpSounds();
        hud.SetActive(false);
        //StartCoroutine(CloseOpenNote(0.1f));

        if (selectedItem != ItemOptions.Note)
        {
            GameManager.Instance.AcquireItem(selectedItem.ToString());
        }
    }

    public void ActivateWinScreen()
    {
        noteUI.GetComponent<MenuController>().SetWinScreen();
        playableDirector.SetActive(false);
        Time.timeScale = 0;
    }

    private void HandlePickUpSounds()
    {
        if (isNote)
        {
            dropSound.SetActive(false);
            pickUpSound.SetActive(true);
        }
        else if(isWin == false)
        {
            pickUpCardSound.Play();
        }
    }

    private void EndInteraction()
    {
        isInteracting = false;
        reading = false;
        playerMovement.SetIsAbleToLook(true);
        noteUI.SetActive(false);

        HandleDropSounds();
        hud.SetActive(true);

        if (!isNote)
        {
            hudCard.SetActive(true);
        }

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (isNote == false && isWin == false)
        {
            DestroyGameObjectWithAnimation();
        }
    }

    private void HandleDropSounds()
    {
        if (isNote)
        {
            pickUpSound.SetActive(false);
            dropSound.SetActive(true);
        }
        else
        {
            dropCardSound.Play();
        }
    }

    private void DestroyGameObjectWithAnimation()
    {
        cardCollected = true;
        StartCoroutine(DestroyWithAnimation());
    }

    private IEnumerator DestroyWithAnimation()
    {
        float duration = 2f;
        float currentTime = 0f;
        Vector3 initialScale = gameObject.transform.localScale;
        Vector3 targetScale = Vector3.zero;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float normalizedTime = currentTime / duration;
            gameObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, normalizedTime);
            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator CloseOpenNote(float time)
    {
        yield return new WaitForSeconds(time);
        reading = !reading;
        Time.timeScale = reading ? 0f : 1f;
        if (isWin)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
