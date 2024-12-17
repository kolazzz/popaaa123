using UnityEngine;

public class TRIGGERPAYSA : MonoBehaviour
{
    [Header("������ ��� �����������")]
    [SerializeField] private GameObject pauseObject; // ������, ������� ����� ������������

    private bool isPaused = false;

    private void Start()
    {
        if (pauseObject != null)
        {
            pauseObject.SetActive(false); // �������� ������ ��� ������
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPaused) // ���������, ��� �������� �����
        {
            Debug.Log("����� ����� � �������-����."); // ����� �����
            PauseGame();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // ���������, ��� ����� ����� �� ����
        {
            Debug.Log("����� ����� �� �������-����."); // ����� ������
        }
    }

    private void Update()
    {
        if (isPaused && Input.GetMouseButtonDown(0)) // ����������� ���� �� ����� ���
        {
            Debug.Log("����� ����� ��� ��� ������������� ����."); // ����� �����
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // ������������� �����
        if (pauseObject != null)
        {
            pauseObject.SetActive(true); // ���������� ������
        }
        Debug.Log("���� ���������� �� �����."); // ����� �����
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // ������������ �����
        if (pauseObject != null)
        {
            pauseObject.SetActive(false); // �������� ������
        }
        Debug.Log("���� ������������."); // ����� �������������
    }
}