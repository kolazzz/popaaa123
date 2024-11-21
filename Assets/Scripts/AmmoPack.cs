using UnityEngine;
using TMPro;

public class AmmoPack : MonoBehaviour
{
    [Header("Ammo Settings")]
    [SerializeField] private int ammoRestoreAmount = 5; // ���������� ����������������� ��������

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI pickupText; // ������ �� TextMeshPro ��� ����������� ������
    [SerializeField] private float textDisplayDuration = 2f; // ������������ ������ ������

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TopDownCharacterController player = collision.GetComponent<TopDownCharacterController>();
        if (player != null)
        {
            // ��������������� ������� ������
            player.RestoreAmmo(ammoRestoreAmount);

            // ���������� �����
            ShowPickupText($"+{ammoRestoreAmount} Ammo");

            // ���������� ������ ��������
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
