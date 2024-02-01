using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private int _fieldSizeModifier;
        [SerializeField] private List<CellBlock> _cellBlocks;
        [SerializeField] private List<IronFenceSwiper> _fences;
        [SerializeField] private List<Material> _matherials;

        public int BallsCount { get; private set; }

        public int FieldSizeModifier { get; private set; }

        private void Awake()
        {
            FieldSizeModifier = _fieldSizeModifier;

            for (int i = 0; i < _cellBlocks.Count; i++)
            {
                BallsCount += _cellBlocks[i].BallsCount;
            }
        }

        private void Start()
        {
            for (int i = 0; i < _fences.Count; i++)
            {
                Material material = GetMaterial();
                _fences[i].SetMaterial(material);
            }
        }

        private Material GetMaterial()
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
    }
}