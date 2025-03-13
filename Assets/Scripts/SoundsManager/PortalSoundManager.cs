using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PortalSoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip enterPortalSound; // Sonido al entrar al portal

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Detecta al jugador
        {
            PlayEnterPortalSound();
        }
    }

    public void PlayEnterPortalSound()
    {
        if (enterPortalSound != null)
        {
            audioSource.PlayOneShot(enterPortalSound); // Reproduce el sonido al entrar al portal
        }
        else
        {
            Debug.LogWarning("El sonido de entrada al portal no está asignado.");
        }
    }
}