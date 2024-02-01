using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SoundButton : GameButton
    {
        private const int FalseValue = 0;
        private const int TrueValue = 1;

        [SerializeField] private Sprite _soundOnIcon;
        [SerializeField] private Sprite _soundOffIcon;
        [SerializeField] private ButtonNames _buttonName;

        private bool _isMuted;

        public event Action<bool> SoundButtonClicked;

        private void Awake()
        {
            Button.image.sprite = PlayerPrefs.GetInt(_buttonName.ToString(), FalseValue) == TrueValue ? _soundOffIcon : _soundOnIcon;
            _isMuted = PlayerPrefs.GetInt(_buttonName.ToString(), FalseValue) == TrueValue;
            SoundButtonClicked?.Invoke(_isMuted);
        }

        protected override void OnButtonClick()
        {
            base.OnButtonClick();
            _isMuted = !_isMuted;
            Button.image.sprite = _isMuted ? _soundOffIcon : _soundOnIcon;
            SoundButtonClicked?.Invoke(_isMuted);
            SaveData(_isMuted);
        }

        private void SaveData(bool isMuted)
        {
            int value = isMuted ? TrueValue : FalseValue;
            PlayerPrefs.SetInt(_buttonName.ToString(), value);
            PlayerPrefs.Save();
        }
    }
}