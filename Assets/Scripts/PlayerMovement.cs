using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isVertical;

    private bool _isPressed;
    private Vector3 _playerStartPosition;

    private void Start()
    {
        _playerStartPosition = _player.transform.position;
    }

    private void Update()
    {

        if (_isPressed)
        {
            if (_isVertical)
            {
                _player.transform.Translate(0, _speed, 0);
            }
            else
            {
                _player.transform.Translate(_speed, 0, 0);
            }
            // Vector3 transformPosition = _player.transform.position;
            // transformPosition.y = _playerStartPosition.y;
            // _player.transform.position = transformPosition;
            
        }
    }

    public void OnPointerDown(PointerEventData eventData) => _isPressed = true;

    public void OnPointerUp(PointerEventData eventData) => _isPressed = false;
}
