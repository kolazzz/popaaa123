using UnityEngine;

public class PortalAnimation : MonoBehaviour
{
    [Header("Portal Animation Settings")]
    [SerializeField] private Sprite[] portalSprites;  // ������ �������� ��� ��������
    [SerializeField] private float animationSpeed = 0.1f;  // �������� ��������

    private SpriteRenderer spriteRenderer;
    private float animationTimer;
    private int currentFrame;
    private bool isAnimationComplete = false;  // ���� ���������� ��������

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
          // �������� ������ �� ���������
    }

    void Update()
    {
        if (!isAnimationComplete)
        {
            AnimatePortal();  // ��������� ��������, ���� ��� �� ���������
        }
    }

    /// <summary>
    /// ��������� �������� ������� � ������.
    /// </summary>
    public void ActivatePortal()
    {
        spriteRenderer.enabled = true;  // ���������� ������
        currentFrame = 0;
        animationTimer = 0f;
        isAnimationComplete = false;

        if (portalSprites.Length > 0)
        {
            spriteRenderer.sprite = portalSprites[currentFrame];  // ������������� ������ ����
        }
    }

    /// <summary>
    /// ������������ �������� �������.
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

            currentFrame++;  // ������� � ���������� �����

            if (currentFrame >= portalSprites.Length)
            {
                // ���� �������� ����� �������, ��������� ��������� ����
                currentFrame = portalSprites.Length - 1;
                isAnimationComplete = true;  // �������� ���������� ��������
            }

            // ������������� ������� ���� � ������
            if (currentFrame >= 0 && currentFrame < portalSprites.Length)
            {
                spriteRenderer.sprite = portalSprites[currentFrame];
            }
        }
    }
}
