using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // �������� ����
    public float lifeTime = 5f; // ����� ����� ����
    public int damage = 1; // ���� �� ����

    void Start()
    {
        // ������ �������� ����������� ����� lifeTime ������
        StartCoroutine(DestroyAfterTime());
    }

    void Update()
    {
        // ����������� ���� ������
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ���������, ���� �� � ������� ������ EnemyAI
        if (collision.gameObject.TryGetComponent(out EnemyAI enemy))
        {
            // ������� ���� �����
            enemy.TakeDamage(damage);
        }

        // ���������� ���� ��� ����� ������������
        Destroy(gameObject);
    }

    IEnumerator DestroyAfterTime()
    {
        // �������� lifeTime ������
        yield return new WaitForSeconds(lifeTime);
        // ����������� ����
        Destroy(gameObject);
    }
}
