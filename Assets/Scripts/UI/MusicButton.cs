using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Sprite _musicOnIcon;
    [SerializeField] private Sprite _musicOffIcon;

    private void Awake()
    {
        _button.image.sprite = _musicOnIcon;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (_button.image.sprite == _musicOnIcon)
            _button.image.sprite = _musicOffIcon;
        else
            _button.image.sprite = _musicOnIcon;
    }
}
