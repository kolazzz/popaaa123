using UnityEngine;

public class ActivateOnExit : MonoBehaviour
{
    public GameObject targetObject;  // ������, � ������� ����� ������������ ����������
    private SpriteRenderer spriteRenderer;
    private Collider2D objectCollider;

    void Start()
    {
        // �������� ���������� SpriteRenderer � Collider2D �� targetObject
        spriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        objectCollider = targetObject.GetComponent<Collider2D>();

        // ��������, ��� ���������� ���� �� �������, � ���������� ��� ���������
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; // ��������� SpriteRenderer �� ���������
        }

        if (objectCollider != null)
        {
            objectCollider.enabled = false; // ��������� Collider2D �� ���������
        }
    }

    // �����, ������� ����������� ��� ������ ������ �� ���������� ����
    void OnTriggerExit2D(Collider2D other)
    {
        // ���������, ���� ������ � ����� "Player" ������� �� ���������� ����
        if (other.CompareTag("Player"))
        {
            // �������� SpriteRenderer � Collider
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
