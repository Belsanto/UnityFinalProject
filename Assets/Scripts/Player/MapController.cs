using System;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private Camera miniMapCamera;

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
}