using System;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private Camera miniMapCamera;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject inv;
    
    private void Start()
    {
        miniMapCamera.enabled = true;
        miniMapCamera.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            miniMapCamera.enabled = !miniMapCamera.enabled;
        }
    }

    private void OnEnable()
    {
        hud.SetActive(false);
        inv.SetActive(false);
    }

    private void OnDisable()
    {
        hud.SetActive(true);
    }
}