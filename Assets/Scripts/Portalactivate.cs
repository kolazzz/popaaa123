using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [Header("Portal Prefab")]
    [SerializeField] private GameObject portalPrefab;  // Префаб портала

    [Header("Spawn Position")]
    [SerializeField] private Transform spawnPoint;  // Точка появления портала

    private Collider2D activatorCollider;  // Ссылка на коллайдер активатора
    private bool hasSpawned = false;  // Флаг, чтобы портал создавался только один раз

    void Start()
    {
        activatorCollider = GetComponent<Collider2D>();

        if (portalPrefab == null)
        {
            Debug.LogError("Portal prefab is not assigned in PortalSpawner.");
        }

        if (spawnPoint == null)
        {
            Debug.LogWarning("Spawn point is not assigned. Using activator's position.");
            spawnPoint = transform;  // Если не указана точка, используем позицию активатора
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSpawned)
            return;  // Прекращаем выполнение, если портал уже был создан

        if (other.CompareTag("Player"))  // Проверяем, что это игрок
        {
            SpawnPortal();
            DisableActivator();
        }
    }

    /// <summary>
    /// Создает портал в указанной позиции.
    /// </summary>
    private void SpawnPortal()
    {
        if (portalPrefab != null)
        {
            GameObject portalInstance = Instantiate(portalPrefab, spawnPoint.position, Quaternion.identity);

            // Убедимся, что SpriteRenderer включен
            SpriteRenderer spriteRenderer = portalInstance.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                Debug.LogWarning("No SpriteRenderer found on the portal prefab.");
            }

            hasSpawned = true;  // Помечаем, что портал был создан
        }
        else
        {
            Debug.LogError("Portal prefab is not assigned.");
        }
    }


    /// <summary>
    /// Отключает активатор, чтобы предотвратить повторное создание портала.
    /// </summary>
    private void DisableActivator()
    {
        if (activatorCollider != null)
        {
            activatorCollider.enabled = false;  // Деактивируем коллайдер
        }
    }
}
