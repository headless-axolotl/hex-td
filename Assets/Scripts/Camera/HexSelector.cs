using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Extensions;
using Lotl.Hexgrid;
using Lotl.Generic.Variables;

namespace Lotl.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class HexSelector : MonoBehaviour
    {
        [SerializeField] private FloatReference hexSize;
        [SerializeField] private HexVariable selectedHex;
        [SerializeField] private UnityEngine.Camera scanCamera;
        [SerializeField] private LayerMask scanMask;

        private void Awake()
        {
            if(hexSize == null)
            {
                Debug.LogError("Hex size is null!");
            }
            if (selectedHex == null)
            {
                Debug.Log("Selected hex is null!");
            }
            scanCamera = GetComponent<UnityEngine.Camera>();
        }

        private void FixedUpdate()
        {
            SelectHex();
        }

        void SelectHex()
        {
            Ray scanRay = scanCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(scanRay, out RaycastHit hitInfo, 128f, scanMask))
            {
                selectedHex.Value = Hex.PixelToHex(hitInfo.point.xz(), hexSize); 
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            DrawHex(Hex.HexToPixel(selectedHex.Value, hexSize).xz(), hexSize);

            void DrawHex(Vector3 centre, float size)
            {
                float root3 = Mathf.Sqrt(3);
                Vector3[] points = new Vector3[6]
                {
                    new(0, 0,  size), new( size * root3 / 2, 0,  size / 2), new( size * root3 / 2, 0, -size / 2),
                    new(0, 0, -size), new(-size * root3 / 2, 0, -size / 2), new(-size * root3 / 2, 0,  size / 2),
                };
                for (int i = 0; i < 6; i++)
                    points[i] += centre;
                for (int i = 0; i < 6; i++)
                    Gizmos.DrawLine(points[i], points[(i + 1) % 6]);
            }
        }
    }

#endif
}
