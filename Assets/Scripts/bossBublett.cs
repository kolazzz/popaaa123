using System.Collections;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    public float speed = 10f; // Скорость пули
    public float lifeTime = 5f; // Время жизни пули
    public int damage = 1; // Урон от пули

    private Transform player;
    private Vector2 targetDirection;

    void Start()
    {
        // Найти игрока
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            targetDirection = (player.position - transform.position).normalized;
        }

        // Запуск корутины уничтожения через lifeTime секунд
        StartCoroutine(DestroyAfterTime());
    }

    void Update()
    {
        // Если игрок найден, двигаться к нему
        if (player != null)
        {
            transform.position += (Vector3)targetDirection * speed * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Нанести урон игроку, если столкновение произошло с игроком
        if (collision.gameObject.CompareTag("Player"))
        {
            TopDownCharacterController playerController = collision.gameObject.GetComponent<TopDownCharacterController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
        }

        // Уничтожить пулю при любом столкновении
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