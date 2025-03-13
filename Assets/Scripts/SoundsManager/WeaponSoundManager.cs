using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponSoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip shootSound; // Sonido al disparar

    private AudioSource audioSource;
    PlayerController playerController;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
    }

    public void PlayShootSound()
    {
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound); // Reproduce el sonido de disparo
            Debug.Log("HO FA!!!!!!!!!!!");
        }
        else
        {
            Debug.LogWarning("El sonido de disparo no está asignado.");
        }
    }
    public void Update()
    {
        if (!playerController.m_AttachedObject && !playerController.m_AttachingObject)
        {

            if (Input.GetMouseButtonUp(0))
            {
                PlayShootSound();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                PlayShootSound();
                
            }
        }
    }
}