using UnityEngine;

public class SequentialObjectActivatorWithReset : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToActivate; // ������ �������� ��� ���������
    private int currentIndex = 0; // ������ �������� ������� � �������
    private const int chunkSize = 4; // ���������� �������� � ����� ������ (��������, 4)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivateNextObject();
        }
    }

    private void ActivateNextObject()
    {
        if (currentIndex < objectsToActivate.Length)
        {
            // ����������, �������� �� ����� �������� �����
            if (currentIndex % chunkSize == 0 && currentIndex > 0)
            {
                ResetPreviousChunk();
            }

            // �������� ������� ������
            objectsToActivate[currentIndex].SetActive(true);
            currentIndex++;
        }
    }

    private void ResetPreviousChunk()
    {
        // ��������� ������ � ����� ����������� �����
        int startIndex = Mathf.Max(0, currentIndex - chunkSize);
        int endIndex = Mathf.Min(currentIndex, objectsToActivate.Length);

        // ��������� ������� ����������� �����
        for (int i = startIndex; i < endIndex; i++)
        {
            objectsToActivate[i].SetActive(false);
        }
    }
}
