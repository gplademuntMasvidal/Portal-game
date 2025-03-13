using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip walkSound; // Clip para el sonido de andar

    [Header("Settings")]
    [SerializeField] private LayerMask groundLayer; // Capas consideradas como suelo
    private AudioSource audioSource;
    private CharacterController characterController; // Para verificar el movimiento del jugador

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        audioSource.loop = true; // Configura el AudioSource para que el sonido sea continuo
    }

    private void Update()
    {
        HandleWalkingSound();
    }

    private void HandleWalkingSound()
    {
        // Verifica si el jugador está tocando el suelo y moviéndose
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            if (!audioSource.isPlaying) // Si no se está reproduciendo, comienza el sonido
            {
                audioSource.clip = walkSound;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying) // Si no se mueve o está en el aire, detén el sonido
            {
                audioSource.Stop();
            }
        }
    }
}