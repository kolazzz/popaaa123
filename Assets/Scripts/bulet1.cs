using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectCounter : MonoBehaviour
{
    [Header("Object Layer Settings")]
    [SerializeField] private LayerMask targetLayer;  // Слой, объекты которого нужно отслеживать

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI counterText; // Ссылка на TextMeshPro для отображения счетчика

    private int objectCount;  // Счетчик объектов с выбранным слоем

    void Start()
    {
        // Инициализация счетчика
        objectCount = GetObjectsCount();
        UpdateCounterText();

        // Регистрация для отслеживания изменений объектов
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDestroy()
    {
        // Очистка подписки, чтобы избежать утечек памяти
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void Update()
    {
        // Каждый кадр обновляем количество объектов
        int newCount = GetObjectsCount();
        if (newCount != objectCount)
        {
            objectCount = newCount;
            UpdateCounterText();
        }
    }

    // Метод для получения количества объектов с нужным слоем в сцене
    private int GetObjectsCount()
    {
        // Найти все объекты с нужным слоем
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy"); // Для поиска объектов с конкретным тегом

        int count = 0;
        foreach (GameObject obj in objects)
        {
            if (((1 << obj.layer) & targetLayer) != 0)
            {
                count++;
            }
        }
        return count;
    }

    // Метод для обновления текста
    private void UpdateCounterText()
    {
        counterText.text = objectCount.ToString();  // Отображение количества объектов
    }

    private void OnSceneUnloaded(Scene currentScene)
    {
        // Обновить счетчик объектов при выгрузке сцены (если объекты удаляются)
        objectCount = GetObjectsCount();
        UpdateCounterText();
    }
}