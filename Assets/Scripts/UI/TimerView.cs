using Controller;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TimerView : MonoBehaviour
    {
        private const int SecondsInMinute = 60;
        private const float TimeToChangeColor = 6f;
        private const int SingleDigitLenght = 1;

        [SerializeField] private TMP_Text _view;
        [SerializeField] private Timer _timer;
        [SerializeField] private Color _startTimeColor;
        [SerializeField] private Color _changedTimeColor;

        private int _seconds;
        private int _minutes;

        private void Update()
        {
            _minutes = (int)_timer.CurrentTime / SecondsInMinute;
            _seconds = (int)_timer.CurrentTime - _minutes * SecondsInMinute;
            string text = _seconds.ToString().Length == SingleDigitLenght
                ? $"{_minutes} : 0{_seconds}"
                : $"{_minutes} : {_seconds}";
            _view.text = text;

            if (_timer.CurrentTime > TimeToChangeColor)
            {
                _view.color = _startTimeColor;
            }
            else
            {
                _view.color = _changedTimeColor;
            }
        }
    }
}