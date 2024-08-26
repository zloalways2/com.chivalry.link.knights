using System.Collections.Generic;
using UnityEngine;

public class ObjectConnectorManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _toDeactivate;
    [SerializeField] private List<GameObject> _toActivate;
    [SerializeField] private List<ObjectIDComponent> _component;

    // Новый список для хранения всех линий
    private List<LineRenderer> _lineRenderers = new List<LineRenderer>();

    private bool IsWinner = false;

    private void Awake()
    {
        _toActivate = new List<GameObject>();
        SceneManager sceneManager = GetComponentInParent<SceneManager>();
        _toDeactivate.Add(sceneManager.Levels);
        _toActivate.Add(sceneManager.Winner);
    }

    private void Update()
    {
        CheckAllConnected();
        ChangeBlock();
    }

    private void CheckAllConnected()
    {
        if (IsWinner) return;
        foreach (ObjectIDComponent component in _component)
        {
            if (!component.isConnected)
            {
                return;
            }
        }

        IsWinner = true;
    }

    private void ChangeBlock()
    {
        if (IsWinner)
        {
            //Debug.Log("WIN-2");
            foreach (GameObject block in _toDeactivate)
            {
                block.SetActive(false);
            }
            foreach (GameObject block in _toActivate)
            {
                block.SetActive(true);
            }
            IsWinner = false;
            ResetAllConnections();
        }
    }

    // Метод для добавления линии в список
    public void AddLine(LineRenderer lineRenderer)
    {
        if (!_lineRenderers.Contains(lineRenderer))
        {
            _lineRenderers.Add(lineRenderer);
            //Debug.Log("AddLine" + _lineRenderers.Count);
        }
    }

    // Метод для удаления линии из списка
    public void RemoveLine(LineRenderer lineRenderer)
    {
        if (_lineRenderers.Contains(lineRenderer))
        {
            _lineRenderers.Remove(lineRenderer);
            //Debug.Log("RemoveLine" + _lineRenderers.Count);
        }
    }

    // Метод для сброса всех соединений
    public void ResetAllConnections()
    {
        //foreach (var lineRenderer in _lineRenderers)
        //{
        //    Destroy(lineRenderer.gameObject);
        //}

        //_lineRenderers.Clear();

        // Сброс всех флагов на объектах
        foreach (var component in _component)
        {
            component.isConnected = false;
        }
    }
}
