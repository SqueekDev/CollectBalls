using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private List<CellBlock> _cellBlocks;
    [SerializeField] private List<Grille> _grilles;
    [SerializeField] private List<Material> _matherials;

    private Quaternion _startRotation;
    private Coroutine _shakeCorutine;
    private Coroutine _rotateCorutine;
    private float _delayTime = 0.01f;
    private float _step = 0.2f;
    
    public int BallsCount { get; private set; }

    private void Awake()
    {
        _startRotation = transform.rotation;

        for (int i = 0; i < _cellBlocks.Count; i++)
        {
            BallsCount += _cellBlocks[i].BallsCount;
        }
    }

    private void OnEnable()
    {
        InputDetection.Swiped += OnSwiped;
    }

    private void Start()
    {
        for (int i = 0; i < _grilles.Count; i++)
        {
            Material material = GetMaterial();
            _grilles[i].SetMaterial(material);
        }
    }

    private void OnDisable()
    {
        InputDetection.Swiped -= OnSwiped;       
    }

    public Material GetMaterial()
    {
        if (_matherials.Count > 0)
        {
            int matNumber = Random.Range(0, _matherials.Count);
            Material material = _matherials[matNumber];
            _matherials.RemoveAt(matNumber);
            return material;
        }
        else
        {
            return null;
        }
    }

    private void OnSwiped(Vector3 direction)
    {
        CheckCorutine(_shakeCorutine);
        Quaternion targetRotation = GetRotation(direction);
        _shakeCorutine = StartCoroutine(Shake(targetRotation));
    }

    private IEnumerator Shake(Quaternion targetRotation)
    {
        transform.rotation = _startRotation;
        WaitUntil delay = new WaitUntil(() => _rotateCorutine == null);
        CheckCorutine(_rotateCorutine);
        _rotateCorutine = StartCoroutine(Rotate(_startRotation, targetRotation));
        yield return delay;
        CheckCorutine(_rotateCorutine);
        _rotateCorutine = StartCoroutine(Rotate(targetRotation, _startRotation));
        yield return delay;
        _shakeCorutine = null;
    }

    private IEnumerator Rotate(Quaternion startRotation, Quaternion targetRotation)
    {
        WaitForSeconds delay = new WaitForSeconds(_delayTime);
        float rotateProgress = 0;

        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, rotateProgress);
            rotateProgress += _step;
            yield return delay;
        }

        _rotateCorutine = null;
    }

    private void CheckCorutine(Coroutine coroutine)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    private Quaternion GetRotation(Vector3 direction)
    {
        Quaternion rotation;
        float rotationValue = 7f;

        if (direction.x != 0)
            rotation = direction.x > 0 ? Quaternion.Euler(0, -rotationValue, 0) : Quaternion.Euler(0, rotationValue, 0);
        else
            rotation = direction.y > 0 ? Quaternion.Euler(rotationValue, 0, 0) : Quaternion.Euler(-rotationValue, 0, 0);

        return rotation;
    }
}
