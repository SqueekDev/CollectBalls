using UnityEngine;
using UnityEngine.Events;

public class GamePanel : MonoBehaviour
{
    public event UnityAction Opened;
    public event UnityAction Closed;

    private void OnEnable()
    {
        Opened?.Invoke();
    }

    private void OnDisable()
    {
        Closed?.Invoke();
    }
}
