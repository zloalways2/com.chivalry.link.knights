using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawLineBetweenButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public LineRenderer lineRenderer;
    public Button[] buttons;

    private bool isDrawing = false;
    private bool isOverButton = false;

    void Update()
    {
        if (isDrawing && isOverButton)
        {
            Vector3[] positions = new Vector3[buttons.Length + 1];
            positions[0] = Input.mousePosition;
            for (int i = 0; i < buttons.Length; i++)
            {
                positions[i + 1] = buttons[i].transform.position;
            }
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOverButton = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOverButton = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isOverButton)
        {
            isDrawing = true;
            lineRenderer.positionCount = 0;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrawing = false;
        lineRenderer.positionCount = 0;
    }
}