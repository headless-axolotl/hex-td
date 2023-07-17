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

        private Vector3 baseRight, baseForward;
        private bool mouseMovementMode;

        private void Awake()
        {
            mouseMovementMode = false;
            CalcultaeVectorBase();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                mouseMovementMode = !mouseMovementMode;
            Move();
        }

        private void CalcultaeVectorBase()
        {
            baseRight = transform.right; baseRight.y = 0;
            baseRight.Normalize();
            baseForward = transform.forward; baseForward.y = 0;
            baseForward.Normalize();
        }

        private void Move()
        {
            if (mouseMovementMode) MouseMove();
            else KeyboardMove();
        }

        private Vector2 CalculateDirection()
        {
            Vector3 screenCenter = new(Screen.width / 2, Screen.height / 2);
            Vector3 direction = Input.mousePosition - screenCenter;
            direction.x = 2 * direction.x / Screen.width;
            direction.y = 2 * direction.y / Screen.height;
            // float fitToScreen = Mathf.Min(Screen.width, Screen.height) / 2;
            return direction; // fitToScreen;
        }

        private void MouseMove()
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

        private void KeyboardMove()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical   = Input.GetAxis("Vertical");

            transform.position += Time.deltaTime * speedMultiplier *
                (horizontal * baseRight + vertical * baseForward);
        }
    }
}
