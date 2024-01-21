using UnityEngine;
using YandexSDK;

namespace UI
{
    public class LostPanel : GamePanel
    {
        [SerializeField] private AdShower _adShower;
        [SerializeField] private AudioSource _lostSound;

        protected override void OnEnable()
        {
            base.OnEnable();
            _adShower.VideoAdShowed += OnVideoAdShowed;
            _lostSound.Play();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _adShower.VideoAdShowed -= OnVideoAdShowed;
        }

        private void OnVideoAdShowed()
        {
            gameObject.SetActive(false);
        }
    }
}