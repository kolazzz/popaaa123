using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [Header("Portal Prefab")]
    [SerializeField] private GameObject portalPrefab;  // ������ �������

    [Header("Spawn Position")]
    [SerializeField] private Transform spawnPoint;  // ����� ��������� �������

    private Collider2D activatorCollider;  // ������ �� ��������� ����������
    private bool hasSpawned = false;  // ����, ����� ������ ���������� ������ ���� ���

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
            spawnPoint = transform;  // ���� �� ������� �����, ���������� ������� ����������
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSpawned)
            return;  // ���������� ����������, ���� ������ ��� ��� ������

        if (other.CompareTag("Player"))  // ���������, ��� ��� �����
        {
            SpawnPortal();
            DisableActivator();
        }
    }

    /// <summary>
    /// ������� ������ � ��������� �������.
    /// </summary>
    private void SpawnPortal()
    {
        if (portalPrefab != null)
        {
            GameObject portalInstance = Instantiate(portalPrefab, spawnPoint.position, Quaternion.identity);

            // ��������, ��� SpriteRenderer �������
            SpriteRenderer spriteRenderer = portalInstance.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                Debug.LogWarning("No SpriteRenderer found on the portal prefab.");
            }

            hasSpawned = true;  // ��������, ��� ������ ��� ������
        }
        else
        {
            Debug.LogError("Portal prefab is not assigned.");
        }
    }


    /// <summary>
    /// ��������� ���������, ����� ������������� ��������� �������� �������.
    /// </summary>
    private void DisableActivator()
    {
        if (activatorCollider != null)
        {
            activatorCollider.enabled = false;  // ������������ ���������
        }
    }
}
