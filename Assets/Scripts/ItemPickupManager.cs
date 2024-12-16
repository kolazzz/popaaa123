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

    private GameObject currentHighlight; // ������� ������������ ������

    private void Start()
    {
        // ���������� "Pistol" ��� ��������� ������ ��� �������
        SetUI("Pistol", null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������, ��� �������� ������������� � �������
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
        // ���������� ����� ������
        if (weaponText != null)
            weaponText.text = weaponName;

        // �������� ���������
        ResetHighlights();

        // ���������� ����� ������
        if (highlightObject != null)
        {
            var spriteRenderer = highlightObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.red; // ���������� �������
                currentHighlight = highlightObject; // ��������� ������� ������������ ������
            }
        }
    }

    private void ResetHighlights()
    {
        // �������� ��������� ����������� �������
        if (currentHighlight != null)
        {
            var spriteRenderer = currentHighlight.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.color = Color.white; // ������� ����� ����
        }

        currentHighlight = null; // �������� ������� ���������
    }
}
