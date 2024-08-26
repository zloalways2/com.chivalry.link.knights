using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreateLevel : MonoBehaviour
{
    public GameObject _prefab; // ������, ������� ����� ������
    public GameObject _scene; // ������, ��������� �������� �� ���������
    public Transform _parentTransform; // ������������ ������
    [SerializeField] private ChangeBlockView _changeBlockView; // ������, ��� ������� �� ������� ����� ����������� ������

    private void Awake()
    {
        _changeBlockView.TargetBlocksLoaded += OnButtonClick;
    }

    private void OnButtonClick()
    {
        GameObject newLevel = Instantiate(_prefab, Vector3.zero, Quaternion.identity, _parentTransform);
    }
}
