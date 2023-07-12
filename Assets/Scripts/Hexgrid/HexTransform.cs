using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Extensions;
using Lotl.Generic.Variables;

namespace Lotl.Hexgrid
{
    public class HexTransform : MonoBehaviour
    {
        #region Properties

        [SerializeField] private FloatReference hexSize;
        [SerializeField] private Hex position;
        [SerializeField] private float yPosition;
        private Vector3 worldPosition;

        public Hex Position
        {
            get => position;
            set { position = value; UpdateWorldPosition(); }
        }

        public float YPosition
        {
            get => yPosition;
            set { worldPosition.y = yPosition = value;; }
        }

        #endregion

        #region Methods

        private void Awake()
        {
            SnapToGrid();
        }

        private void FixedUpdate()
        {
            UpdateTransform();
        }

        private void SnapToGrid()
        {
            Position = Hex.PixelToHex(transform.position.xz(), hexSize);
            YPosition = transform.position.y;
        }

        private void UpdateWorldPosition()
        {
            worldPosition = Hex.HexToPixel(position, hexSize).xz();
            worldPosition.y = yPosition;
        }

        private void UpdateTransform()
        {
            if (worldPosition != transform.position)
            {
                transform.position = worldPosition;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            UpdateWorldPosition();
        }

#endif

        #endregion
    }
}
