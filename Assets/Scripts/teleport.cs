using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderOnTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string sceneNameToLoad; // Имя сцены, которую нужно загрузить

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Проверяем, если в триггер вошёл объект с тегом "Player"
        {
            LoadScene();
        }
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneNameToLoad))
        {
            SceneManager.LoadScene(sceneNameToLoad);
        }
        else
        {
            Debug.LogWarning("Scene name to load is not set!");
        }
    }
}