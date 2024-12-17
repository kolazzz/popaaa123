
using UnityEngine;

public class SpriteSwitcherS : MonoBehaviour
{
    [Header("Sprite Settings")]
    [SerializeField] private Sprite newSprite; // Новый спрайт
    [SerializeField] private SpriteRenderer spriteRenderer; // Ссылка на SpriteRenderer объекта
    [SerializeField] private KeyCode switchKey = KeyCode.Space; // Клавиша для переключения

    private Sprite originalSprite; // Исходный спрайт
    private bool isSwitched = false;

    private void Start()
    {
        // Получаем SpriteRenderer, если не задан в инспекторе
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Сохраняем исходный спрайт
        if (spriteRenderer != null)
        {
            originalSprite = spriteRenderer.sprite;
        }
    }

    private void Update()
    {
        // Проверяем нажатие клавиши
        if (Input.GetKeyDown(switchKey) && !isSwitched)
        {
            StartCoroutine(SwitchSpriteForTime(6f));
        }
    }

    private System.Collections.IEnumerator SwitchSpriteForTime(float duration)
    {
        if (spriteRenderer == null || newSprite == null) yield break;

        isSwitched = true;

        // Переключаем спрайт
        spriteRenderer.sprite = newSprite;

        // Ждём указанное время
        yield return new WaitForSeconds(duration);

        // Возвращаем исходный спрайт
        spriteRenderer.sprite = originalSprite;

        isSwitched = false;
    }
}
