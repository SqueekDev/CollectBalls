using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Controller
{
    public class GamePauseFlag : MonoBehaviour
    {
        [SerializeField] private List<GamePanel> _panels;

        public bool IsPaused { get; private set; }

        private void OnEnable()
        {
            foreach (var panel in _panels)
            {
                panel.Opened += OnPanelOpened;
                panel.Closed += OnPanelClosed;
            }
        }

        private void OnDisable()
        {
            foreach (var panel in _panels)
            {
                panel.Opened -= OnPanelOpened;
                panel.Closed -= OnPanelClosed;
            }
        }

        private void OnPanelOpened()
        {
            IsPaused = true;
        }

        private void OnPanelClosed()
        {
            IsPaused = false;
        }
    }
}