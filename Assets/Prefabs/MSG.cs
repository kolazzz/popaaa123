using UnityEngine;

public class SequentialObjectActivatorWithReset : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToActivate; // Массив объектов для включения
    private int currentIndex = 0; // Индекс текущего объекта в массиве
    private const int chunkSize = 4; // Количество объектов в одной группе (например, 4)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivateNextObject();
        }
    }

    private void ActivateNextObject()
    {
        if (currentIndex < objectsToActivate.Length)
        {
            // Определяем, достигли ли конца текущего блока
            if (currentIndex % chunkSize == 0 && currentIndex > 0)
            {
                ResetPreviousChunk();
            }

            // Включаем текущий объект
            objectsToActivate[currentIndex].SetActive(true);
            currentIndex++;
        }
    }

    private void ResetPreviousChunk()
    {
        // Вычисляем начало и конец предыдущего блока
        int startIndex = Mathf.Max(0, currentIndex - chunkSize);
        int endIndex = Mathf.Min(currentIndex, objectsToActivate.Length);

        // Выключаем объекты предыдущего блока
        for (int i = startIndex; i < endIndex; i++)
        {
            objectsToActivate[i].SetActive(false);
        }
    }
}
