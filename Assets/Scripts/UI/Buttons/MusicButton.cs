using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MusicButton : GameButton
{
    private const string _musicKeyName = "Music";
    private const int _falseValue = 0;
    private const int _trueValue = 1;

    [SerializeField] private Sprite _musicOnIcon;
    [SerializeField] private Sprite _musicOffIcon;

    private string _keyName;
    private bool _isMuted;

    public event UnityAction<bool> Clicked;

    private void Awake()
    {
        _keyName = GetKeyName();
        Button.image.sprite = PlayerPrefs.GetInt(_keyName, _falseValue) == _trueValue ? _musicOffIcon : _musicOnIcon;
        _isMuted = PlayerPrefs.GetInt(_keyName, _falseValue) == _trueValue;
        Clicked?.Invoke(_isMuted);
    }

    protected override void OnButtonClick()
    {
        base.OnButtonClick();

        if (_isMuted == false)
        {
            Button.image.sprite = _musicOffIcon;
            _isMuted = true;
            Clicked?.Invoke(_isMuted);
        }
        else
        {
            Button.image.sprite = _musicOnIcon;
            _isMuted = false;
            Clicked?.Invoke(_isMuted);
        }

        SaveData(_isMuted);
    }

    protected virtual string GetKeyName()
    {
        return _musicKeyName;
    }

    private void SaveData(bool isMuted)
    {
        int value = isMuted ? _trueValue : _falseValue;
        PlayerPrefs.SetInt(_keyName, value);
        PlayerPrefs.Save();
    }
}
