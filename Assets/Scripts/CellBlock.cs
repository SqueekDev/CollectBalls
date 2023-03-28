using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CellBlock : MonoBehaviour
{
    [SerializeField] private List<Ball> _balls;

    private bool _isReleased;

    public event UnityAction Released;

    private void Awake()
    {
        _isReleased = false;

        for (int i = 0; i < _balls.Count; i++)
        {
            _balls[i].Init(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Grille grille) && _isReleased == false)
        {
            Released?.Invoke();
            _isReleased = true;
        }
    }
}
