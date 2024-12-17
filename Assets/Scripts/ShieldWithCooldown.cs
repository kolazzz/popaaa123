using System.Collections;
using UnityEngine;
using TMPro;

public class ShieldWithAnimation : MonoBehaviour
{
    [Header("Shield Animation Settings")]
    public Sprite[] sprites;                  // Анимация щита
    public float frameDuration = 0.1f;        // Длительность кадра
    public float shieldDuration = 3f;         // Длительность щита
    public float cooldownDuration = 10f;      // Кулдаун щита

    [Header("Cooldown Settings")]
    public GameObject objectA;                // Отключаем анимацию на объекте A
    public GameObject objectB;                // Включаем объект B во время КД щита
    public TextMeshProUGUI cooldownText;      // Текст кулдауна щита

    [Header("Scan Settings")]
    public float scanCooldownDuration = 5f;   // Кулдаун скана
    public GameObject objectC;                // Включается во время кулдауна скана
    public TextMeshProUGUI scanCooldownText;  // Текст для обратного отсчета скана

    [Header("Dash Settings")]
    public GameObject objectD;                // Включается во время кулдауна рывка
    public TextMeshProUGUI dashCooldownText;  // Текст для обратного отсчета рывка

    private SpriteRenderer spriteRenderer;
    private Collider2D shieldCollider;
    private bool isPlaying = false;
    private bool isOnCooldown = false;
    private bool isScanOnCooldown = false;
    private bool isDashOnCooldown = false;

    private TopDownCharacterController controller;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        shieldCollider = GetComponentInChildren<Collider2D>();
        controller = GetComponentInParent<TopDownCharacterController>();

        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (shieldCollider != null) shieldCollider.enabled = false;

        if (objectB != null) objectB.SetActive(false);
        if (cooldownText != null) cooldownText.gameObject.SetActive(false);

        if (objectC != null) objectC.SetActive(false);
        if (scanCooldownText != null) scanCooldownText.gameObject.SetActive(false);

        if (objectD != null) objectD.SetActive(false);
        if (dashCooldownText != null) dashCooldownText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isPlaying && !isOnCooldown)
        {
            StartCoroutine(ActivateShield());
        }

        if (Input.GetKeyDown(KeyCode.R) && !isScanOnCooldown)
        {
            StartCoroutine(ScanWithCooldown());
        }

        if (Input.GetMouseButtonDown(1) && !isDashOnCooldown)
        {
            
            StartCoroutine(DashWithCooldown());
        }
    }

    private IEnumerator ActivateShield()
    {
        isPlaying = true;
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (shieldCollider != null) shieldCollider.enabled = true;

        controller.isInvincible = true;

        for (int i = 0; i < sprites.Length; i++)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(frameDuration);
        }

        yield return new WaitForSeconds(shieldDuration);

        for (int i = sprites.Length - 1; i >= 0; i--)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(frameDuration);
        }

        controller.isInvincible = false;

        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (shieldCollider != null) shieldCollider.enabled = false;

        isPlaying = false;
        StartCoroutine(ShieldCooldown());
    }

    private IEnumerator ShieldCooldown()
    {
        isOnCooldown = true;
        if (objectA != null) objectA.SetActive(false);
        if (objectB != null) objectB.SetActive(true);

        if (cooldownText != null)
        {
            cooldownText.gameObject.SetActive(true);
            for (float timer = cooldownDuration; timer > 0; timer--)
            {
                cooldownText.text = Mathf.Ceil(timer).ToString();
                yield return new WaitForSeconds(1f);
            }
            cooldownText.gameObject.SetActive(false);
        }

        if (objectA != null) objectA.SetActive(true);
        if (objectB != null) objectB.SetActive(false);

        isOnCooldown = false;
    }

    private IEnumerator ScanWithCooldown()
    {
        isScanOnCooldown = true;

        controller.ShowEnemyColliders();
        yield return new WaitForSeconds(controller.colliderVisibilityDuration);
        controller.HideEnemyColliders();

        if (objectC != null) objectC.SetActive(true);

        if (scanCooldownText != null)
        {
            scanCooldownText.gameObject.SetActive(true);
            for (float timer = scanCooldownDuration; timer > 0; timer--)
            {
                scanCooldownText.text = Mathf.Ceil(timer).ToString();
                yield return new WaitForSeconds(1f);
            }
            scanCooldownText.gameObject.SetActive(false);
        }

        if (objectC != null) objectC.SetActive(false);

        isScanOnCooldown = false;
    }

    private IEnumerator DashWithCooldown()
    {
        isDashOnCooldown = true;

        // Запускаем рывок через контроллер
        if (controller != null)
        {
            controller.TemporarySpeedBoost();
        }

        if (objectD != null) objectD.SetActive(true);

        if (dashCooldownText != null)
        {
            dashCooldownText.gameObject.SetActive(true);
            for (float timer = controller.dashCooldownTime; timer > 0; timer--)
            {
                dashCooldownText.text = Mathf.Ceil(timer).ToString();
                yield return new WaitForSeconds(1f);
            }
            dashCooldownText.gameObject.SetActive(false);
        }

        if (objectD != null) objectD.SetActive(false);

        isDashOnCooldown = false;
    }


}
