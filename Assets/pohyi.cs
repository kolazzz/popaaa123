using TMPro;
using UnityEngine;

public class BlockCharacterControl : MonoBehaviour
{
    public GameObject player;           // Ссылка на объект персонажа
    private TopDownCharacterController playerController;  // Ссылка на компонент управления персонажем
    public TextMeshProUGUI[] blockingTexts;  // Массив ссылок на текстовые компоненты, которые блокируют управление

    void Start()
    {
        // Получаем компонент PlayerController (предположим, что он существует)
        playerController = player.GetComponent<TopDownCharacterController>();
    }

    void Update()
    {
        // Проверяем, есть ли на сцене активные блокирующие тексты
        bool isBlockingTextActive = false;

        foreach (TextMeshProUGUI text in blockingTexts)
        {
            if (text != null && text.gameObject.activeInHierarchy && !string.IsNullOrEmpty(text.text))
            {
                isBlockingTextActive = true;
                break;
            }
        }

        // Если текст активен, блокируем управление
        if (isBlockingTextActive)
        {
            playerController.enabled = false; // Отключаем управление персонажем
        }
        else
        {
            playerController.enabled = true; // Включаем управление, если текст не активен
        }
    }
}
