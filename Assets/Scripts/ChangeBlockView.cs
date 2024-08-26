using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChangeBlockView : MonoBehaviour
{
    public event Action TargetBlocksLoaded;

    [FormerlySerializedAs("_currentBlocks")] [SerializeField] private List<GameObject> _toDeactivate;
    [FormerlySerializedAs("_targetBlocks")] [SerializeField] private List<GameObject> _toActivate;
    [SerializeField] private Button _targetButton;

    private void Awake()
    {
        _targetButton.onClick.AddListener(OnTargetButtonCliked);
    }

    public void OnTargetButtonCliked()
    {
        foreach (GameObject block in _toDeactivate)
        {
            block.SetActive(false);
        }
        foreach (GameObject block in _toActivate)
        {
            block.SetActive(true);
        }

        TargetBlocksLoaded?.Invoke();
    }
}

