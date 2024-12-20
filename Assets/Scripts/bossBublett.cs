using System.Collections;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    public float speed = 10f; // �������� ����
    public float lifeTime = 5f; // ����� ����� ����
    public int damage = 1; // ���� �� ����

    private Transform player;
    private Vector2 targetDirection;

    void Start()
    {
        // ����� ������
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            targetDirection = (player.position - transform.position).normalized;
        }

        // ������ �������� ����������� ����� lifeTime ������
        StartCoroutine(DestroyAfterTime());
    }

    void Update()
    {
        // ���� ����� ������, ��������� � ����
        if (player != null)
        {
            transform.position += (Vector3)targetDirection * speed * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ������� ���� ������, ���� ������������ ��������� � �������
        if (collision.gameObject.CompareTag("Player"))
        {
            TopDownCharacterController playerController = collision.gameObject.GetComponent<TopDownCharacterController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
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