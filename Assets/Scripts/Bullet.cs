using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // �������� ����
    public float lifeTime = 5f; // ����� ����� ����

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
        // ����������� ���� ��� ������������ � ������������
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
