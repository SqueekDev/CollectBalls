using System;
using UnityEngine;

namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        public event Action Opened;

        public event Action Closed;

        protected virtual void OnEnable()
        {
            Opened?.Invoke();
        }

        protected virtual void OnDisable()
        {
            Closed?.Invoke();
        }
    }
}