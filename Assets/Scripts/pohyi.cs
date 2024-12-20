using TMPro;
using UnityEngine;

public class BlockCharacterControl : MonoBehaviour
{
    public GameObject player;           // ������ �� ������ ���������
    private TopDownCharacterController playerController;  // ������ �� ��������� ���������� ����������
    public TextMeshProUGUI[] blockingTexts;  // ������ ������ �� ��������� ����������, ������� ��������� ����������

    void Start()
    {
        // �������� ��������� PlayerController (�����������, ��� �� ����������)
        playerController = player.GetComponent<TopDownCharacterController>();
    }

    void Update()
    {
        // ���������, ���� �� �� ����� �������� ����������� ������
        bool isBlockingTextActive = false;

        foreach (TextMeshProUGUI text in blockingTexts)
        {
            if (text != null && text.gameObject.activeInHierarchy && !string.IsNullOrEmpty(text.text))
            {
                isBlockingTextActive = true;
                break;
            }
        }

        // ���� ����� �������, ��������� ����������
        if (isBlockingTextActive)
        {
            playerController.enabled = false; // ��������� ���������� ����������
        }
        else
        {
            playerController.enabled = true; // �������� ����������, ���� ����� �� �������
        }
    }
}
