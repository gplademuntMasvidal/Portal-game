using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectSoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip fallSound;

    private AudioSource audioSource;
    private bool hasLanded = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasLanded)
        {
            audioSource.PlayOneShot(fallSound);
            hasLanded = true; // Previene múltiples sonidos de caída
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        hasLanded = false; // Permite que se reproduzca el sonido de nuevo
    }
}