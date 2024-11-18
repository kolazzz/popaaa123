using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopDownCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask damageLayerMask;

    private Camera _mainCamera;

    // Анимация через спрайты
    [Header("Animation Sprites")]
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private float animationSpeed = 0.1f;

    private SpriteRenderer spriteRenderer;
    private float animationTimer;
    private int currentFrame;
    private bool isRunning;

    // Линия прицела
    [Header("Aim Line Settings")]
    [SerializeField] private Color aimLineColor = Color.red; // Цвет линии прицела
    [SerializeField] private float aimLineMaxLength = 5f;    // Максимальная длина линии прицела
    private LineRenderer aimLine;

    // Система здоровья
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 4; // Максимальное здоровье
    private int currentHealth; // Текущее здоровье

    // Лимит на количество выстрелов
    [Header("Shooting Settings")]
    [SerializeField] private int maxShots = 15;  // Максимальное количество выстрелов
    private int currentShots; // Текущее количество выстрелов

    // UI для патронов
    [Header("Ammo Settings")]
    [SerializeField] private TextMeshProUGUI ammoText;  // Ссылка на TextMeshPro для отображения количества патронов
    [SerializeField] private SpriteRenderer ammoSpriteRenderer; // Ссылка на спрайт для бар патронов
    [SerializeField] private Sprite[] ammoSprites; // Массив спрайтов для патронов

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Создание компонента LineRenderer для прицела
        aimLine = gameObject.AddComponent<LineRenderer>();
        aimLine.startWidth = 0.05f;
        aimLine.endWidth = 0.05f;
        aimLine.material = new Material(Shader.Find("Sprites/Default"));
        aimLine.startColor = aimLineColor;
        aimLine.endColor = aimLineColor;

        // Инициализация здоровья
        currentHealth = maxHealth;

        // Инициализация количества выстрелов
        currentShots = 0;

        // Инициализация UI патронов
        UpdateAmmoUI();
    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        isRunning = moveInput.magnitude > 0;

        UpdateAnimation();
        RotateTowardsCursor();

        if (!isRunning)
        {
            Shoot();
        }

        UpdateAimLine();

        if (_mainCamera != null)
        {
            _mainCamera.transform.rotation = Quaternion.identity;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && currentShots < maxShots)  // Проверка на лимит выстрелов
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector2 direction = mousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            Instantiate(bulletPrefab, firePoint.position, bulletRotation);

            // Увеличиваем количество выстрелов
            currentShots++;

            // Обновляем UI патронов
            UpdateAmmoUI();
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
        // Обновляем текст с количеством патронов
        ammoText.text = (maxShots - currentShots).ToString();

        // Сменить спрайт бара с патронами каждые 3 выстрела
        int ammoIndex = (currentShots / 3) % ammoSprites.Length;
        ammoSpriteRenderer.sprite = ammoSprites[ammoIndex];
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ApplyKnockback(Vector2 direction, float force = 1500f)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public int GetCurrentHealth()
    {
        return currentHealth; // Возвращаем текущее здоровье игрока
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMaskUtil.ContainsLayer(damageLayerMask, collision.gameObject))
        {
            // Враг наносит урон игроку
            TakeDamage(1);
        }
    }
}
