using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Ball : MonoBehaviour
{
    private CellBlock _cell;
    private Rigidbody _rigidbody;
    private SphereCollider _collider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
        _rigidbody.isKinematic = true;
        float minSizeMultiplier = 0.8f;
        float maxSizeMultiplier = 1.5f;
        float sizeMultiplier = Random.Range(minSizeMultiplier, maxSizeMultiplier);
        transform.localScale *= sizeMultiplier;
    }

    private void OnDisable()
    {
        if (_cell != null)
            _cell.Released -= OnCellReleased;
    }

    public void Init(CellBlock cellBlock)
    {
        _cell = cellBlock;
        _cell.Released += OnCellReleased;
    }

    private void OnCellReleased()
    {
        transform.parent = null;
        _rigidbody.isKinematic = false;
        float minExtractionForce = 2;
        float maxExtractionForce = 3;
        float extractionForce = Random.Range(minExtractionForce, maxExtractionForce);
        float spread = 0.2f;
        float directionX = Random.Range(-spread, spread);
        float directionY = Random.Range(0, spread);
        float directionZ = -1f;
        Vector3 direction = new Vector3(directionX, directionY, directionZ);
        _rigidbody.AddForce(direction * extractionForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CollectionField collectionField))
        {
            _collider.isTrigger = false;
        }
    }
}
