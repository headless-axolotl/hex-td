using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Camera
{
    public class CameraMovementController : MonoBehaviour
    {
        [SerializeField] private FloatReference speedMultiplier;
        [SerializeField] private FloatReference maxSpeed;
        [SerializeField] private FloatReference minMagnitude;

        private bool shouldMove;
        private Vector3 baseRight, baseForward;

        private void Awake()
        {
            shouldMove = true;
            CalcultaeVectorBase();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                shouldMove = !shouldMove;
            if (shouldMove)
                Move();
        }

        private void CalcultaeVectorBase()
        {
            baseRight = transform.right.normalized;
            baseForward = transform.forward; baseForward.y = 0;
            baseForward.Normalize();
        }

        private Vector2 CalculateDirection()
        {
            Vector3 screenCenter = new(Screen.width / 2, Screen.height / 2);
            Vector3 direction = Input.mousePosition - screenCenter;
            float fitToScreen = Mathf.Min(Screen.width, Screen.height) / 2;
            return direction / fitToScreen;
        }

        private void Move()
        {
            Vector2 direction = CalculateDirection();
            float magnitude = direction.magnitude;
            if (magnitude < minMagnitude)
                return;

            float multiplier = speedMultiplier * (magnitude - minMagnitude);
            if (multiplier > maxSpeed) multiplier = maxSpeed / magnitude;
            direction.Normalize();

            transform.position += Time.deltaTime * multiplier * 
                (direction.x * baseRight + direction.y * baseForward);
        }
    }
}
