using System;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [SerializeField] private float maxDuration = 50f;
    [SerializeField] private float minDuration = 10f;
    [SerializeField] private float minRange = 50f;
    [SerializeField] private float maxRange = 80f;
    [SerializeField] private float minIntensity = 2f;
    [SerializeField] private float maxIntensity = 3f;

    private Light flashlight;
    private float currentDuration;
    private bool isOn;

    private void Start()
    {
        flashlight = GetComponent<Light>();
        currentDuration = maxDuration;
        SetLight();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        UpdateDuration();
    }

    private void ToggleFlashlight()
    {
        isOn = !isOn;
        if (isOn)
        {
            currentDuration = maxDuration;
        }

        SetLight();
    }

    private void UpdateDuration()
    {
        if (isOn)
        {
            currentDuration -= Time.deltaTime;

            if (currentDuration <= 0)
            {
                currentDuration = 0;
                isOn = false;
                SetLight();
            }
            else if (currentDuration <= 5)
            {
                // Flickering effect
                float randomIntensity = UnityEngine.Random.Range(minIntensity, maxIntensity);
                flashlight.intensity = randomIntensity;
            }
        }
    }

    private void SetLight()
    {
        if (isOn)
        {
            flashlight.range = Mathf.Lerp(minRange, maxRange, 1 - currentDuration / maxDuration);
            flashlight.intensity = Mathf.Lerp(minIntensity, maxIntensity, 1 - currentDuration / maxDuration);
            flashlight.enabled = true;
        }
        else
        {
            flashlight.enabled = false;
        }
    }
}