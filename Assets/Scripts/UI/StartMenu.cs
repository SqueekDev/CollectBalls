using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class StartMenu : MonoBehaviour
    {
        private const int GameSceneNumber = 2;

        [SerializeField] private GamePanel _startPanel;
        [SerializeField] private Button _startButton;
        [SerializeField] private AudioSource _clickSound;

        private void OnEnable()
        {
            _startButton.onClick.AddListener(StartLevel);
        }

        private void Start()
        {
            _startPanel.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(StartLevel);
        }

        private void StartLevel()
        {
            _clickSound.Play();
            SceneManager.LoadScene(GameSceneNumber);
        }
    }
}