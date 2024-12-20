using TMPro;
using UnityEngine;

public class DisableTextsOnClick : MonoBehaviour
{
    public TextMeshProUGUI[] textsToDisable;  // Массив текстов, которые будут выключаться

    void Update()
    {
        // Проверяем, была ли нажата левая кнопка мыши
        if (Input.GetMouseButtonDown(0))  // 0 соответствует левой кнопке мыши
        {
            DisableTexts();  // Отключаем все тексты
        }
    }

    // Метод для деактивации всех текстов в массиве
    void DisableTexts()
    {
        foreach (TextMeshProUGUI text in textsToDisable)
        {
            if (text != null)
            {
                text.gameObject.SetActive(false);  // Деактивируем текст
            }
        }
    }
}