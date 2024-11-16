using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Скорость пули
    public float lifeTime = 5f; // Время жизни пули

    void Start()
    {
        // Запуск корутины уничтожения через lifeTime секунд
        StartCoroutine(DestroyAfterTime());
    }

    void Update()
    {
        // Перемещение пули вперед
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Уничтожение пули при столкновении с препятствием
        Destroy(gameObject);
    }

    IEnumerator DestroyAfterTime()
    {
        // Ожидание lifeTime секунд
        yield return new WaitForSeconds(lifeTime);
        // Уничтожение пули
        Destroy(gameObject);
    }
}
