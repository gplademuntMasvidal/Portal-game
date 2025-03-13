using UnityEngine;

public class Button : MonoBehaviour
{
    // La puerta específica que este botón controlará
    [SerializeField] private DoorController linkedDoor;
    private bool isActivated = false; // Previene activaciones múltiples

    private void OnCollisionEnter(Collision collision)
    {
        if (isActivated) return; // Si ya está activado, no hace nada

        if (collision.collider.CompareTag("CompanionCube")) // Detecta el cubo
        {
            Debug.Log($"Botón activado por {collision.collider.name}.");

            if (linkedDoor != null) // Verifica que haya una puerta asignada
            {
                linkedDoor.OpenDoor(); // Abre la puerta asignada
                isActivated = true; // Marca como activado
            }
            else
            {
                Debug.LogWarning("No se ha asignado una puerta al botón.");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Opcional: lógica para cerrar la puerta al dejar de estar en contacto
        if (collision.collider.CompareTag("CompanionCube"))
        {
            if (linkedDoor != null)
            {
                linkedDoor.CloseDoor(); // Cierra la puerta al salir de la colisión
                isActivated = false; // Permite reactivar el botón
            }
        }
    }
}