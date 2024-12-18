using UnityEngine;

public class portals : MonoBehaviour
{
    [Header("Idle Animation Settings")]
    [SerializeField] private Sprite[] idleSprites;  // ������ ������ ��������
    [SerializeField] private Sprite[] secondAnimationSprites; // ������ ������ ��������
    [SerializeField] private float animationSpeed = 0.1f;  // �������� ��������

    private SpriteRenderer spriteRenderer;
    private float animationTimer;
    private int currentFrame;
    private bool isPlayingSecondAnimation = false; // ���� ��� ����������� ������� ��������

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
        // ���� �������� ������ ��������, ���������� ������ �
        Sprite[] currentSprites = isPlayingSecondAnimation ? secondAnimationSprites : idleSprites;

        if (currentSprites == null || currentSprites.Length == 0)
            return;

        animationTimer += Time.unscaledDeltaTime; // ���������� unscaledDeltaTime
        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            currentFrame++;

            // ���� ������� �������� ���������
            if (currentFrame >= currentSprites.Length)
            {
                currentFrame = 0;

                // ������� �� ������ ��������, ���� ������� ������
                if (!isPlayingSecondAnimation)
                {
                    isPlayingSecondAnimation = true;
                }
            }

            spriteRenderer.sprite = currentSprites[currentFrame];
        }
    }
}