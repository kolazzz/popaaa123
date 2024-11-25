using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class TopDownCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float regularShootCooldown = 1f;  // Кулдаун для обычного оружия (1 секунда)
    private float shotgunShootCooldown = 2f;  // Кулдаун для дробовика (3 секунды)

    private float regularShootTimer = 0f;     // Таймер для отслеживания кулдауна обычного оружия
    private float shotgunShootTimer = 0f;     // Таймер для отслеживания кулдауна дробовика

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bullet2Prefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask damageLayerMask;

    private List<GameObject> highlights = new List<GameObject>(); // Список подсветок


    private Camera _mainCamera;
    
    [Header("Collider Visualization Settings")]
    [SerializeField] private string objectsTagToShowColliders = "Enemy"; // Тег объектов для отображения коллайдеров
    [SerializeField] private float colliderVisibilityDuration = 3f; // Длительность отображения
    private bool showColliders = false;
    private float colliderTimer = 0f;

    // Анимация через спрайты
    [Header("Animation Sprites")]
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private float animationSpeed = 0.1f;

    private SpriteRenderer spriteRenderer;
    private float animationTimer;
    private int currentFrame;
    private bool isRunning;

    // Анимация получения урона
    [Header("Damage Animation")]
    [SerializeField] private Sprite[] damageSprites;
    [SerializeField] private float damageAnimationSpeed = 0.1f;
    private bool isTakingDamage = false;

    // Анимация смерти
    [Header("Death Animation")]
    [SerializeField] private Sprite[] deathSprites;
    [SerializeField] private float deathAnimationSpeed = 0.2f;
    private bool isDying = false;

    // Линия прицела
    [Header("Aim Line Settings")]
    [SerializeField] private Color aimLineColor = Color.red;
    [SerializeField] private float aimLineMaxLength = 5f;
    private LineRenderer aimLine;

    // Система здоровья
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 4;
    private int currentHealth;

    // Лимит на количество выстрелов
    [Header("Shooting Settings")]
    [SerializeField] private int maxShots = 15;
    private int currentShots;

    // UI для патронов
    [Header("Ammo Settings")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private SpriteRenderer ammoSpriteRenderer;
    [SerializeField] private Sprite[] ammoSprites;

    // Затемнение экрана
    [Header("Death Screen Effect")]
    [SerializeField] private GameObject darknessOverlay; // UI-панель с затемнением
    [SerializeField] private float darknessFadeSpeed = 1f; // Скорость появления затемнения
    [SerializeField] private Canvas gameCanvas; // Canvas, который нужно выключить

    private bool isSceneFrozen = false; // Флаг остановки сцены

    // Тип оружия
    private enum WeaponType { Regular, Shotgun }
    private WeaponType currentWeapon = WeaponType.Regular;

    [SerializeField] private GameObject[] weaponPickups; // Массив объектов оружия для подбора

    // Параметры дробовика
    [Header("Shotgun Settings")]
    [SerializeField] private float shotgunSpreadAngle = 15f;  // Угол рассеивания пуль
    [SerializeField] private int shotgunPelletCount = 3; // Количество пуль, выстреливаемых дробовиком

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        aimLine = gameObject.AddComponent<LineRenderer>();
        aimLine.startWidth = 0.05f;
        aimLine.endWidth = 0.05f;
        aimLine.material = new Material(Shader.Find("Sprites/Default"));
        aimLine.startColor = aimLineColor;
        aimLine.endColor = aimLineColor;

        currentHealth = maxHealth;
        currentShots = 0;

        UpdateAmmoUI();
    }

    void Update()
    {
        if (isDying || isTakingDamage) return; // Блокируем управление при проигрывании анимации

        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        isRunning = moveInput.magnitude > 0;

        UpdateAnimation();
        RotateTowardsCursor();

        // Стрельба в зависимости от оружия
        if (currentWeapon == WeaponType.Regular)
        {
            RegularShoot();
        }
        else if (currentWeapon == WeaponType.Shotgun)
        {
            ShotgunShoot();
        }

        UpdateAimLine();

        if (_mainCamera != null)
        {
            _mainCamera.transform.rotation = Quaternion.identity;
        }

        if (regularShootTimer > 0f)
        {
            regularShootTimer -= Time.deltaTime;
        }

        if (shotgunShootTimer > 0f)
        {
            shotgunShootTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ShowEnemyColliders();
            colliderTimer = colliderVisibilityDuration; // Устанавливаем таймер
        }

        if (colliderTimer > 0f)
        {
            colliderTimer -= Time.deltaTime;

            if (colliderTimer <= 0f)
            {
                HideEnemyColliders();
            }
        }
    }

    void FixedUpdate()
    {
        if (isDying || isTakingDamage) return; // Блокируем движение при проигрывании анимации
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void RegularShoot()
    {
        if (Input.GetMouseButtonDown(0) && currentShots < maxShots && regularShootTimer <= 0f)
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            // Направление выстрела
            Vector2 direction = mousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Создание пули и выстрел
            Instantiate(bulletPrefab, firePoint.position, bulletRotation);

            currentShots++;  // Уменьшаем количество патронов
            UpdateAmmoUI();

            // Сбросить кулдаун
            regularShootTimer = regularShootCooldown;
        }
    }


    private void ShotgunShoot()
    {
        if (Input.GetMouseButtonDown(0) && currentShots < maxShots && shotgunShootTimer <= 0f)
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            // Стреляем тремя пулями с углом рассеивания
            for (int i = -1; i <= 1; i++)
            {
                Vector2 direction = (mousePosition - transform.position).normalized;
                float angleOffset = shotgunSpreadAngle * i; // Смещение угла для каждого выстрела
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
                Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, 0, angle));

                // Создание пуль для дробовика
                Instantiate(bullet2Prefab, firePoint.position, bulletRotation);
            }

            currentShots++;  // Уменьшаем количество патронов
            UpdateAmmoUI();

            // Сбросить кулдаун
            shotgunShootTimer = shotgunShootCooldown;
        }
    }



    private void UpdateAnimation()
    {
        Sprite[] currentSprites = isRunning ? runSprites : idleSprites;
        animationTimer += Time.deltaTime;
        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            currentFrame = (currentFrame + 1) % currentSprites.Length;
            spriteRenderer.sprite = currentSprites[currentFrame];
        }
    }

    private void RotateTowardsCursor()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void UpdateAimLine()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 direction = (mousePosition - transform.position).normalized;
        Vector3 aimEndPoint = transform.position + (Vector3)direction * aimLineMaxLength;

        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, aimEndPoint);
    }

    private void UpdateAmmoUI()
    {
        ammoText.text = (maxShots - currentShots).ToString();
        int ammoIndex = (currentShots / 3) % ammoSprites.Length;
        ammoSpriteRenderer.sprite = ammoSprites[ammoIndex];
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && !isDying)
        {
            StartCoroutine(PlayDeathAnimation());
        }
        else if (!isTakingDamage) // Анимация получения урона
        {
            StartCoroutine(PlayDamageAnimation());
        }
    }

    private IEnumerator PlayDamageAnimation()
    {
        isTakingDamage = true;
        rb.linearVelocity = Vector2.zero;

        for (int i = 0; i < damageSprites.Length; i++)
        {
            spriteRenderer.sprite = damageSprites[i];
            yield return new WaitForSeconds(damageAnimationSpeed);
        }

        isTakingDamage = false;
    }

    private IEnumerator PlayDeathAnimation()
    {
        isDying = true;
        rb.linearVelocity = Vector2.zero;

        // Остановить сцену
        FreezeScene();

        // Отключить Canvas
        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(false);
        }

        // Включить затемнение
        yield return StartCoroutine(FadeInDarkness());

        // Анимация смерти
        for (int i = 0; i < deathSprites.Length; i++)
        {
            spriteRenderer.sprite = deathSprites[i];
            yield return new WaitForSeconds(deathAnimationSpeed);
        }

        // Перезапуск сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void FreezeScene()
    {
        if (isSceneFrozen) return;

        isSceneFrozen = true;

        // Найти все объекты с Rigidbody2D и отключить их
        foreach (Rigidbody2D rb in FindObjectsOfType<Rigidbody2D>())
        {
            if (rb.gameObject != gameObject) // Исключить игрока
            {
                rb.linearVelocity = Vector2.zero;
                rb.isKinematic = true;
            }
        }

        // Найти все активные MonoBehaviour и отключить их
        foreach (MonoBehaviour script in FindObjectsOfType<MonoBehaviour>())
        {
            if (script.gameObject != gameObject) // Исключить игрока
            {
                script.enabled = false;
            }
        }
    }

    private IEnumerator FadeInDarkness()
    {
        if (darknessOverlay == null) yield break;

        CanvasGroup canvasGroup = darknessOverlay.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = darknessOverlay.AddComponent<CanvasGroup>();
        }

        darknessOverlay.SetActive(true);
        canvasGroup.alpha = 0f;

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * darknessFadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    public void ApplyKnockback(Vector2 direction, float force = 5f)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, столкнулся ли игрок с объектом оружия
        foreach (GameObject weaponPickup in weaponPickups)
        {
            if (collision.gameObject == weaponPickup)
            {
                SwitchToShotgun();
                Destroy(collision.gameObject);  // Удаление объекта оружия
                break;
            }
        }

        // Механика получения урона
        if (LayerMaskUtil.ContainsLayer(damageLayerMask, collision.gameObject))
        {
            TakeDamage(1);
        }
    }

    private void SwitchToShotgun()
    {
        currentWeapon = WeaponType.Shotgun;
        Debug.Log("Switched to Shotgun");
    }

    public void RestoreHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public void RestoreAmmo(int amount)
    {
        currentShots = Mathf.Clamp(currentShots - amount, 0, maxShots);
        UpdateAmmoUI();
    }

    private void ShowEnemyColliders()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(objectsTagToShowColliders);

        foreach (GameObject enemy in enemies)
        {
            // Находим триггерные CircleCollider2D
            CircleCollider2D[] colliders = enemy.GetComponents<CircleCollider2D>()
                .Where(c => c.isTrigger) // Только триггеры
                .ToArray();

            if (colliders.Length == 0)
            {
                Debug.LogWarning($"Enemy '{enemy.name}' does not have a trigger CircleCollider2D.");
                continue;
            }

            foreach (var collider in colliders)
            {
                // Создаем объект подсветки
                var highlight = new GameObject("Highlight");
                var lineRenderer = highlight.AddComponent<LineRenderer>();

                lineRenderer.startWidth = 0.05f;
                lineRenderer.endWidth = 0.05f;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
                lineRenderer.useWorldSpace = true;

                // Строим круг на основе коллайдера
                int segments = 100; // Количество сегментов круга для визуализации
                float radius = collider.radius * enemy.transform.lossyScale.x; // Учитываем масштаб объекта
                Vector3 center = collider.transform.position + (Vector3)collider.offset;

                Vector3[] points = GetCirclePoints(center, radius, segments);
                lineRenderer.positionCount = points.Length;
                lineRenderer.SetPositions(points);

                highlights.Add(highlight); // Добавляем подсветку в список
            }
        }
    }



    private void HideEnemyColliders()
    {
        foreach (var highlight in highlights)
        {
            Destroy(highlight); // Удаляем подсветку
        }
        highlights.Clear(); // Очищаем список
    }



    private Vector3[] GetCirclePoints(Vector3 center, float radius, int segments)
    {
        Vector3[] points = new Vector3[segments + 1];
        float angleStep = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * angleStep * i;
            float x = center.x + Mathf.Cos(angle) * radius;
            float y = center.y + Mathf.Sin(angle) * radius;
            points[i] = new Vector3(x, y, 0);
        }

        return points;
    }





}