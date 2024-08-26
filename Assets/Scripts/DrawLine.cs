using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer.positionCount = 2; // Устанавливаем количество вершин линии
        lineRenderer.startWidth = 0.1f; // Ширина линии в начале
        lineRenderer.endWidth = 0.1f; // Ширина линии в конце
    }

    void Update()
    {
        // Устанавливаем позиции вершин линии в пространстве
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        lineRenderer.SetPosition(1, new Vector3(1, 1, 1));
    }
}