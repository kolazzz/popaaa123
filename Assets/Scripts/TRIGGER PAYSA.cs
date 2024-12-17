using UnityEngine;

public class TRIGGERPAYSA : MonoBehaviour
{
    [Header("Объект для отображения")]
    [SerializeField] private GameObject pauseObject; // Объект, который будет отображаться

    private bool isPaused = false;

    private void Start()
    {
        if (pauseObject != null)
        {
            pauseObject.SetActive(false); // Скрываем объект при старте
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPaused) // Проверяем, что персонаж вошел
        {
            Debug.Log("Игрок вошел в триггер-зону."); // Дебаг входа
            PauseGame();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Проверяем, что игрок вышел из зоны
        {
            Debug.Log("Игрок вышел из триггер-зоны."); // Дебаг выхода
        }
    }

    private void Update()
    {
        if (isPaused && Input.GetMouseButtonDown(0)) // Возобновить игру по клику ЛКМ
        {
            Debug.Log("Игрок нажал ЛКМ для возобновления игры."); // Дебаг клика
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Останавливаем время
        if (pauseObject != null)
        {
            pauseObject.SetActive(true); // Показываем объект
        }
        Debug.Log("Игра поставлена на паузу."); // Дебаг паузы
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Возобновляем время
        if (pauseObject != null)
        {
            pauseObject.SetActive(false); // Скрываем объект
        }
        Debug.Log("Игра возобновлена."); // Дебаг возобновления
    }
}