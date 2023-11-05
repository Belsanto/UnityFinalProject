using System.Collections;
using UnityEngine;

public class JumpScare : MonoBehaviour
{

    [SerializeField] private GameObject jumpScareObject;
    [SerializeField] private AudioSource sound;

    private AudioSource scareSound;
    public float fadeDuration = 1f;
    public float inactiveDuration = 1f;
    public float jumpScareDelay = 5f; // Agrega un retraso entre jump scares

    private bool canTriggerJumpScare = true;

    private void Start()
    {
        scareSound = jumpScareObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canTriggerJumpScare)
        {
            StartCoroutine(TriggerJumpScare());
        }
    }

    IEnumerator TriggerJumpScare()
    {
        canTriggerJumpScare = false;

        jumpScareObject.SetActive(true);
        jumpScareObject.GetComponent<Animation>().Play();
        scareSound.Play();
        sound.Play();
        StartFadeOut();

        yield return new WaitForSeconds(inactiveDuration);

        jumpScareObject.SetActive(false);

        yield return new WaitForSeconds(jumpScareDelay);

        canTriggerJumpScare = true;
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