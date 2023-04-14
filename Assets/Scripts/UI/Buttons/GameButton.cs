using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private AudioSource _audioSource;

    protected Button Button => _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    protected virtual void OnButtonClick()
    {
        _audioSource.Play();
    }
}
