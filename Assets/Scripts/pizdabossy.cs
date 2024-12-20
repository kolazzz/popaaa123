using System.Collections;
using UnityEngine;

public class CameraZoomOnTrigger : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera targetCamera; // ������, ������� ����� ��������������
    public float targetSize = 5f; // �������� ������ ������
    public float zoomSpeed = 2f; // �������� ��������� �������

    [Header("Additional Trigger Action")]
    public GameObject objectToActivate; // ������, ������� ����� ����������

    private float originalSize; // �������� ������ ������
    private Coroutine zoomCoroutine;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main; // ���� ������ �� �������, ������� �������
        }

        originalSize = targetCamera.orthographicSize;

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false); // ���������, ��� ������ ���������� ��������
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
        }
        zoomCoroutine = StartCoroutine(ZoomCamera(targetSize));

        if (objectToActivate != null && !objectToActivate.activeSelf)
        {
            objectToActivate.SetActive(true); // �������� ������, ���� �� ��� �� �������
        }
    }

    IEnumerator ZoomCamera(float target)
    {
        while (!Mathf.Approximately(targetCamera.orthographicSize, target))
        {
            targetCamera.orthographicSize = Mathf.Lerp(targetCamera.orthographicSize, target, zoomSpeed * Time.deltaTime);
            yield return null;
        }

        targetCamera.orthographicSize = target; // ���������, ��� ������ ����� ���������
    }
}