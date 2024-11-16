using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Ссылка на игрока
    public Vector3 offset;   // Смещение камеры от позиции игрока

    void LateUpdate()
    {
        if (target != null)
        {
            // Следуем за позицией игрока
            transform.position = target.position + offset;
            // Зафиксированное вращение камеры
            transform.rotation = Quaternion.identity;
        }
    }
}
