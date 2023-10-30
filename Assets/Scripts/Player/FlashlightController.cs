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
    [SerializeField] private AudioSource activeSound;
    [SerializeField] private GameObject lightOn;
    [SerializeField] private GameObject lightOff;
    [SerializeField] private GameObject lightDisable;
    
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
        lightOn.SetActive(false);
        lightDisable.SetActive(false);
        lightOff.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        if (isOn == false)
        {
            if (currentDuration <= minDuration)
            {
                lightOn.SetActive(false);
                lightDisable.SetActive(true);
                lightOff.SetActive(false);
            }
            else
            {
                lightOn.SetActive(false);
                lightDisable.SetActive(false);
                lightOff.SetActive(true);
            }
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
                currentDuration += Time.deltaTime*2;
        }
    }

    private void SetLight()
    {
        if (isOn)
        {
            lightOn.SetActive(true);
            lightDisable.SetActive(false);
            lightOff.SetActive(false);
            currentIntensity = Mathf.Lerp(minIntensity, maxIntensity, maxDuration / minDuration);
            flashlight.intensity = currentIntensity;
            flashlight.spotAngle = maxSpotAngle;
            flashlight.enabled = true;
        }
        else
        {
            flashlight.enabled = false;
        }
    }
}