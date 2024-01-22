using System.Collections;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    public class Ball : MonoBehaviour
    {
        private const float MinSizeMultiplier = 1f;
        private const float MaxSizeMultiplier = 1.6f;
        private const float MinDelayTime = 0f;
        private const float MaxDelayTime = 0.2f;
        private const float MinExtractionForce = 2;
        private const float MaxExtractionForce = 3;
        private const float Spread = 0.2f;
        private const float DirectionZ = -1f;

        private CellBlock _cell;
        private Rigidbody _rigidbody;
        private SphereCollider _collider;
        private Coroutine _releaseCorutine;
        private WaitForSeconds _delay;

        private void Awake()
        {
            _delay = new WaitForSeconds(Random.Range(MinDelayTime, MaxDelayTime));
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
            _rigidbody.isKinematic = true;
            float sizeMultiplier = Random.Range(MinSizeMultiplier, MaxSizeMultiplier);
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
            float extractionForce = Random.Range(MinExtractionForce, MaxExtractionForce);
            float directionX = Random.Range(-Spread, Spread);
            float directionY = Random.Range(0, Spread);
            Vector3 direction = new Vector3(directionX, directionY, DirectionZ);
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