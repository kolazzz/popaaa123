using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public GameObject door; // Дверь, которую нужно открыть
    public GameObject[] enemies; // Враги в комнате
    private bool allEnemiesDestroyed = false; // Флаг для проверки, что все враги уничтожены

    // Метод для проверки, уничтожены ли все враги
    void Update()
    {
        CheckEnemiesStatus();
    }

    // Проверка статуса врагов
    void CheckEnemiesStatus()
    {
        allEnemiesDestroyed = true; // Предположим, что все враги уничтожены

        // Проверяем всех врагов
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null) // Если враг еще существует, ставим флаг в false
            {
                allEnemiesDestroyed = false;
                break;
            }
        }

        // Если все враги уничтожены, открываем дверь
        if (allEnemiesDestroyed)
        {
            OpenDoor();
        }
    }

    // Открыть дверь (например, деактивируем её или анимируем)
    void OpenDoor()
    {
        if (door != null)
        {
            door.SetActive(false); // Отключаем дверь (можно использовать анимацию для более сложного эффекта)
        }
    }
}
