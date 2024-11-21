using UnityEngine;
using TMPro;

public class HealthPack : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int healthRestoreAmount = 1; // ���������� ������������������ ��������

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI pickupText; // ������ �� TextMeshPro ��� ����������� ������
    [SerializeField] private float textDisplayDuration = 2f; // ������������ ������ ������

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TopDownCharacterController player = collision.GetComponent<TopDownCharacterController>();
        if (player != null)
        {
            // ��������������� �������� ������
            player.RestoreHealth(healthRestoreAmount);

            // ���������� �����
            ShowPickupText($"+{healthRestoreAmount} HP");

            // ���������� ������ �������
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
