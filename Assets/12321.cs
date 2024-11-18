using TMPro;
using UnityEngine;

public class DisableTextsOnClick : MonoBehaviour
{
    public TextMeshProUGUI[] textsToDisable;  // ������ �������, ������� ����� �����������

    void Update()
    {
        // ���������, ���� �� ������ ����� ������ ����
        if (Input.GetMouseButtonDown(0))  // 0 ������������� ����� ������ ����
        {
            DisableTexts();  // ��������� ��� ������
        }
    }

    // ����� ��� ����������� ���� ������� � �������
    void DisableTexts()
    {
        foreach (TextMeshProUGUI text in textsToDisable)
        {
            if (text != null)
            {
                text.gameObject.SetActive(false);  // ������������ �����
            }
        }
    }
}