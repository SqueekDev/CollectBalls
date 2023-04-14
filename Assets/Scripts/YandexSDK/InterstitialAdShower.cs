using UnityEngine;
using UnityEngine.Events;
using Agava.YandexGames;

public class InterstitialAdShower : MonoBehaviour
{
    [SerializeField] private LevelController _levelController;

    private const int _numberToShowAd = 2;
    private int _counter;

    public event UnityAction<bool> AdShowing;

    private void OnEnable()
    {
        _levelController.LevelFinished += OnLevelFinished;
    }

    private void OnDisable()
    {
        _levelController.LevelFinished -= OnLevelFinished;
    }

    private void OnLevelFinished()
    {
        if (_counter < _numberToShowAd)
        {
            _counter++;
        }
        else
        {
            InterstitialAd.Show(OnOpenCallBack, OnCloseCallBack);
            _counter = 0;
        }

    }

    private void OnOpenCallBack()
    {
        Time.timeScale = 0;
        AdShowing?.Invoke(true);
    }

    private void OnCloseCallBack(bool state)
    {
        if (state)
            return;

        Time.timeScale = 1;
        AdShowing?.Invoke(false);
    }
}
