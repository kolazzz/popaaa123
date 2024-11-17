using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NOPP : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas; // Canvas, ������� �������� UI-�������
    [SerializeField] private Camera mainCamera; // ������� ������ �����
    [SerializeField] private bool disablePostProcessing = true; // ��������/��������� ������ ����-���������

    private Camera uiCamera;
    private UniversalAdditionalCameraData uiCameraData;

    private void Start()
    {
        if (targetCanvas == null || mainCamera == null)
        {
            Debug.LogError("���������� ������� Canvas � ������� ������!");
            return;
        }

        if (targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Debug.LogError("Canvas ������ ���� � ������ Screen Space - Camera ��� World Space!");
            return;
        }

        // ������� ������ ��� ���������� UI
        GameObject cameraObject = new GameObject("UICamera");
        cameraObject.transform.parent = mainCamera.transform;
        cameraObject.transform.localPosition = Vector3.zero;
        cameraObject.transform.localRotation = Quaternion.identity;

        uiCamera = cameraObject.AddComponent<Camera>();
        uiCamera.clearFlags = CameraClearFlags.Depth;
        uiCamera.cullingMask = targetCanvas.gameObject.layer; // �������� ������ ���� Canvas

        // ��������� ������ � Canvas
        targetCanvas.worldCamera = uiCamera;

        // ��������� Universal Render Pipeline ��� ����� ������
        uiCameraData = uiCamera.GetUniversalAdditionalCameraData();
        uiCameraData.renderPostProcessing = !disablePostProcessing;
    }

    private void OnValidate()
    {
        if (uiCamera != null && uiCameraData != null)
        {
            uiCameraData.renderPostProcessing = !disablePostProcessing;
        }
    }
}
