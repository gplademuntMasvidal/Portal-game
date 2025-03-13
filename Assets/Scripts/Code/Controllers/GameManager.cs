using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public interface IRestartGameElement
{
    void RestartGame();
}

public class GameManager : MonoBehaviour
{
    static GameManager m_GameManager;
    PlayerController m_PlayerController;
    [SerializeField] private PauseAnimation m_PauseAnimation;
    List<IRestartGameElement> m_RestartGameElements = new List<IRestartGameElement>();
    public GameObject m_StartMenuPanel;
    public GameObject m_PauseMenuPanel;
    public GameObject m_HudPanel;
    public GameObject m_YouAreDeadPanel;
    public bool m_IsPlaying;


    private void Awake()
    {
        if (m_GameManager == null)
        {
            m_GameManager = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }

    static public GameManager GetGameManager()
    {
        return m_GameManager;
    }

    public PlayerController GetPlayer()
    {
        return m_PlayerController;
    }

    public void SetPlayer(PlayerController Player)
    {
        m_PlayerController = Player;
        DontDestroyOnLoad(m_PlayerController.gameObject);
    }

    /*public void SetMenu(PauseAnimation PauseAnimation)
    {
        m_PauseAnimation = PauseAnimation;
    }*/

    public void StartGame()
    {
        m_IsPlaying = true;
        m_YouAreDeadPanel.SetActive(false);
        m_StartMenuPanel.SetActive(false);
        m_HudPanel.SetActive(true);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_PlayerController.RestartGame();
    }
    public void RestartGame()
    {
        m_IsPlaying = true;
        m_YouAreDeadPanel.SetActive(false);
        m_StartMenuPanel.SetActive(false);
        //m_PauseAnimation.PlayClosingPausePanelAnimation();
        m_HudPanel.SetActive(true);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_PlayerController.RestartGame();
    }
    public void OpenStartMenu()
    {
        m_IsPlaying = false;
        m_YouAreDeadPanel.SetActive(false);
        m_PauseMenuPanel.SetActive(false);
        m_HudPanel.SetActive(false);
        m_StartMenuPanel.SetActive(true);
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void PauseGame()
    {
        Debug.Log("PauseGame called");

        m_IsPlaying = false;
        Time.timeScale = 0f;
        m_YouAreDeadPanel.SetActive(false);
        m_StartMenuPanel.SetActive(false);
        m_HudPanel.SetActive(false);
        m_PauseMenuPanel.SetActive(true);

        //StartCoroutine(HandlePauseMenuOpening());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator HandlePauseMenuOpening()
    {

        if (m_PauseMenuPanel == null)
        {
            Debug.LogError("m_PauseMenuPanel is null! Check if it is being destroyed.");
            yield break;
        }
        Debug.Log("Opening Pause Menu Panel");
        m_PauseMenuPanel.SetActive(true); // Asegúrate de que el panel esté activo
        Debug.Log("Animation started!");
        // Reproduce la animación
        // yield return StartCoroutine(m_PauseAnimation.PlayOpeningPausePanelAnimationAndWait());
        yield return new WaitForSeconds(1.0f); // Simula la duración de la animación
        Debug.Log("Animation finished!");
        //Debug.Log("Pause Menu Panel Active After Animation: " + m_PauseMenuPanel.activeSelf);
    }
    public void ResumeGame()
    {
        m_IsPlaying = true;
        m_YouAreDeadPanel.SetActive(false);
        m_StartMenuPanel.SetActive(false);

        m_PauseMenuPanel.SetActive(false); // Ocultar el menú de pausa

        //StartCoroutine(HandlePauseMenuClosing());

        Time.timeScale = 1f; // Reanudar el juego
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private IEnumerator HandlePauseMenuClosing()
    {
        // Reproduce la animación de cierre y espera a que termine
        yield return StartCoroutine(m_PauseAnimation.PlayClosingPausePanelAnimationAndWait());

        // Una vez que termina la animación, oculta el menú y muestra el HUD
        m_PauseMenuPanel.SetActive(false); // Ocultar el menú de pausa
        m_HudPanel.SetActive(true);        // Mostrar el HUD
    }

    public void YouAreDead()
    {
        m_IsPlaying = false;
        m_YouAreDeadPanel.SetActive(true);
        m_PauseMenuPanel.SetActive(false);
        m_HudPanel.SetActive(false);
        m_StartMenuPanel.SetActive(false);
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GoToTheLastCheckpoint()
    {
        m_IsPlaying = true;
        m_YouAreDeadPanel.SetActive(false);
        m_StartMenuPanel.SetActive(false);
        m_PauseMenuPanel.SetActive(false); // Ocultar el menú de pausa
        m_HudPanel.SetActive(true);
        Time.timeScale = 1f; // Reanudar el juego
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.GetGameManager().GetPlayer().Respawn();
    }

    public void QuitGame()
    {
        // Salir del juego
        Application.Quit();
        Debug.Log("Game Quit");  // Para cuando estés en el editor
    }

    // Llamar al menú de inicio cuando se cargue el juego
    void Start()
    {
        OpenStartMenu();  // Mostrar el menú de inicio cuando se carga el juego
    }







}