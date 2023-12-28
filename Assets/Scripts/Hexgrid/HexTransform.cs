using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Utility;
using Lotl.Generic.Variables;

namespace Lotl.Hexgrid
{
    public class HexTransform : MonoBehaviour
    {
        #region Properties

        [SerializeField] private FloatReference hexSize;
        [SerializeField] private Hex hexPosition;
        [SerializeField] private float hexYPosition;
        private Vector3 worldPosition;

        public Hex HexPosition
        {
            get => hexPosition;
            set
            {
                hexPosition = value;
                UpdateTransformFromHex();
            }
        }

        public float HexYPosition
        {
            get => hexYPosition;
            set { worldPosition.y = hexYPosition = value; }
        }

        public Vector3 WorldPosition
        {
            get => worldPosition;
            set
            {
                transform.position = worldPosition = value;
                hexYPosition = worldPosition.y;
                UpdateHexFromTransform();
            }
        }

        #endregion

        #region Methods

        private void OnEnable()
        {
            UpdateTransformFromHex();
        }

        private void FixedUpdate()
        {
            UpdateTransformFromWorld();
        }

        private void UpdateHexFromTransform()
        {
            HexPosition = Hex.PixelToHex(transform.position.xz(), hexSize);
            HexYPosition = transform.position.y;
            worldPosition = transform.position;
        }

        private void UpdateTransformFromHex()
        {
            UpdateWorldFromHex();
            UpdateTransformFromWorld();
        }

        private void UpdateWorldFromHex()
        {
            worldPosition = Hex.HexToPixel(hexPosition, hexSize).xz();
            worldPosition.y = hexYPosition;
        }

        private void UpdateTransformFromWorld()
        {
            if (worldPosition != transform.position)
            {
                transform.position = worldPosition;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            UpdateWorldFromHex();
        }

#endif

        #endregion
    }
}
