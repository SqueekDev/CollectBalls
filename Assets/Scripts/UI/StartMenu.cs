using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GamePanel _startPanel;
    [SerializeField] private Button _startButton;
    [SerializeField] private AudioSource _clickSound;

    private void Start()
    {
        _startPanel.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(StartLevel);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(StartLevel);
    }

    private void StartLevel()
    {
        _clickSound.Play();
        SceneManager.LoadScene(2);
    }
}
