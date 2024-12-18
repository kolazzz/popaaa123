using UnityEngine;
using TMPro;
using System.Collections;

public class PauseOnTrigger : MonoBehaviour
{
    [Header("������ ��� �����������")]
    [SerializeField] private GameObject dialogBox; // ���� �������
    [SerializeField] private TextMeshProUGUI dialogText; // ���� ������ ��� ������ ��������� ������
    [SerializeField] private TextMeshProUGUI secondaryText; // ������ �� ����� ��� ��������������� ������
    [SerializeField] private string dialogMessage = "��� ������ ������ �������."; // �������� ����� �������
    [SerializeField] private string secondaryMessage = "��� �������������� �����."; // �������������� �����
    [SerializeField] private float typingSpeed = 0.05f; // �������� ��������� (� �������� ����� �������)
    [SerializeField] private KeyCode[] continueKeys; // ������ ������ ��� �����������

    private bool isPaused = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private Collider2D triggerCollider; // ��������� �������-����

    private void Start()
    {
        // �������� ���� ������� � ������� ����� ��� ������
        if (dialogBox != null)
        {
            dialogBox.SetActive(false);
        }

        if (dialogText != null)
        {
            dialogText.text = ""; // ������� �������� �����
        }

        if (secondaryText != null)
        {
            secondaryText.text = ""; // ������� �������������� �����
        }

        // �������� ������ �� ��������� �������-����
        triggerCollider = GetComponent<Collider2D>();
        if (triggerCollider == null)
        {
            Debug.LogError("��������� ��� �������-���� �� ������!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPaused)
        {
            Debug.Log("����� ����� � �������-����.");
            PauseGame();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("����� ����� �� �������-����.");
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false; // ��������� �������
                Debug.Log("������� ��������.");
            }
        }
    }

    private void Update()
    {
        if (isPaused && !isTyping && AnyContinueKeyPressed()) // �������� ������� ����� ������� �� �������
        {
            Debug.Log("����� ����� ������� ��� �����������.");
            ResumeGame();
        }
    }

    private bool AnyContinueKeyPressed()
    {
        foreach (KeyCode key in continueKeys)
        {
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }
        return false;
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (dialogBox != null)
        {
            dialogBox.SetActive(true); // ���������� ���� �������
        }

        if (dialogText != null)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // ������������� ���������� ��������, ���� ����
            }
            typingCoroutine = StartCoroutine(TypeText(dialogMessage)); // �������� �������� �����
        }
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (dialogBox != null)
        {
            dialogBox.SetActive(false); // �������� ���� �������
        }

        if (dialogText != null)
        {
            dialogText.text = ""; // ������� �������� �����
        }

        if (secondaryText != null)
        {
            secondaryText.text = ""; // ������� �������������� �����
        }
    }

    private IEnumerator TypeText(string message)
    {
        isTyping = true;
        dialogText.text = ""; // ������� ����� ����� ������� ��������
        int index = 0;
        string cursor = "<color=#FFFFFF>|</color>"; // ������ ��� ��������

        while (index <= message.Length)
        {
            dialogText.text = message.Substring(0, index) + cursor; // ��������� ����� �� ������ �������
            index++;
            yield return new WaitForSecondsRealtime(typingSpeed); // ���������� WaitForSecondsRealtime ��� ������ � �����
        }

        dialogText.text = message; // ������ ����� ��� �������
        isTyping = false;

        // �������� �������������� �����
        if (secondaryText != null)
        {
            secondaryText.text = secondaryMessage; // ������������� �������������� �����
            Debug.Log("������� �������������� �����.");
        }

        // �������� ������
        StartCoroutine(BlinkCursor());
    }

    private IEnumerator BlinkCursor()
    {
        string baseText = dialogText.text;

        while (isPaused) // ������ ��������, ���� ������ ������
        {
            dialogText.text = baseText + "<color=#FFFFFF>|</color>";
            yield return new WaitForSecondsRealtime(0.5f); // ���������� WaitForSecondsRealtime ��� �������
            dialogText.text = baseText;
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
