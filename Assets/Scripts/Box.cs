using UnityEngine;

public class Box : MonoBehaviour
{
    public int health = 5; // Количество очков здоровья коробки
    public GameObject splinterPrefab; // Префаб щепки
    public int splinterCount = 5; // Количество щепок при разрушении коробки
    public float splinterForce = 5f; // Сила разлета щепок

    // Метод, вызываемый при попадании по коробке
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DestroyBox();
        }
    }

    // Метод для разрушения коробки и создания щепок
    private void DestroyBox()
    {
        for (int i = 0; i < splinterCount; i++)
        {
            // Создаем щепку
            GameObject splinter = Instantiate(splinterPrefab, transform.position, Random.rotation);
            // Получаем Rigidbody щепки для добавления силы разлета
            Rigidbody rb = splinter.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Применяем силу разлета к щепке
                Vector3 randomDirection = Random.insideUnitSphere.normalized;
                rb.AddForce(randomDirection * splinterForce, ForceMode.Impulse);
            }
        }

        // Уничтожаем объект коробки
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверка попадания по коробке пулей (на примере объекта с тегом "Bullet")
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1); // Отнимаем 1 HP при попадании пули
            Destroy(collision.gameObject); // Уничтожаем пулю
        }
    }
}