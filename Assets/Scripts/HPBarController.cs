using UnityEngine;

public class HPBarController : MonoBehaviour
{
    [Header("������ �� �������")]
    public Sprite[] hpSprites; // ������ �������� ��� HP

    [Header("������ �� SpriteRenderer HP-����")]
    public SpriteRenderer spriteRenderer; // ������ �� ��������� SpriteRenderer

    private TopDownCharacterController playerController;

    void Start()
    {
        playerController = FindObjectOfType<TopDownCharacterController>(); // ������� ������ ������
        if (playerController == null)
        {
            Debug.LogError("����� �� ������!");
        }
    }

    void Update()
    {
        if (playerController != null)
        {
            // ��������� ������ HP-���� � ����������� �� �������� �������� ������
            UpdateHPBar();
        }
    }

    private void UpdateHPBar()
    {
        // �������� ������� �������� ������ � ��������� ������ HP-����
        int currentHealth = playerController.GetCurrentHealth();
        if (currentHealth >= 1 && currentHealth <= hpSprites.Length)
        {
            spriteRenderer.sprite = hpSprites[currentHealth - 1];
        }
        else if (currentHealth <= 0)
        {
            spriteRenderer.sprite = hpSprites[0]; // ���� �������� 0 ��� ������, ���������� ����� ������ ������
        }
    }
}
