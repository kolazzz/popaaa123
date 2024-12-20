using UnityEngine;

public class ActivateOnExit : MonoBehaviour
{
    public GameObject targetObject;  // Объект, в котором нужно активировать компоненты
    private SpriteRenderer spriteRenderer;
    private Collider2D objectCollider;

    void Start()
    {
        // Получаем компоненты SpriteRenderer и Collider2D из targetObject
        spriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        objectCollider = targetObject.GetComponent<Collider2D>();

        // Убедимся, что компоненты есть на объекте, и изначально они отключены
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; // Отключаем SpriteRenderer по умолчанию
        }

        if (objectCollider != null)
        {
            objectCollider.enabled = false; // Отключаем Collider2D по умолчанию
        }
    }

    // Метод, который срабатывает при выходе игрока из триггерной зоны
    void OnTriggerExit2D(Collider2D other)
    {
        // Проверяем, если объект с тегом "Player" выходит из триггерной зоны
        if (other.CompareTag("Player"))
        {
            // Включаем SpriteRenderer и Collider
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }

            if (objectCollider != null)
            {
                objectCollider.enabled = true;
            }
        }
    }
}
