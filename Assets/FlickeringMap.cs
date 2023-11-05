using System.Collections;
using UnityEngine;

public class FlickeringMap : MonoBehaviour
{
    public Light flickeringLight;
    public float minIntensity = 0.25f;
    public float maxIntensity = 0.75f;
    public float flickerSpeed = 10f; // Ajusta este valor para controlar la velocidad de parpadeo
    public float delayTime = 2f; // Introducido por el inspector, retraso en segundos antes de que inicie el parpadeo

    private float targetIntensity;

    void Start()
    {
        if (flickeringLight == null)
        {
            flickeringLight = GetComponent<Light>();
        }

        StartCoroutine(StartFlickerAfterDelay());
    }

    IEnumerator StartFlickerAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            while (Mathf.Abs(flickeringLight.intensity - targetIntensity) > 0.05f)
            {
                flickeringLight.intensity = Mathf.Lerp(flickeringLight.intensity, targetIntensity, Time.deltaTime * flickerSpeed);
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}