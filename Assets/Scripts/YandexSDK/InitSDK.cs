using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Agava.YandexGames;

public sealed class InitSDK : MonoBehaviour
{
    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize(OnInitialized);
    }

    private void OnInitialized()
    {
        SceneManager.LoadScene(1);
    }
}
