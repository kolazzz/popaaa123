using UnityEngine;
using TMPro;
using System.Collections;

public class PauseOnTrigger : MonoBehaviour
{
    [Header("Объект для отображения")]
    [SerializeField] private GameObject dialogBox; // Окно диалога
    [SerializeField] private TextMeshProUGUI dialogText; // Поле текста для вывода основного текста
    [SerializeField] private TextMeshProUGUI secondaryText; // Ссылка на текст для дополнительного текста
    [SerializeField] private string dialogMessage = "Это пример текста диалога."; // Основной текст диалога
    [SerializeField] private string secondaryMessage = "Это дополнительный текст."; // Дополнительный текст
    [SerializeField] private float typingSpeed = 0.05f; // Скорость печатания (в секундах между буквами)
    [SerializeField] private KeyCode[] continueKeys; // Массив клавиш для продолжения

    private bool isPaused = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private Collider2D triggerCollider; // Коллайдер триггер-зоны

    private void Start()
    {
        // Скрываем окно диалога и очищаем текст при старте
        if (dialogBox != null)
        {
            dialogBox.SetActive(false);
        }

        if (dialogText != null)
        {
            dialogText.text = ""; // Очищаем основной текст
        }

        if (secondaryText != null)
        {
            secondaryText.text = ""; // Очищаем дополнительный текст
        }

        // Получаем ссылку на коллайдер триггер-зоны
        triggerCollider = GetComponent<Collider2D>();
        if (triggerCollider == null)
        {
            Debug.LogError("Коллайдер для триггер-зоны не найден!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPaused)
        {
            Debug.Log("Игрок вошел в триггер-зону.");
            PauseGame();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Игрок вышел из триггер-зоны.");
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false; // Отключаем триггер
                Debug.Log("Триггер отключен.");
            }
        }
    }

    private void Update()
    {
        if (isPaused && !isTyping && AnyContinueKeyPressed()) // Проверка нажатия любой клавиши из массива
        {
            Debug.Log("Игрок нажал клавишу для продолжения.");
            ResumeGame();
        }
    }

    private bool AnyContinueKeyPressed()
    {
        foreach (KeyCode key in continueKeys)
        {
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }
        return false;
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (dialogBox != null)
        {
            dialogBox.SetActive(true); // Показываем окно диалога
        }

        if (dialogText != null)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // Останавливаем предыдущую корутину, если есть
            }
            typingCoroutine = StartCoroutine(TypeText(dialogMessage)); // Начинаем печатать текст
        }
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (dialogBox != null)
        {
            dialogBox.SetActive(false); // Скрываем окно диалога
        }

        if (dialogText != null)
        {
            dialogText.text = ""; // Очищаем основной текст
        }

        if (secondaryText != null)
        {
            secondaryText.text = ""; // Очищаем дополнительный текст
        }
    }

    private IEnumerator TypeText(string message)
    {
        isTyping = true;
        dialogText.text = ""; // Очищаем текст перед началом анимации
        int index = 0;
        string cursor = "<color=#FFFFFF>|</color>"; // Курсор для анимации

        while (index <= message.Length)
        {
            dialogText.text = message.Substring(0, index) + cursor; // Добавляем текст по одному символу
            index++;
            yield return new WaitForSecondsRealtime(typingSpeed); // Используем WaitForSecondsRealtime для работы в паузе
        }

        dialogText.text = message; // Полный текст без курсора
        isTyping = false;

        // Показать дополнительный текст
        if (secondaryText != null)
        {
            secondaryText.text = secondaryMessage; // Устанавливаем дополнительный текст
            Debug.Log("Показан дополнительный текст.");
        }

        // Мигающий курсор
        StartCoroutine(BlinkCursor());
    }

    private IEnumerator BlinkCursor()
    {
        string baseText = dialogText.text;

        while (isPaused) // Мигаем курсором, пока диалог открыт
        {
            dialogText.text = baseText + "<color=#FFFFFF>|</color>";
            yield return new WaitForSecondsRealtime(0.5f); // Используем WaitForSecondsRealtime для мигания
            dialogText.text = baseText;
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
