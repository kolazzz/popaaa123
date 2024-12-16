using UnityEngine;
using TMPro;

public class ItemPickupManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI weaponText;

    [Header("Inspector Objects")]
    [SerializeField] private GameObject object1; // For Shotgun
    [SerializeField] private GameObject object2; // For Railgun
    [SerializeField] private GameObject object3; // For Katana

    private GameObject currentHighlight; // Текущий подсвеченный объект

    private void Start()
    {
        // Установить "Pistol" как дефолтное оружие при запуске
        SetUI("Pistol", null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что персонаж соприкасается с оружием
        switch (collision.tag)
        {
            case "Shotgun":
                SetUI("Shotgun", object1);
                
                break;

            case "Railgun":
                SetUI("Railgun", object2);
                
                break;

            case "Katana":
                SetUI("Katana", object3);
                
                break;
        }
    }

    private void SetUI(string weaponName, GameObject highlightObject)
    {
        // Установить текст оружия
        if (weaponText != null)
            weaponText.text = weaponName;

        // Сбросить подсветку
        ResetHighlights();

        // Подсветить новый объект
        if (highlightObject != null)
        {
            var spriteRenderer = highlightObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.red; // Подсветить красным
                currentHighlight = highlightObject; // Сохранить текущий подсвеченный объект
            }
        }
    }

    private void ResetHighlights()
    {
        // Сбросить подсветку предыдущего объекта
        if (currentHighlight != null)
        {
            var spriteRenderer = currentHighlight.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.color = Color.white; // Вернуть белый цвет
        }

        currentHighlight = null; // Очистить текущую подсветку
    }
}
