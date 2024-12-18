using UnityEngine;

public class portals : MonoBehaviour
{
    [Header("Idle Animation Settings")]
    [SerializeField] private Sprite[] idleSprites;  // Первый массив спрайтов
    [SerializeField] private Sprite[] secondAnimationSprites; // Второй массив спрайтов
    [SerializeField] private float animationSpeed = 0.1f;  // Скорость анимации

    private SpriteRenderer spriteRenderer;
    private float animationTimer;
    private int currentFrame;
    private bool isPlayingSecondAnimation = false; // Флаг для определения текущей анимации

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        AnimateIdle();
    }

    private void AnimateIdle()
    {
        // Если включена вторая анимация, используем только её
        Sprite[] currentSprites = isPlayingSecondAnimation ? secondAnimationSprites : idleSprites;

        if (currentSprites == null || currentSprites.Length == 0)
            return;

        animationTimer += Time.unscaledDeltaTime; // Используем unscaledDeltaTime
        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            currentFrame++;

            // Если текущая анимация завершена
            if (currentFrame >= currentSprites.Length)
            {
                currentFrame = 0;

                // Переход на вторую анимацию, если текущая первая
                if (!isPlayingSecondAnimation)
                {
                    isPlayingSecondAnimation = true;
                }
            }

            spriteRenderer.sprite = currentSprites[currentFrame];
        }
    }
}