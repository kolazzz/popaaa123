using UnityEngine;

public class Box : MonoBehaviour
{
    public int health = 5; // ���������� ����� �������� �������
    public GameObject splinterPrefab; // ������ �����
    public int splinterCount = 5; // ���������� ����� ��� ���������� �������
    public float splinterForce = 5f; // ���� ������� �����

    // �����, ���������� ��� ��������� �� �������
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DestroyBox();
        }
    }

    // ����� ��� ���������� ������� � �������� �����
    private void DestroyBox()
    {
        for (int i = 0; i < splinterCount; i++)
        {
            // ������� �����
            GameObject splinter = Instantiate(splinterPrefab, transform.position, Random.rotation);
            // �������� Rigidbody ����� ��� ���������� ���� �������
            Rigidbody rb = splinter.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // ��������� ���� ������� � �����
                Vector3 randomDirection = Random.insideUnitSphere.normalized;
                rb.AddForce(randomDirection * splinterForce, ForceMode.Impulse);
            }
        }

        // ���������� ������ �������
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �������� ��������� �� ������� ����� (�� ������� ������� � ����� "Bullet")
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1); // �������� 1 HP ��� ��������� ����
            Destroy(collision.gameObject); // ���������� ����
        }
    }
}