using UnityEngine;
using UnityEngine.UI;

public class ExitButtonView : MonoBehaviour
{
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _exitButton.onClick.AddListener(Application.Quit);
    }
}
