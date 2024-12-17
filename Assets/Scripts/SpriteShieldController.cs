
using System.Collections;
using UnityEngine;

public class SpriteShieldController : MonoBehaviour
{
    [Header("Shield Settings")]
    public Sprite[] shieldSprites;     // Массив спрайтов для анимации щита
    public float frameDuration = 0.1f; // Длительность кадра анимации
    public float shieldDuration = 3f;  // Длительность существования щита
    public GameObject shieldPrefab;    // Префаб щита (со SpriteRenderer)

    private GameObject currentShield;
    private SpriteRenderer shieldSpriteRenderer;
    private bool isShieldActive = false;

    private SpriteAnimationController animationController;

    private void Start()
    {
        animationController = GetComponent<SpriteAnimationController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isShieldActive) // Активация щита по клавише F
        {
            StartCoroutine(ActivateShield());
        }
    }

    private IEnumerator ActivateShield()
    {
        isShieldActive = true;

        // Создаём щит вокруг игрока
        currentShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);
        shieldSpriteRenderer = currentShield.GetComponent<SpriteRenderer>();

        // Проигрывание анимации щита
        for (int i = 0; i < shieldSprites.Length; i++)
        {
            shieldSpriteRenderer.sprite = shieldSprites[i];
            yield return new WaitForSeconds(frameDuration);
        }

        // Ожидание окончания щита
        yield return new WaitForSeconds(shieldDuration);

        // Отключение щита с анимацией назад
        for (int i = shieldSprites.Length - 1; i >= 0; i--)
        {
            shieldSpriteRenderer.sprite = shieldSprites[i];
            yield return new WaitForSeconds(frameDuration);
        }

        Destroy(currentShield);
        isShieldActive = false;
    }
}
