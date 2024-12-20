using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectCounter : MonoBehaviour
{
    [Header("Object Layer Settings")]
    [SerializeField] private LayerMask targetLayer;  // ����, ������� �������� ����� �����������

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI counterText; // ������ �� TextMeshPro ��� ����������� ��������

    private int objectCount;  // ������� �������� � ��������� �����

    void Start()
    {
        // ������������� ��������
        objectCount = GetObjectsCount();
        UpdateCounterText();

        // ����������� ��� ������������ ��������� ��������
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDestroy()
    {
        // ������� ��������, ����� �������� ������ ������
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void Update()
    {
        // ������ ���� ��������� ���������� ��������
        int newCount = GetObjectsCount();
        if (newCount != objectCount)
        {
            objectCount = newCount;
            UpdateCounterText();
        }
    }

    // ����� ��� ��������� ���������� �������� � ������ ����� � �����
    private int GetObjectsCount()
    {
        // ����� ��� ������� � ������ �����
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy"); // ��� ������ �������� � ���������� �����

        int count = 0;
        foreach (GameObject obj in objects)
        {
            if (((1 << obj.layer) & targetLayer) != 0)
            {
                count++;
            }
        }
        return count;
    }

    // ����� ��� ���������� ������
    private void UpdateCounterText()
    {
        counterText.text = objectCount.ToString();  // ����������� ���������� ��������
    }

    private void OnSceneUnloaded(Scene currentScene)
    {
        // �������� ������� �������� ��� �������� ����� (���� ������� ���������)
        objectCount = GetObjectsCount();
        UpdateCounterText();
    }
}