using System.Collections;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    public class Ball : MonoBehaviour
    {
        private CellBlock _cell;
        private Rigidbody _rigidbody;
        private SphereCollider _collider;
        private Coroutine _releaseCorutine;
        private WaitForSeconds _delay;
        private float _minSizeMultiplier = 1f;
        private float _maxSizeMultiplier = 1.6f;
        private float _minDelayTime = 0f;
        private float _maxDelayTime = 0.2f;
        private float _minExtractionForce = 2;
        private float _maxExtractionForce = 3;
        private float _spread = 0.2f;
        private float _directionZ = -1f;

        private void Awake()
        {
            _delay = new WaitForSeconds(Random.Range(_minDelayTime, _maxDelayTime));
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
            _rigidbody.isKinematic = true;
            float sizeMultiplier = Random.Range(_minSizeMultiplier, _maxSizeMultiplier);
            transform.localScale = new Vector3(sizeMultiplier * transform.localScale.x, sizeMultiplier * transform.localScale.y, sizeMultiplier * transform.localScale.z);
        }

        private void OnDisable()
        {
            if (_cell != null)
            {
                _cell.Released -= OnCellReleased;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out CollectionField collectionField))
            {
                _collider.isTrigger = false;
            }
        }

        public void Init(CellBlock cellBlock)
        {
            _cell = cellBlock;
            _cell.Released += OnCellReleased;
        }

        private IEnumerator Release()
        {
            yield return _delay;
            transform.parent = null;
            _rigidbody.isKinematic = false;
            float extractionForce = Random.Range(_minExtractionForce, _maxExtractionForce);
            float directionX = Random.Range(-_spread, _spread);
            float directionY = Random.Range(0, _spread);
            Vector3 direction = new Vector3(directionX, directionY, _directionZ);
            _rigidbody.AddForce(direction * extractionForce, ForceMode.Impulse);
            _releaseCorutine = null;
        }

        private void OnCellReleased()
        {
            if (_releaseCorutine != null)
            {
                StopCoroutine(_releaseCorutine);
            }

            _releaseCorutine = StartCoroutine(Release());
        }
    }
}