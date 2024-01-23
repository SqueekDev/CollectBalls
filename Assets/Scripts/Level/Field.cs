using System.Collections.Generic;
using Global;
using UnityEngine;

namespace Level
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private int _fieldSizeModifier;
        [SerializeField] private List<CellBlock> _cellBlocks;
        [SerializeField] private List<GrilleSwiper> _grilles;
        [SerializeField] private List<Material> _matherials;

        public int BallsCount { get; private set; }

        public int FieldSizeModifier { get; private set; }

        private void Awake()
        {
            FieldSizeModifier = _fieldSizeModifier;

            for (int i = GlobalValues.Zero; i < _cellBlocks.Count; i++)
            {
                BallsCount += _cellBlocks[i].BallsCount;
            }
        }

        private void Start()
        {
            for (int i = GlobalValues.Zero; i < _grilles.Count; i++)
            {
                Material material = GetMaterial();
                _grilles[i].SetMaterial(material);
            }
        }

        private Material GetMaterial()
        {
            if (_matherials.Count > GlobalValues.Zero)
            {
                int matNumber = Random.Range(GlobalValues.Zero, _matherials.Count);
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