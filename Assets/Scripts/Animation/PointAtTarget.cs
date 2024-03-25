using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lotl.Units.Attackers;
using Lotl.Generic.Variables;
using Lotl.Units.Generic.StateMachine;

namespace Lotl.Animation
{
    public class PointAtTarget : MonoBehaviour
    {
        private IContainsTarget driver;
        [SerializeField] private FloatReference turnaroundTime;
        [SerializeField] private Transform arrow;

        private float turnSpeed = 1;

        private void Start()
        {
            if(!TryGetComponent(out driver))
                Debug.LogWarning("Missing IContainsTarget!");
            
            turnSpeed = 180f / (turnaroundTime + float.Epsilon);
        }

        private void Update()
        {
            if(driver.CurrentTarget == null) return;
            Turn();
        }

        private void Turn()
        {
            Vector3 direction = driver.CurrentTarget.transform.position - arrow.position;
            direction.y = 0; direction.Normalize();
            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            float totalAngle = Quaternion.Angle(arrow.rotation, targetRotation);

            if (totalAngle <= Mathf.Epsilon) return;

            float rotationInterpolation = Mathf.Clamp(
                turnSpeed * Time.fixedDeltaTime / totalAngle,
                0, 0.9f);

            arrow.rotation = Quaternion.Slerp(
                arrow.rotation,
                targetRotation,
                rotationInterpolation);
        }
    }
}
