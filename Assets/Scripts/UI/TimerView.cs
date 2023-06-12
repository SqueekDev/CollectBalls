using UnityEngine;
using TMPro;

public class TimerView : MonoBehaviour
{
    private const int SecondsInMinute = 60;
    private const float TimeToChangeColor = 6f;

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
        _view.text = _seconds.ToString().Length == 1 ? $"{_minutes} : 0{_seconds}" : $"{_minutes} : {_seconds}";

        if (_timer.CurrentTime > TimeToChangeColor)
            _view.color = _startTimeColor;
        else
            _view.color = _changedTimeColor;
    }
}
