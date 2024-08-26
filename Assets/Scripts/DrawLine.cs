using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer.positionCount = 2; // ������������� ���������� ������ �����
        lineRenderer.startWidth = 0.1f; // ������ ����� � ������
        lineRenderer.endWidth = 0.1f; // ������ ����� � �����
    }

    void Update()
    {
        // ������������� ������� ������ ����� � ������������
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        lineRenderer.SetPosition(1, new Vector3(1, 1, 1));
    }
}