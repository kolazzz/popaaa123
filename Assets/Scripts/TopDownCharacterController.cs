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
    }

    void Update()
    {
        // Получение ввода для перемещения
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        // Определение, находится ли персонаж в движении
        isRunning = moveInput.magnitude > 0;

        // Обновление анимации
        UpdateAnimation();

        // Поворот персонажа в зависимости от позиции курсора
        RotateTowardsCursor();

        // Стрельба
        if (!isRunning)
        {
            Shoot();
        }

        // Обновление линии прицела
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector2 direction = mousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            Instantiate(bulletPrefab, firePoint.position, bulletRotation);
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

        // Определение направления и ограничение длины линии прицела
        Vector2 direction = (mousePosition - transform.position).normalized;
        Vector3 aimEndPoint = transform.position + (Vector3)direction * aimLineMaxLength;

        aimLine.SetPosition(0, transform.position); // Начальная точка линии - позиция персонажа
        aimLine.SetPosition(1, aimEndPoint);        // Конечная точка линии - ограниченная длиной
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMaskUtil.ContainsLayer(damageLayerMask, collision.gameObject))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
