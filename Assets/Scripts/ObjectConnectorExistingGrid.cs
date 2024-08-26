using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectConnectorExistingGrid : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private GameObject startObject;
    private bool isDrawing = false;

    // Массив всех объектов сетки
    public GameObject[] gridObjects;

    // ID, по которым будем соединять объекты
    public int objectID;

    // Список точек, через которые проходит линия
    private List<Vector3> linePoints = new List<Vector3>();

    // Список всех построенных линий
    private List<List<Vector3>> allLines = new List<List<Vector3>>();

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Инициализация объектов сетки
        if (gridObjects == null || gridObjects.Length == 0)
        {
            gridObjects = GameObject.FindGameObjectsWithTag("Connectable");
        }
    }

    void Update()
    {
        // Начало касания
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Устанавливаем Z в 0 для 2D
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Connectable") && CheckObjectID(hit.collider.gameObject))
            {
                startObject = hit.collider.gameObject;
                lineRenderer.positionCount = 1;
                lineRenderer.SetPosition(0, startObject.transform.position);
                linePoints.Add(startObject.transform.position);
                MarkObjectAsOccupied(startObject); // Помечаем начальный объект как занятый
                isDrawing = true;
            }
        }

        // Рисование линии через объекты сетки
        if (isDrawing && Input.GetMouseButton(0))
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPos.z = 0;  // Устанавливаем Z в 0 для 2D
            GameObject closestObject = GetClosestGridObject(currentPos);

            if (closestObject != null && !IsObjectOccupied(closestObject) && !IsObjectBehindAnyLine(closestObject.transform.position)) // Проверка, что объект не занят и не за линией
            {
                // Проверка на пересечение с уже построенными линиями
                if (!IsIntersectingWithAnyLine(startObject.transform.position, closestObject.transform.position))
                {
                    int nextPointIndex = lineRenderer.positionCount;
                    lineRenderer.positionCount = nextPointIndex + 1;
                    lineRenderer.SetPosition(nextPointIndex, closestObject.transform.position);
                    linePoints.Add(closestObject.transform.position);
                    MarkObjectAsOccupied(closestObject); // Помечаем объект как занятый

                    // Проверка пересечения с другим объектом
                    RaycastHit2D hit = Physics2D.Raycast(closestObject.transform.position, Vector2.zero);

                    if (hit.collider != null && hit.collider.gameObject != startObject && CheckObjectID(hit.collider.gameObject))
                    {
                        lineRenderer.SetPosition(nextPointIndex, closestObject.transform.position); // Привязка конца линии к ближайшему объекту
                        allLines.Add(new List<Vector3>(linePoints));// Сохраняем линию в списке всех линий
                        //Debug.Log("allLinesadd");
                        isDrawing = false;
                    }
                }
            }
        }

        // Завершение касания
        if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            ClearLineAndUnmarkObjects(); // Очистка линии и снятие отметки занятости с объектов
            isDrawing = false;
        }
    }

    // Метод для нахождения ближайшего объекта в сетке
    private GameObject GetClosestGridObject(Vector3 position)
    {
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in gridObjects)
        {
            float distance = Vector3.Distance(position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        return closestObject;
    }

    // Метод для проверки ID объекта
    private bool CheckObjectID(GameObject obj)
    {
        ObjectIDComponent idComponent = obj.GetComponent<ObjectIDComponent>();
        if (idComponent != null && idComponent.ID == objectID)
        {
            return true;
        }
        return false;
    }

    // Метод для пометки объекта как занятого
    private void MarkObjectAsOccupied(GameObject obj)
    {
        obj.AddComponent<OccupiedMarker>(); // Добавляем компонент, который помечает объект как занятый
    }

    // Метод для снятия отметки занятости объекта
    private void UnmarkObjectAsOccupied(GameObject obj)
    {
        OccupiedMarker marker = obj.GetComponent<OccupiedMarker>();
        if (marker != null)
        {
            Destroy(marker); // Удаляем компонент, помечающий объект как занятый
        }
    }

    // Метод для снятия отметок "занятости" со всех объектов и очистки линии
    private void ClearLineAndUnmarkObjects()
    {
        // Снятие отметки занятости со всех объектов, через которые проходила линия
        foreach (Vector3 point in linePoints)
        {
            GameObject obj = FindObjectAtPosition(point);
            if (obj != null)
            {
                UnmarkObjectAsOccupied(obj);
            }
        }

        // Очистка линии
        lineRenderer.positionCount = 0;
        linePoints.Clear();
        //Debug.Log("linePoints.Clear()");
    }

    // Метод для поиска объекта по позиции
    private GameObject FindObjectAtPosition(Vector3 position)
    {
        foreach (GameObject obj in gridObjects)
        {
            if (Vector3.Distance(obj.transform.position, position) < 0.1f)
            {
                return obj;
            }
        }
        return null;
    }

    // Метод для проверки, занят ли объект
    private bool IsObjectOccupied(GameObject obj)
    {
        return obj.GetComponent<OccupiedMarker>() != null; // Проверяем, есть ли на объекте компонент OccupiedMarker
    }

    // Метод для проверки, находится ли объект за любой существующей линией
    private bool IsObjectBehindAnyLine(Vector3 objectPosition)
    {
        if (linePoints.Count < 2) return false;

        for (int i = 1; i < linePoints.Count; i++)
        {
            Vector3 lineStart = linePoints[i - 1];
            Vector3 lineEnd = linePoints[i];

            Vector3 lineDirection = (lineEnd - lineStart).normalized;
            Vector3 toObjectDirection = (objectPosition - lineEnd).normalized;

            // Проверка, находится ли объект за уже построенной линией
            if (Vector3.Dot(lineDirection, toObjectDirection) < 0)
            {
                return true;
            }
        }

        return false;
    }

    // Метод для проверки, пересекается ли новая линия с уже построенными
    private bool IsIntersectingWithAnyLine(Vector3 start, Vector3 end)
    {
        foreach (List<Vector3> line in allLines)
        {
            for (int i = 1; i < line.Count; i++)
            {
                Vector3 lineStart = line[i - 1];
                Vector3 lineEnd = line[i];

                if (IsLinesIntersecting(start, end, lineStart, lineEnd))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Метод для проверки пересечения двух линий
    private bool IsLinesIntersecting(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
    {
        float x1 = start1.x, y1 = start1.y;
        float x2 = end1.x, y2 = end1.y;
        float x3 = start2.x, y3 = start2.y;
        float x4 = end2.x, y4 = end2.y;

        float denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
        if (denominator == 0)
        {
            return false; // Линии параллельны
        }

        float intersectX = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / denominator;
        float intersectY = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / denominator;

        return IsPointOnSegment(intersectX, intersectY, x1, y1, x2, y2) && IsPointOnSegment(intersectX, intersectY, x3, y3, x4, y4);
    }

    // Метод для проверки, находится ли точка на отрезке
    private bool IsPointOnSegment(float px, float py, float x1, float y1, float x2, float y2)
    {
        return px >= Mathf.Min(x1, x2) && px <= Mathf.Max(x1, x2) && py >= Mathf.Min(y1, y2) && py <= Mathf.Max(y1, y2);
    }
}
