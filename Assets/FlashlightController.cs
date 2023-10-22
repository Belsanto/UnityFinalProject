using System;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [SerializeField] private float maxDuration = 50f;
    [SerializeField] private float minDuration = 10f;
    [SerializeField] private float minIntensity = 2f;
    [SerializeField] private float maxIntensity = 3f;
    [SerializeField] private float minSpotAngle = 50f;
    [SerializeField] private float maxSpotAngle = 80f;

    private Light flashlight;
    private float currentDuration;
    private float currentIntensity;
    private bool isOn;
    //private float originalSpotAngle;
    
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
        if (isOn == false && currentDuration > minDuration)
        {
            isOn = !isOn;
            SetLight();
        }else
        if (isOn)
        {
            isOn = false;
            SetLight();
        }
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
            else if (currentDuration <= minDuration)
            {
                // Flickering effect
                float randomIntensity = UnityEngine.Random.Range(minIntensity, maxIntensity);
                float randomSpotAngle = Mathf.Lerp(minSpotAngle, maxSpotAngle, currentDuration / minDuration);
                flashlight.intensity = randomIntensity;
                flashlight.spotAngle = randomSpotAngle;
            }
        }
        else
        {
            if(currentDuration<= maxDuration)
                currentDuration += Time.deltaTime;
        }
    }

    private void SetLight()
    {
        if (isOn)
        {
            flashlight.intensity = maxIntensity;
            flashlight.spotAngle = maxSpotAngle;
            flashlight.enabled = true;
        }
        else
        {
            flashlight.enabled = false;
        }
    }
}