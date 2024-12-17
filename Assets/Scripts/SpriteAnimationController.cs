
using System.Collections;
using UnityEngine;

public class SpriteAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    public Sprite[] sprites;            // Массив спрайтов для анимации
    public float frameDuration = 0.1f;  // Длительность кадра
    public float reverseDelay = 2f;     // Задержка перед обратным проигрыванием

    private SpriteRenderer spriteRenderer;
    private bool isPlaying = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false; // Спрятать спрайт до активации
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isPlaying) // Клавиша для запуска анимации
        {
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        isPlaying = true;
        spriteRenderer.enabled = true;

        // Проигрывание вперёд
        for (int i = 0; i < sprites.Length; i++)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(frameDuration);
        }

        // Ожидание перед проигрыванием задом наперёд
        yield return new WaitForSeconds(reverseDelay);

        // Проигрывание назад
        for (int i = sprites.Length - 1; i >= 0; i--)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(frameDuration);
        }

        // Спрятать спрайт и завершить анимацию
        spriteRenderer.enabled = false;
        isPlaying = false;
    }
}
