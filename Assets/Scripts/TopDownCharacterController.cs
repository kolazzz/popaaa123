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

    [SerializeField] private float railgunShootCooldown = 0.2f; // Скорость стрельбы рельсотрона (настраивается в инспекторе)
    private float railgunShootTimer = 0f;                       // Таймер для стрельбы рельсотрона


    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bullet2Prefab;
    [SerializeField] private GameObject railgunBulletPrefab; // Префаб пули рельсотрона (зеленый)
    [SerializeField] private GameObject katanaBulletPrefab; // Префаб пули для катаны

    [Header("Katana Settings")]
    [SerializeField] private Sprite[] katanaAttackSprites; // Спрайты для анимации катаны
    [SerializeField] private float katanaAnimationSpeed = 0.1f; // Скорость анимации катаны

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource; // Источник звука
    [SerializeField] private AudioClip colliderActivateSound; // Звук активации коллайдеров

    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask damageLayerMask;

    [SerializeField] private TextMeshProUGUI weaponText;
    [SerializeField] private GameObject[] objectsToHighlight;

    private List<GameObject> highlights = new List<GameObject>(); // Список подсветок


    private Camera _mainCamera;

    [Header("Collider Visualization Settings")]
    [SerializeField] private string objectsTagToShowColliders = "Enemy"; // Тег объектов для отображения коллайдеров
    [SerializeField] public float colliderVisibilityDuration = 3f; // Длительность отображения
    private bool showColliders = false;
    private float colliderTimer = 0f;

    // Анимация через спрайты
    [Header("Animation Sprites")]
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private float animationSpeed = 0.1f;

    [Header("Scan Settings")]
    [SerializeField] private float scanCooldown = 5f; // Кулдаун для сканирования (указывается в инспекторе)
    private float scanCooldownTimer = 0f; // Таймер кулдауна

    private SpriteRenderer spriteRenderer;
    private float animationTimer;
    private int currentFrame;
    private bool isRunning;

    [Header("Dash Settings")]
    public float dashDuration = 3f;  // Длительность ускорения
    public float dashCooldownTime = 3f; // Кулдаун рывка
    private bool canDash = true;       // Флаг для контроля кулдауна

    [SerializeField] private AudioSource dashAudioSource; // Аудиоисточник для рывка
    [SerializeField] private AudioClip dashAudioClip;    // Аудиоклип для рывка

    [SerializeField] private AudioSource shieldAudioSource; // Аудиоисточник для щита
    [SerializeField] private AudioClip shieldAudioClip;     // Аудиоклип для активации щита

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
    private enum WeaponType { Regular, Shotgun, Railgun, Katana }
    private WeaponType currentWeapon = WeaponType.Regular;

    [SerializeField] private GameObject[] weaponPickups; // Массив объектов оружия для подбора

    // Параметры дробовика
    [Header("Shotgun Settings")]
    [SerializeField] private float shotgunSpreadAngle = 15f;  // Угол рассеивания пуль
    [SerializeField] private int shotgunPelletCount = 3; // Количество пуль, выстреливаемых дробовиком


    private bool isAttackingWithKatana = false;

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ПКМ = правая кнопка мыши
        {
            Debug.Log("ПКМ нажата!"); // Проверяем, ловится ли нажатие
        }


        if (Input.GetMouseButtonDown(1) && canDash) // Проверка нажатия ПКМ и кулдауна
        {
            Debug.Log("Буст запускается!"); // Проверка вызова
            StartCoroutine(TemporarySpeedBoost()); // Запускаем корутину
        }



        if (isDying || isTakingDamage) return; // Блокируем управление при проигрывании анимации

        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        isRunning = moveInput.magnitude > 0;

        UpdateAnimation();

        if (!isAttackingWithKatana)
        {
            RotateTowardsCursor();
        }

        // Стрельба в зависимости от оружия
        switch (currentWeapon)
        {
            case WeaponType.Regular:
                RegularShoot();
                break;
            case WeaponType.Shotgun:
                ShotgunShoot();
                break;
            case WeaponType.Railgun:
                RailgunShoot();
                break;
            case WeaponType.Katana:
                KatanaShoot();
                break;
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

        if (Input.GetKeyDown(KeyCode.R) && scanCooldownTimer <= 0f)
        {
            ShowEnemyColliders();
            colliderTimer = colliderVisibilityDuration; // Устанавливаем таймер
            scanCooldownTimer = scanCooldown; // Сброс таймера кулдауна
        }

        if (colliderTimer > 0f)
        {
            colliderTimer -= Time.deltaTime;

            if (colliderTimer <= 0f)
            {
                HideEnemyColliders();
            }
        }

        if (scanCooldownTimer > 0f)
        {
            scanCooldownTimer -= Time.deltaTime;
        }


    }


    public IEnumerator TemporarySpeedBoost()
    {
        canDash = false;

        float originalSpeed = moveSpeed; // Сохраняем текущую скорость
        moveSpeed *= 2f; // Увеличиваем скорость
        Debug.Log("Скорость увеличена до: " + moveSpeed);

        // Воспроизводим звук рывка
        if (dashAudioSource != null && dashAudioClip != null)
        {
            dashAudioSource.PlayOneShot(dashAudioClip);
        }

        // Запускаем кулдаун сразу параллельно
        StartCoroutine(DashCooldown());

        yield return new WaitForSeconds(3f); // Длительность ускорения

        moveSpeed = originalSpeed; // Возвращаем скорость к исходной
        Debug.Log("Скорость возвращена: " + moveSpeed);
    }

    private IEnumerator DashCooldown()
    {
        Debug.Log("Кулдаун запущен!");
        yield return new WaitForSeconds(8f); // Кулдаун длится 4 секунды
        Debug.Log("Кулдаун завершён.");
        canDash = true; // Снимаем кулдаун
    }





    private void CheckPickedWeapon(string weaponName)
    {
        // Сброс цвета для всех объектов
        foreach (GameObject obj in objectsToHighlight)
        {
            if (obj != null)
            {
                var renderer = obj.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.color = Color.white; // Сбрасываем цвет
                }
            }
        }

        // Изменяем текст и цвет в зависимости от оружия
        switch (weaponName)
        {
            case "Pistol":
                weaponText.text = "Pistol";
                break;
            case "Shotgun":
                weaponText.text = "Shotgun";
                HighlightObject(0);
                break;
            case "Railgun":
                weaponText.text = "Railgun";
                HighlightObject(1);
                break;
            case "Katana":
                weaponText.text = "Katana";
                HighlightObject(2);
                break;
            default:
                weaponText.text = "Unknown Weapon";
                break;
        }
    }

    private void HighlightObject(int index)
    {
        if (index >= 0 && index < objectsToHighlight.Length && objectsToHighlight[index] != null)
        {
            var renderer = objectsToHighlight[index].GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = Color.red; // Окрашиваем объект в красный
            }
        }
    }




    private void KatanaShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(PlayKatanaAttackAnimation());
            FireBullet(katanaBulletPrefab, true); // Передаём true для короткого расстояния
        }
    }


    private IEnumerator PlayKatanaAttackAnimation()
    {
        for (int i = 0; i < katanaAttackSprites.Length; i++)
        {
            spriteRenderer.sprite = katanaAttackSprites[i];
            yield return new WaitForSeconds(katanaAnimationSpeed);
        }
    }


    void FixedUpdate()
    {
        if (isDying || isTakingDamage) return; // Блокируем движение при проигрывании анимации
        rb.linearVelocity = moveInput.normalized * moveSpeed; // Учитываем текущее значение moveSpeed;
    }

    private void RegularShoot()
    {
        if (Input.GetMouseButtonDown(0) && currentShots < maxShots && regularShootTimer <= 0f)
        {
            FireBullet(bulletPrefab);
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

            for (int i = -1; i <= 1; i++)
            {
                Vector2 direction = (mousePosition - transform.position).normalized;
                float angleOffset = shotgunSpreadAngle * i;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
                Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, 0, angle));

                Instantiate(bullet2Prefab, firePoint.position, bulletRotation);
            }

            currentShots++;
            UpdateAmmoUI();

            shotgunShootTimer = shotgunShootCooldown;
        }
    }

    private void RailgunShoot()
    {
        if (Input.GetMouseButton(0) && currentShots < maxShots)
        {
            if (railgunShootTimer <= 0f)
            {
                FireBullet(railgunBulletPrefab);
                railgunShootTimer = railgunShootCooldown;
            }
        }

        if (railgunShootTimer > 0f)
        {
            railgunShootTimer -= Time.deltaTime;
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
        if (currentWeapon == WeaponType.Katana) return; // Убираем линию прицела для катаны

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

    public bool isInvincible = false; // Флаг неуязвимости
    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // Не получаем урон, если активен щит

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

        FreezeScene();

        if (gameCanvas != null)
        {
            gameCanvas.gameObject.SetActive(false);
        }

        yield return StartCoroutine(FadeInDarkness());

        for (int i = 0; i < deathSprites.Length; i++)
        {
            spriteRenderer.sprite = deathSprites[i];
            yield return new WaitForSeconds(deathAnimationSpeed);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void FreezeScene()
    {
        if (isSceneFrozen) return;

        isSceneFrozen = true;

        foreach (Rigidbody2D rb in FindObjectsOfType<Rigidbody2D>())
        {
            if (rb.gameObject != gameObject)
            {
                rb.linearVelocity = Vector2.zero;
                rb.isKinematic = true;
            }
        }

        foreach (MonoBehaviour script in FindObjectsOfType<MonoBehaviour>())
        {
            if (script.gameObject != gameObject)
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
        if (collision.gameObject.CompareTag("Shotgun"))
        {
            SwitchToShotgun();
            CheckPickedWeapon("Shotgun");
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Railgun"))
        {
            SwitchToRailgun();
            CheckPickedWeapon("Railgun");
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Katana"))
        {
            SwitchToKatana();
            CheckPickedWeapon("Katana");
            Destroy(collision.gameObject);
        }

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

    private void SwitchToRailgun()
    {
        currentWeapon = WeaponType.Railgun;
        Debug.Log("Switched to Railgun");

        // Устанавливаем 100 патронов
        currentShots = 0; // Сбрасываем текущие выстрелы
        maxShots = 100; // Обновляем максимальное количество патронов
        UpdateAmmoUI(); // Обновляем интерфейс
    }

    private void SwitchToKatana()
    {
        currentWeapon = WeaponType.Katana;
        Debug.Log("Switched to Katana");
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

    public void ShowEnemyColliders()
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

        // Проигрывание звука активации
        if (audioSource != null && colliderActivateSound != null)
        {
            audioSource.PlayOneShot(colliderActivateSound);
        }
    }




    public void HideEnemyColliders()
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




    private void FireBullet(GameObject bulletPrefab, bool isShortRange = false)
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

        // Если это короткая дистанция (например, катана)
        Vector3 spawnPosition = isShortRange
            ? transform.position + (Vector3)(direction * 0.001f) // Смещение ближе к персонажу
            : firePoint.position; // Обычное расположение

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, bulletRotation);

        if (isShortRange)
        {
            // Уменьшаем дальность, добавляя компонент для автоуничтожения пули
            Destroy(bullet, 0.3f); // Пуля исчезает через 0.2 секунды
        }

        currentShots++; // Уменьшение количества патронов
        UpdateAmmoUI();
    }









}