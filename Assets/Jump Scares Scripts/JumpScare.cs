using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScare : MonoBehaviour
{

    [SerializeField] private GameObject jumpScareObject;

    private AudioSource scareSound;
    public float fadeDuration = 1f;
    public float inactiveDurantion = 1f;
    private void Start()
    {
        scareSound = jumpScareObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            jumpScareObject.SetActive(true);
            jumpScareObject.GetComponent<Animation>().Play();
            scareSound.Play();

            StartFadeOut();
            Invoke("SetInactive", inactiveDurantion);
        }
    }


    private void SetInactive()
    {
        jumpScareObject.SetActive(false);
    }


    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float startVolume = scareSound.volume;
        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            float elapsed = Time.time - startTime;
            float newVolume = Mathf.Lerp(startVolume, 0, elapsed / fadeDuration);
            scareSound.volume = newVolume;
            yield return null;
        }

        scareSound.Stop();
        scareSound.volume = 1f;
    }


}
