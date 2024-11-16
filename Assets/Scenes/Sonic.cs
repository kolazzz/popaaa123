using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sonic : MonoBehaviour
{
    [Header("������� ������ ��� ������������")]
    [Tooltip("������, ��������� �������� ��������� �������")]
    public GameObject targetObject;

    [Header("������ �������� ��� ���������")]
    [Tooltip("������ ��������, ������� ����� ��������")]
    public List<GameObject> objectsToActivate = new List<GameObject>();

    [Header("��������� �������")]
    [Tooltip("�������� ����� ���������� �������� (� ��������)")]
    public float activationDelay = 1f;

    [Header("��������� �����")]
    [Tooltip("����� �������� ��� ��������������� �����")]
    public AudioSource audioSource;

    [Tooltip("���������, ������� ����� ������������� ��� ��������� Target Object")]
    public AudioClip activationClip;

    private bool isActivating = false;

    void Update()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object �� �������� � ����������!");
            return;
        }

        // ���������, ������� �� ������� ������, � ��������� �������
        if (targetObject.activeSelf && !isActivating)
        {
            StopAllMusic(); // ��������� ��� ������
            PlayActivationClip(); // �������� ��������� ���������
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
            Debug.LogWarning("AudioSource ��� ActivationClip �� ���������!");
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

            // ���������, ��������� �� ��� ������
            if (i == objectsToActivate.Count - 1)
            {
                ReloadScene();
            }
        }

        isActivating = false;
    }

    private void ReloadScene()
    {
        Debug.Log("��� ������� ������������. ������������ �����...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
