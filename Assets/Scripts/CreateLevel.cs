using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreateLevel : MonoBehaviour
{
    public GameObject _prefab; // Префаб, который будет создан
    public GameObject _scene; // Объект, состояние которого мы проверяем
    public Transform _parentTransform; // Родительский объект
    [SerializeField] private ChangeBlockView _changeBlockView; // Кнопка, при нажатии на которую будет создаваться объект

    private void Awake()
    {
        _changeBlockView.TargetBlocksLoaded += OnButtonClick;
    }

    private void OnButtonClick()
    {
        GameObject newLevel = Instantiate(_prefab, Vector3.zero, Quaternion.identity, _parentTransform);
    }
}
