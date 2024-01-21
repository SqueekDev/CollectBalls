using UnityEngine;

namespace UI
{
    public class FinishPanel : GamePanel
    {
        [SerializeField] private AudioSource _winSound;

        protected override void OnEnable()
        {
            _winSound.Play();
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}