using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animation m_Animation;
    public AnimationClip m_OpeningDoorAnimationClip;
    public AnimationClip m_ClosingDoorAnimationClip;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip openDoorSound; // Sonido al abrir la puerta
    [SerializeField] private AudioClip closeDoorSound; // Sonido al cerrar la puerta

    private AudioSource audioSource;

    public bool isDoorOpen = false; // Indica si la puerta está abierta

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OpenDoor()
    {
        if (isDoorOpen)
        {
            Debug.Log("La puerta ya está abierta. No se puede volver a abrir hasta que se cierre.");
            return; // Si la puerta ya está abierta, no hace nada
        }

        // Activa la animación de apertura
        m_Animation.CrossFade(m_OpeningDoorAnimationClip.name);

        // Reproduce el sonido de abrir puerta
        PlaySound(openDoorSound);

        isDoorOpen = true; // Marca la puerta como abierta
    }

    public void CloseDoor()
    {
        if (!isDoorOpen)
        {
            Debug.Log("La puerta ya está cerrada. No se puede cerrar nuevamente.");
            return; // Si la puerta ya está cerrada, no hace nada
        }

        // Activa la animación de cierre
        m_Animation.CrossFade(m_ClosingDoorAnimationClip.name);

        // Reproduce el sonido de cerrar puerta
        PlaySound(closeDoorSound);

        isDoorOpen = false; // Marca la puerta como cerrada
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Reproduce el sonido
        }
        else
        {
            Debug.LogWarning("Sonido o AudioSource no configurado en " + gameObject.name);
        }
    }
}