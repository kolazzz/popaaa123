using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [Header("Animation Settings")]
    public Sprite[] bossSprites;
    public float animationSpeed = 0.1f;

    private SpriteRenderer spriteRenderer;
    private float animationTimer;
    private int currentFrame;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float maxShootInterval = 5f;

    private float shootTimer;

    [Header("Wave Attack Settings")]
    public GameObject wavePrefab;
    public int waveCount = 8;
    public float waveRadius = 5f;
    public float waveCooldown = 10f;

    private float waveTimer;

    [Header("Health Settings")]
    public int maxHealth = 20; // Максимальное здоровье босса
    private int currentHealth;

    [Header("Death Animation Settings")]
    public Sprite[] deathSprites; // Массив спрайтов для анимации смерти
    public float deathAnimationSpeed = 0.1f; // Скорость анимации смерти

    private bool isDying = false; // Флаг, указывающий на проигрывание анимации смерти

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        shootTimer = Random.Range(0f, maxShootInterval);
        waveTimer = waveCooldown;
        currentHealth = maxHealth; // Устанавливаем начальное здоровье
    }

    void Update()
    {
        if (isDying) return; // Если босс умирает, блокируем остальные действия

        PlayAnimation();

        shootTimer -= Time.deltaTime;
        waveTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = Random.Range(0f, maxShootInterval);
        }

        if (waveTimer <= 0f)
        {
            PerformWaveAttack();
            waveTimer = waveCooldown;
        }
    }

    void PlayAnimation()
    {
        if (bossSprites.Length == 0) return;

        animationTimer += Time.deltaTime;
        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            currentFrame = (currentFrame + 1) % bossSprites.Length;
            spriteRenderer.sprite = bossSprites[currentFrame];
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    void PerformWaveAttack()
    {
        if (wavePrefab == null) return;

        for (int i = 0; i < waveCount; i++)
        {
            float angle = i * (360f / waveCount);
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            Vector3 spawnPosition = transform.position + (Vector3)direction * waveRadius;

            GameObject wave = Instantiate(wavePrefab, spawnPosition, Quaternion.identity);
            wave.transform.up = direction; // Направляем волну наружу
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDying) return; // Не уменьшаем здоровье, если босс уже умирает

        currentHealth -= damage; // Уменьшаем здоровье босса

        if (currentHealth <= 0)
        {
            StartCoroutine(PlayDeathAnimation()); // Начинаем анимацию смерти
        }
    }

    IEnumerator PlayDeathAnimation()
    {
        isDying = true; // Устанавливаем флаг смерти

        for (int i = 0; i < deathSprites.Length; i++)
        {
            spriteRenderer.sprite = deathSprites[i];
            yield return new WaitForSeconds(deathAnimationSpeed);
        }

        Destroy(gameObject); // Уничтожаем объект после анимации
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, нанес ли урон объект с тегом "PlayerWeapon" (например, катана)
        if (collision.gameObject.CompareTag("Bullet"))
        {
            int damage = 1; // Урон от оружия игрока (можно сделать параметром)
            TakeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, waveRadius);
    }
}
