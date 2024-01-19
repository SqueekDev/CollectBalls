using System;
using UnityEngine;

namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        public event Action Opened;

        public event Action Closed;

        private void OnEnable()
        {
            Opened?.Invoke();
        }

        private void OnDisable()
        {
            Closed?.Invoke();
        }
    }
}