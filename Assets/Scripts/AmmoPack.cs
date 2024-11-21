using UnityEngine;
using TMPro;

public class AmmoPack : MonoBehaviour
{
    [Header("Ammo Settings")]
    [SerializeField] private int ammoRestoreAmount = 5; // Количество восстанавливаемых патронов

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI pickupText; // Ссылка на TextMeshPro для отображения текста
    [SerializeField] private float textDisplayDuration = 2f; // Длительность показа текста

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TopDownCharacterController player = collision.GetComponent<TopDownCharacterController>();
        if (player != null)
        {
            // Восстанавливаем патроны игроку
            player.RestoreAmmo(ammoRestoreAmount);

            // Показываем текст
            ShowPickupText($"+{ammoRestoreAmount} Ammo");

            // Уничтожаем объект патронов
            Destroy(gameObject);
        }
    }

    private void ShowPickupText(string message)
    {
        if (pickupText != null)
        {
            pickupText.text = message;
            pickupText.gameObject.SetActive(true);
            Invoke(nameof(HidePickupText), textDisplayDuration);
        }
    }

    private void HidePickupText()
    {
        if (pickupText != null)
        {
            pickupText.gameObject.SetActive(false);
        }
    }
}
