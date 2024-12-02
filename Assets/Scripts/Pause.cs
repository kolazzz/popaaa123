using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Canvas pauseMenuCanvas; // ������ �� Canvas
    private bool isPaused = false;

    void Start()
    {
        pauseMenuCanvas.enabled = false; // ��������� Canvas � ������
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
        pauseMenuCanvas.enabled = false; // ��������� Canvas
        Time.timeScale = 1f;            // ���������� �����
        isPaused = false;               // ������ ����
    }

    private void PauseGame()
    {
        pauseMenuCanvas.enabled = true; // �������� Canvas
        Time.timeScale = 0f;            // ������ ����� �� �����
        isPaused = true;                // ������ ����
    }
}
