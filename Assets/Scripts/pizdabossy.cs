using System.Collections;
using UnityEngine;

public class CameraZoomOnTrigger : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera targetCamera; // Камера, которую нужно масштабировать
    public float targetSize = 5f; // Желаемый размер камеры
    public float zoomSpeed = 2f; // Скорость изменения размера

    [Header("Additional Trigger Action")]
    public GameObject objectToActivate; // Объект, который будет включаться

    private float originalSize; // Исходный размер камеры
    private Coroutine zoomCoroutine;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main; // Если камера не указана, берется главная
        }

        originalSize = targetCamera.orthographicSize;

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false); // Убедиться, что объект изначально выключен
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
            objectToActivate.SetActive(true); // Включить объект, если он еще не активен
        }
    }

    IEnumerator ZoomCamera(float target)
    {
        while (!Mathf.Approximately(targetCamera.orthographicSize, target))
        {
            targetCamera.orthographicSize = Mathf.Lerp(targetCamera.orthographicSize, target, zoomSpeed * Time.deltaTime);
            yield return null;
        }

        targetCamera.orthographicSize = target; // Убедиться, что размер точно достигнут
    }
}