using UnityEngine;
using TMPro; // ��� TextMeshPro
using UnityEngine.UI; // ��� Image �����������

public class Menuline : MonoBehaviour
{
    public GameObject[] menuRows; // ������ "���" - ��� ������������ ������ � Image � TextMeshProUGUI
    private int selectedIndex = 0; // ������� ������ ������

    private Color32 selectedTextColor = new Color32(255, 177, 0, 255); // Ƹ���� ����� (#FFB100)
    private Color32 defaultTextColor = new Color32(0, 0, 0, 255);      // ׸���� �����
    private Color32 selectedBackgroundColor = new Color32(0, 0, 0, 255); // ׸���� ���
    private Color32 transparentBackground = new Color32(0, 0, 0, 0);     // ���������� ���

    void Start()
    {
        UpdateMenu();
    }

    void Update()
    {
        // ��������� �� W/S ��� ��������
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
            // �������� ���������� �� ������� ����
            Image background = menuRows[i].GetComponent<Image>();
            TextMeshProUGUI text = menuRows[i].GetComponentInChildren<TextMeshProUGUI>();

            if (i == selectedIndex)
            {
                // ���������� ������: ������ ��� � ����� �����
                background.color = selectedBackgroundColor;
                text.color = selectedTextColor;
            }
            else
            {
                // ������������ ������: ���������� ��� � ������ �����
                background.color = transparentBackground;
                text.color = defaultTextColor;
            }
        }
    }
}
