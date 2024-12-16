using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Fxicons : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject panel; // Панелька
    [SerializeField] private Image spriteImage; // Спрайт
    [SerializeField] private TextMeshProUGUI textMesh; // Текст "F"

    [Header("Cooldown Settings")]
    [SerializeField] private Color panelHighlightColor = Color.red; // Цвет панели при активации
    [SerializeField] private Color spriteHighlightColor = Color.yellow; // Цвет спрайта при активации
    [SerializeField] private Color textHighlightColor = Color.yellow; // Цвет текста при активации
    [SerializeField] private float cooldownDuration = 3f; // Длительность кулдауна
    [SerializeField] private KeyCode activationKey = KeyCode.F; // Клавиша активации

    private Color panelOriginalColor;
    private Color spriteOriginalColor;
    private Color textOriginalColor;

    private Image panelImage; // Компонент Image панели
    private bool isCooldownActive = false;

    private void Start()
    {
        // Получаем компонент Image панели
        panelImage = panel.GetComponent<Image>();

        // Сохраняем оригинальные цвета
        panelOriginalColor = panelImage.color;
        spriteOriginalColor = spriteImage.color;
        textOriginalColor = textMesh.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(activationKey) && !isCooldownActive)
        {
            StartCoroutine(StartCooldownEffect());
        }
    }

    private IEnumerator StartCooldownEffect()
    {
        isCooldownActive = true;

        // Устанавливаем цвета при активации
        panelImage.color = panelHighlightColor;
        spriteImage.color = spriteHighlightColor;
        textMesh.color = textHighlightColor;

        float timer = 0f;

        // Проходим кулдаун от 0 до cooldownDuration
        while (timer < cooldownDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / cooldownDuration;

            // Создаем градиент справа налево
            float leftFade = Mathf.Lerp(1, 0, progress);
            panelImage.color = Color.Lerp(panelHighlightColor, panelOriginalColor, progress);
            spriteImage.color = Color.Lerp(spriteHighlightColor, spriteOriginalColor, progress);
            textMesh.color = Color.Lerp(textHighlightColor, textOriginalColor, progress);

            // Обновляем маску или таймер справа налево (опционально можно использовать эффект через RectMask2D)
            panelImage.fillAmount = 1 - leftFade;
            panelImage.fillOrigin = (int)Image.OriginHorizontal.Right; // Направление слева направо

            yield return null;
        }

        // Возвращаем оригинальные цвета
        panelImage.color = panelOriginalColor;
        spriteImage.color = spriteOriginalColor;
        textMesh.color = textOriginalColor;
        panelImage.fillAmount = 1f;

        isCooldownActive = false;
    }
}
