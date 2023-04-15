using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Ball : MonoBehaviour
{
    private CellBlock _cell;
    private Rigidbody _rigidbody;
    private SphereCollider _collider;
    private Coroutine _releaseCorutine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
        _rigidbody.isKinematic = true;
        float minSizeMultiplier = 1f;
        float maxSizeMultiplier = 1.6f;
        float sizeMultiplier = Random.Range(minSizeMultiplier, maxSizeMultiplier);
        transform.localScale = new Vector3(sizeMultiplier * transform.localScale.x, sizeMultiplier * transform.localScale.y, sizeMultiplier * transform.localScale.z);
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
        if (_releaseCorutine != null)
            StopCoroutine(_releaseCorutine);

        _releaseCorutine = StartCoroutine(Release());
    }

    private IEnumerator Release()
    {
        float minDelayTime = 0f;
        float maxDelayTime = 0.2f;
        float delayTime = Random.Range(minDelayTime, maxDelayTime);
        yield return new WaitForSeconds(delayTime);
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
        _releaseCorutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CollectionField collectionField))
            _collider.isTrigger = false;
    }
}
