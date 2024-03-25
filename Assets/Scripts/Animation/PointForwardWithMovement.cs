using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lotl.Units.Locomotion;
using Lotl.Generic.Variables;
using Lotl.StateMachine;
using Lotl.Units.Generic.StateMachine;

namespace Lotl.Animation
{
    [RequireComponent(typeof(MobileUnitLocomotion))]
    public class PointForwardWithMovement : MonoBehaviour
    {
        private MobileUnitLocomotion locomotion;
        private IContainsTarget containsTarget;
        [SerializeField] private Transform arrow;
        [SerializeField] private FloatReference sampleDelay;
        [SerializeField] private FloatReference turnaroundTime;

        float sampleTimer = 0f, turnSpeed = 0f;

        Quaternion[] blend = new Quaternion[4];
        int blendIndex = 0;

        Quaternion targetRotation;

        private void Start()
        {
            locomotion = GetComponent<MobileUnitLocomotion>();
            containsTarget = GetComponent<IContainsTarget>();
            
            targetRotation = arrow.rotation = locomotion.MovementRotation;

            turnSpeed = 180f / (turnaroundTime + float.Epsilon);

            for (int i = 0; i < blend.Length; i++)
                blend[i] = targetRotation;
        }

        private void Update()
        {
            HandleRotationSampling();
            Turn();
        }

        private void HandleRotationSampling()
        {
            if (sampleTimer <= 0)
            {
                SampleRotation();
                sampleTimer = sampleDelay;
            }
            sampleTimer -= Time.deltaTime;
        }

        private void SampleRotation()
        {
            blendIndex++; blendIndex &= 0b11;
            
            if(locomotion.IsMoving || containsTarget == null)
            {
                blend[blendIndex] = locomotion.MovementRotation;
            }
            else if (containsTarget.CurrentTarget != null)
            {
                Vector3 direction = containsTarget.CurrentTarget.transform.position - arrow.position;
                direction.y = 0; direction.Normalize();
                if (direction == Vector3.zero) return;

                blend[blendIndex] = Quaternion.LookRotation(direction);
            }

            targetRotation = arrow.rotation;
            for (int i = 0; i < blend.Length; i++)
                targetRotation = Quaternion.Lerp(targetRotation, blend[i], 0.6f);
        }

        private void Turn()
        {
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