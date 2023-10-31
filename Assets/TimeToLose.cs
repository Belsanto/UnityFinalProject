using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeToLose : MonoBehaviour
{
    [SerializeField] [Range(1, 8)] 
    private float initialTime = 8.0f; // Tiempo inicial en minutos (ajusta el valor en el Inspector)
    private TextMeshProUGUI textMeshPro;
    
    [SerializeField] 
    private GameObject endGameMenu; 
    public float currentTime { get; private set; }
    private PlayerMovement playerMovement;

    void Start()
    {
        currentTime = initialTime * 60; // Convierte minutos a segundos
        textMeshPro = GetComponent<TextMeshProUGUI>();
        playerMovement = FindObjectOfType<PlayerMovement>(); // referencia al script ImprovedPlayerMovement
        UpdateTimeText();
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimeText();
        }
        else
        {
            // El tiempo ha terminado, imprime un mensaje
            Debug.Log("¡Perdiste!");
            endGameMenu.GetComponent<MenuController>().SetLoseScreen();
            playerMovement.SetIsAbleToLook(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
    }

    void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        string timeText = string.Format("ESCAPE! : {0:00}:{1:00}", minutes, seconds);
        textMeshPro.text = timeText;
    }
}