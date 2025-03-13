using UnityEngine;
using UnityEngine.SceneManagement; // Para recargar la escena
using UnityEngine.UI; // Para manejar el mensaje de victoria

public class WinnerButton : MonoBehaviour
{
    [SerializeField] private GameObject winMessageUI; // Mensaje de "YOU WIN" en pantalla
    [SerializeField] private float restartDelay = 5.0f; // Tiempo antes de reiniciar el juego

    private bool isActivated = false; // Evita múltiples activaciones

    private void OnCollisionEnter(Collision collision)
    {
        if (isActivated) return; // Si ya está activado, no hace nada

        if (collision.collider.CompareTag("CompanionCube")) // Detecta el cubo
        {
            Debug.Log("¡Has ganado la partida!");
            HandleWin();
        }
    }

    private void HandleWin()
    {
        isActivated = true;

        // Mostrar el mensaje de victoria
        if (winMessageUI != null)
        {
            winMessageUI.SetActive(true);
        }

        // Pausar el tiempo del juego
        Time.timeScale = 0;

        // Reiniciar el juego después de un tiempo
        Invoke(nameof(RestartGame), restartDelay);
    }

    private void RestartGame()
    {
        // Restablecer el tiempo de juego
        Time.timeScale = 1;

        // Reiniciar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}