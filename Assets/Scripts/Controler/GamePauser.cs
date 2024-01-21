using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Controller
{
    public class GamePauser : MonoBehaviour
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