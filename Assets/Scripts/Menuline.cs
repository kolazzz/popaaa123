using UnityEngine;
using TMPro; // Для TextMeshPro
using UnityEngine.UI; // Для Image компонентов

public class Menuline : MonoBehaviour
{
    public GameObject[] menuRows; // Каждый "ряд" - это родительский объект с Image и TextMeshProUGUI
    private int selectedIndex = 0; // Текущий индекс строки

    private Color32 selectedTextColor = new Color32(255, 177, 0, 255); // Жёлтый текст (#FFB100)
    private Color32 defaultTextColor = new Color32(0, 0, 0, 255);      // Чёрный текст
    private Color32 selectedBackgroundColor = new Color32(0, 0, 0, 255); // Чёрный фон
    private Color32 transparentBackground = new Color32(0, 0, 0, 0);     // Прозрачный фон

    void Start()
    {
        UpdateMenu();
    }

    void Update()
    {
        // Навигация по W/S или стрелкам
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 1 + menuRows.Length) % menuRows.Length;
            UpdateMenu();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 1) % menuRows.Length;
            UpdateMenu();
        }
    }

    void UpdateMenu()
    {
        for (int i = 0; i < menuRows.Length; i++)
        {
            // Получаем компоненты из каждого ряда
            Image background = menuRows[i].GetComponent<Image>();
            TextMeshProUGUI text = menuRows[i].GetComponentInChildren<TextMeshProUGUI>();

            if (i == selectedIndex)
            {
                // Выделенная строка: чёрный фон и жёлтый текст
                background.color = selectedBackgroundColor;
                text.color = selectedTextColor;
            }
            else
            {
                // Невыделенная строка: прозрачный фон и чёрный текст
                background.color = transparentBackground;
                text.color = defaultTextColor;
            }
        }
    }
}
