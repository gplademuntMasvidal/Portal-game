using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AttachSoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip attachSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAttachSound()
    {
        audioSource.PlayOneShot(attachSound);
    }
}