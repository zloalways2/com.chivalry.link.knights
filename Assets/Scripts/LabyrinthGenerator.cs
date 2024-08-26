using UnityEngine;

public class LabyrinthGenerator : MonoBehaviour
{
    public Texture2D labyrinthImage; // Ссылка на изображение лабиринта
    public GameObject wallPrefab; // Префаб стены лабиринта
    public float wallSize; // Размер стены

    void Start()
    {
        // Получение размеров изображения
        int width = labyrinthImage.width;
        int height = labyrinthImage.height;

        // Создание лабиринта
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Проверка цвета пикселя
                Color pixelColor = labyrinthImage.GetPixel(x, y);

                // Создание стены, если пиксель черный
                if (pixelColor.r < 0.5f)
                {
                    // Создание экземпляра стены
                    GameObject wall = Instantiate(wallPrefab, new Vector2(x * wallSize, y * wallSize), Quaternion.identity);
                    // Установка размеров стены
                    wall.transform.localScale = new Vector2(wallSize, wallSize);
                }
            }
        }
    }
}