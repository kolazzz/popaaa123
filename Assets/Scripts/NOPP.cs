using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NOPP : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas; // Canvas, который содержит UI-объекты
    [SerializeField] private Camera mainCamera; // Главная камера сцены
    [SerializeField] private bool disablePostProcessing = true; // Включить/отключить запрет пост-обработки

    private Camera uiCamera;
    private UniversalAdditionalCameraData uiCameraData;

    private void Start()
    {
        if (targetCanvas == null || mainCamera == null)
        {
            Debug.LogError("Необходимо указать Canvas и главную камеру!");
            return;
        }

        if (targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Debug.LogError("Canvas должен быть в режиме Screen Space - Camera или World Space!");
            return;
        }

        // Создаем камеру для рендеринга UI
        GameObject cameraObject = new GameObject("UICamera");
        cameraObject.transform.parent = mainCamera.transform;
        cameraObject.transform.localPosition = Vector3.zero;
        cameraObject.transform.localRotation = Quaternion.identity;

        uiCamera = cameraObject.AddComponent<Camera>();
        uiCamera.clearFlags = CameraClearFlags.Depth;
        uiCamera.cullingMask = targetCanvas.gameObject.layer; // Рендерим только слой Canvas

        // Связываем камеру с Canvas
        targetCanvas.worldCamera = uiCamera;

        // Настройка Universal Render Pipeline для новой камеры
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
