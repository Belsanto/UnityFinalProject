using System;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private Camera miniMapCamera;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject zones;
    
    private void Start()
    {
        zones.SetActive(false);
        miniMapCamera.enabled = true;
        miniMapCamera.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            var enabled1 = miniMapCamera.enabled;
            enabled1 = !enabled1;
            miniMapCamera.enabled = enabled1;
            hud.SetActive(!enabled1);
            zones.SetActive(enabled1);
            
        }
    }
}