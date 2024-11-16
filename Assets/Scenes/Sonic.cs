using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sonic : MonoBehaviour
{
    [Header("Целевой объект для отслеживания")]
    [Tooltip("Объект, активация которого запускает процесс")]
    public GameObject targetObject;

    [Header("Список объектов для активации")]
    [Tooltip("Список объектов, которые нужно включить")]
    public List<GameObject> objectsToActivate = new List<GameObject>();

    [Header("Настройки времени")]
    [Tooltip("Задержка между включением объектов (в секундах)")]
    public float activationDelay = 1f;

    [Header("Настройки аудио")]
    [Tooltip("Аудио источник для воспроизведения клипа")]
    public AudioSource audioSource;

    [Tooltip("Аудиоклип, который будет проигрываться при активации Target Object")]
    public AudioClip activationClip;

    private bool isActivating = false;

    void Update()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object не назначен в инспекторе!");
            return;
        }

        // Проверяем, включён ли целевой объект, и запускаем процесс
        if (targetObject.activeSelf && !isActivating)
        {
            StopAllMusic(); // Выключить всю музыку
            PlayActivationClip(); // Включить указанный аудиоклип
            StartCoroutine(ActivateObjectsSequentially());
        }
    }

    private void StopAllMusic()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (var source in allAudioSources)
        {
            source.Stop();
        }
    }

    private void PlayActivationClip()
    {
        if (audioSource != null && activationClip != null)
        {
            audioSource.clip = activationClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource или ActivationClip не назначены!");
        }
    }

    private IEnumerator ActivateObjectsSequentially()
    {
        isActivating = true;

        for (int i = 0; i < objectsToActivate.Count; i++)
        {
            var obj = objectsToActivate[i];
            if (obj != null)
            {
                obj.SetActive(true);
                yield return new WaitForSeconds(activationDelay);
            }

            // Проверяем, последний ли это объект
            if (i == objectsToActivate.Count - 1)
            {
                ReloadScene();
            }
        }

        isActivating = false;
    }

    private void ReloadScene()
    {
        Debug.Log("Все объекты активированы. Перезагрузка сцены...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
