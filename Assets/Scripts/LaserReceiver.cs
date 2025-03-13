using UnityEngine;

public class LaserReceiver : MonoBehaviour
{
    // La puerta espec�fica que este bot�n controlar�
    [SerializeField] public DoorController linkedDoor;
    public bool isActivated = false; // Previene activaciones m�ltiples

    private void OnCollisionEnter(Collision collision)
    {
        if (isActivated) return; // Si ya est� activado, no hace nada

        if (collision.collider.CompareTag("Laser")) // Detecta el cubo
        {
            Debug.Log($"Bot�n activado por {collision.collider.name}.");

            if (linkedDoor != null) // Verifica que haya una puerta asignada
            {
                linkedDoor.OpenDoor(); // Abre la puerta asignada
                isActivated = true; // Marca como activado
            }
            else
            {
                Debug.LogWarning("No se ha asignado una puerta al bot�n.");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Opcional: l�gica para cerrar la puerta al dejar de estar en contacto
        if (collision.collider.CompareTag("Laser"))
        {
            if (linkedDoor != null)
            {
                linkedDoor.CloseDoor(); // Cierra la puerta al salir de la colisi�n
                isActivated = false; // Permite reactivar el bot�n
            }
        }
    }
}