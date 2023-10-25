using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpen : MonoBehaviour
{
    private enum GatesOptions { Red, Blue, Gold}
    
    [Header("UI")]
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject inv;
    [SerializeField] private GameObject pickUpText;

    
    [Header("Gate")]
    [SerializeField] private GameObject door;
    [SerializeField] private GatesOptions gateColor = GatesOptions.Red; // Valor rojo predeterminado

    [Header("Audio")]
    [SerializeField] private AudioSource openSound;
    [SerializeField] private AudioSource lockSound;
    [SerializeField] private AudioSource closeSound;

    private bool inReach;
    private bool doorOpen;
    
    private bool isMoving = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float movementSpeed = .5f;
    
    void Start()
    {
        hud.SetActive(true);
        inv.SetActive(false);
        pickUpText.SetActive(false);
        inReach = false;
        doorOpen = false;
    }

    void OnMouseEnter()
    {
        inReach = true;
        pickUpText.SetActive(true);
        
    }

    void OnMouseExit()
    {
        inReach = false;
        pickUpText.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inReach && doorOpen == false && !isMoving)
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager.HasItem(gateColor.ToString()))
            {
                openSound.Play(); // Reproducir sonido de abrir
                initialPosition = door.transform.position;
                targetPosition = initialPosition + new Vector3(0, 2.5f, 0);
                isMoving = true;
            }
            else
            {
                lockSound.Play(); // Reproducir sonido de bloqueado
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && inReach && doorOpen == true && !isMoving)
        {
            closeSound.Play(); // Reproducir sonido de cerrar
            initialPosition = door.transform.position;
            targetPosition = initialPosition - new Vector3(0, 2.5f, 0);
            isMoving = true;
        }

        if (isMoving)
        {
            float step = movementSpeed * Time.deltaTime;
            door.transform.position = Vector3.Lerp(door.transform.position, targetPosition, step);
            if (Vector3.Distance(door.transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                doorOpen = !doorOpen;
            }
        }
    }
}
