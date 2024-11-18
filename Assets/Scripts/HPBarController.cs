using UnityEngine;

public class HPBarController : MonoBehaviour
{
    [Header("Ссылки на спрайты")]
    public Sprite[] hpSprites; // Массив спрайтов для HP

    [Header("Ссылка на SpriteRenderer HP-бара")]
    public SpriteRenderer spriteRenderer; // Ссылка на компонент SpriteRenderer

    private TopDownCharacterController playerController;

    void Start()
    {
        playerController = FindObjectOfType<TopDownCharacterController>(); // Находим объект игрока
        if (playerController == null)
        {
            Debug.LogError("Игрок не найден!");
        }
    }

    void Update()
    {
        if (playerController != null)
        {
            // Обновляем спрайт HP-бара в зависимости от текущего здоровья игрока
            UpdateHPBar();
        }
    }

    private void UpdateHPBar()
    {
        // Получаем текущее здоровье игрока и обновляем спрайт HP-бара
        int currentHealth = playerController.GetCurrentHealth();
        if (currentHealth >= 1 && currentHealth <= hpSprites.Length)
        {
            spriteRenderer.sprite = hpSprites[currentHealth - 1];
        }
        else if (currentHealth <= 0)
        {
            spriteRenderer.sprite = hpSprites[0]; // Если здоровье 0 или меньше, показываем самый низкий спрайт
        }
    }
}
