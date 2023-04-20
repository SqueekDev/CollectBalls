using UnityEngine;
using UnityEngine.Events;
using Agava.YandexGames;

public class AdShower : MonoBehaviour
{
    [SerializeField] private LevelController _levelController;
    [SerializeField] private VideoButton _videoButton;

    private const int _numberToShowAd = 2;
    private int _counter;

    public event UnityAction<bool> AdShowing;
    public event UnityAction VideoAdShowed;

    private void OnEnable()
    {
        _levelController.LevelFinished += OnLevelFinished;
        _videoButton.Clicked += OnVideoButtonClick;
    }

    private void OnDisable()
    {
        _levelController.LevelFinished -= OnLevelFinished;
        _videoButton.Clicked -= OnVideoButtonClick;
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

    private void OnVideoButtonClick()
    {
        VideoAd.Show(OnOpenCallBack, OnRewardCallBack, OnCloseCallback);
    }

    private void OnOpenCallBack()
    {
        Time.timeScale = 0;
        AdShowing?.Invoke(true);
    }

    private void OnRewardCallBack()
    {
        VideoAdShowed?.Invoke();
    }

    private void OnCloseCallback()
    {
        Time.timeScale = 1;
        AdShowing?.Invoke(false);
    }

    private void OnCloseCallBack(bool state)
    {
        Time.timeScale = 1;
        AdShowing?.Invoke(false);
    }
}
