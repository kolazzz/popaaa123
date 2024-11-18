using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public GameObject door; // �����, ������� ����� �������
    public GameObject[] enemies; // ����� � �������
    private bool allEnemiesDestroyed = false; // ���� ��� ��������, ��� ��� ����� ����������

    // ����� ��� ��������, ���������� �� ��� �����
    void Update()
    {
        CheckEnemiesStatus();
    }

    // �������� ������� ������
    void CheckEnemiesStatus()
    {
        allEnemiesDestroyed = true; // �����������, ��� ��� ����� ����������

        // ��������� ���� ������
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null) // ���� ���� ��� ����������, ������ ���� � false
            {
                allEnemiesDestroyed = false;
                break;
            }
        }

        // ���� ��� ����� ����������, ��������� �����
        if (allEnemiesDestroyed)
        {
            OpenDoor();
        }
    }

    // ������� ����� (��������, ������������ � ��� ���������)
    void OpenDoor()
    {
        if (door != null)
        {
            door.SetActive(false); // ��������� ����� (����� ������������ �������� ��� ����� �������� �������)
        }
    }
}
