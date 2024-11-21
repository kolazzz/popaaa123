using UnityEngine;
using TMPro;

public class HealthPack : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int healthRestoreAmount = 1; // Количество восстанавливаемого здоровья

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI pickupText; // Ссылка на TextMeshPro для отображения текста
    [SerializeField] private float textDisplayDuration = 2f; // Длительность показа текста

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TopDownCharacterController player = collision.GetComponent<TopDownCharacterController>();
        if (player != null)
        {
            // Восстанавливаем здоровье игрока
            player.RestoreHealth(healthRestoreAmount);

            // Показываем текст
            ShowPickupText($"+{healthRestoreAmount} HP");

            // Уничтожаем объект аптечки
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
