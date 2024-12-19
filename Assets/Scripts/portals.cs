using UnityEngine;

public class PortalAnimation : MonoBehaviour
{
    [Header("Portal Animation Settings")]
    [SerializeField] private Sprite[] portalSprites;  // Массив спрайтов для анимации
    [SerializeField] private float animationSpeed = 0.1f;  // Скорость анимации

    private SpriteRenderer spriteRenderer;
    private float animationTimer;
    private int currentFrame;
    private bool isAnimationComplete = false;  // Флаг завершения анимации

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
          // Скрываем портал до активации
    }

    void Update()
    {
        if (!isAnimationComplete)
        {
            AnimatePortal();  // Обновляем анимацию, пока она не завершена
        }
    }

    /// <summary>
    /// Запускает анимацию портала с начала.
    /// </summary>
    public void ActivatePortal()
    {
        spriteRenderer.enabled = true;  // Отображаем портал
        currentFrame = 0;
        animationTimer = 0f;
        isAnimationComplete = false;

        if (portalSprites.Length > 0)
        {
            spriteRenderer.sprite = portalSprites[currentFrame];  // Устанавливаем первый кадр
        }
    }

    /// <summary>
    /// Обрабатывает анимацию портала.
    /// </summary>
    private void AnimatePortal()
    {
        if (portalSprites == null || portalSprites.Length == 0)
        {
            Debug.LogWarning("Portal sprites array is empty or null.");
            return;
        }

        animationTimer += Time.deltaTime;

        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;

            currentFrame++;  // Переход к следующему кадру

            if (currentFrame >= portalSprites.Length)
            {
                // Если достигли конца массива, фиксируем последний кадр
                currentFrame = portalSprites.Length - 1;
                isAnimationComplete = true;  // Отмечаем завершение анимации
            }

            // Устанавливаем текущий кадр в спрайт
            if (currentFrame >= 0 && currentFrame < portalSprites.Length)
            {
                spriteRenderer.sprite = portalSprites[currentFrame];
            }
        }
    }
}
