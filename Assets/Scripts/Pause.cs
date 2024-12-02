using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Canvas pauseMenuCanvas; // Ссылка на Canvas
    private bool isPaused = false;

    void Start()
    {
        pauseMenuCanvas.enabled = false; // Отключаем Canvas в начале
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void ResumeGame()
    {
        pauseMenuCanvas.enabled = false; // Отключаем Canvas
        Time.timeScale = 1f;            // Возвращаем время
        isPaused = false;               // Меняем флаг
    }

    private void PauseGame()
    {
        pauseMenuCanvas.enabled = true; // Включаем Canvas
        Time.timeScale = 0f;            // Ставим время на паузу
        isPaused = true;                // Меняем флаг
    }
}
