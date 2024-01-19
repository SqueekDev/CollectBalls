using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MusicButton : GameButton
    {
        private const string MusicKeyName = "Music";
        private const int FalseValue = 0;
        private const int TrueValue = 1;

        [SerializeField] private Sprite _musicOnIcon;
        [SerializeField] private Sprite _musicOffIcon;

        private string _keyName;
        private bool _isMuted;

        public event Action<bool> Clicked;

        private void Awake()
        {
            _keyName = GetKeyName();
            Button.image.sprite = PlayerPrefs.GetInt(_keyName, FalseValue) == TrueValue ? _musicOffIcon : _musicOnIcon;
            _isMuted = PlayerPrefs.GetInt(_keyName, FalseValue) == TrueValue;
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
            return MusicKeyName;
        }

        private void SaveData(bool isMuted)
        {
            int value = isMuted ? TrueValue : FalseValue;
            PlayerPrefs.SetInt(_keyName, value);
            PlayerPrefs.Save();
        }
    }
}