using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Units.Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyLocomotion : MonoBehaviour
    {
        #region Properties

        [Header("Data")]
        [SerializeField] private FloatReference speed;
        [SerializeField] private FloatReference turnaroundTime;
        [Header("Avoidance")]
        [SerializeField] private FloatReference avoidanceMagnitude;
        [SerializeField] private FloatReference avoidanceBaseMagnitude;
        [SerializeField] private FloatReference avoidanceRadius;
        [SerializeField] private UnitTribeMask scanTribeMask;
        [SerializeField] private LayerMask scanMask;

        private new Rigidbody rigidbody;

        private Quaternion movementRotation = Quaternion.identity;

        private float turnSpeed = 1f;
        
        private bool shouldMove = false;
        private Vector3 target = Vector3.zero;

        #endregion

        #region Methods

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            turnSpeed = 180 / turnaroundTime;
            movementRotation = transform.rotation;
            rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if(shouldMove) Move();
        }

        public void MoveTowards(Vector3 target)
        {
            shouldMove = true;
            this.target = target;
        }

        public void StopMoving()
        {
            // rigidbody.velocity = Vector3.zero;
            shouldMove = false;
        }

        private void Move()
        {
            Vector3 direction = target - transform.position;
            direction.y = 0; direction.Normalize();
            if (direction == Vector3.zero) return;

            ModifyDirection(ref direction);
            
            Turn(direction);
            
            rigidbody.velocity =
                speed // * Time.fixedDeltaTime
                * (movementRotation * Vector3.forward);
        }

        private void ModifyDirection(ref Vector3 direction)
        {
            List<Unit> unitsToAvoid = UnitScanner.Scan(
                transform.position,
                avoidanceRadius,
                scanMask, scanTribeMask);

            Vector3 totalNudge = Vector3.zero;

            for (int i = 0; i < unitsToAvoid.Count; i++)
            {
                Vector3 nudge = transform.position - unitsToAvoid[i].transform.position;
                nudge.y = 0;
                
                if (nudge == Vector3.zero) continue;
                
                ModifyNudge(ref nudge);
                totalNudge += nudge;
            }

            direction += totalNudge;
            direction.Normalize();
        }

        private void ModifyNudge(ref Vector3 nudge)
        {
            Vector3 normalized = nudge.normalized;
            // (+1) to avoid big spike with values close to zero.
            nudge = nudge / (nudge.sqrMagnitude + 1) * avoidanceMagnitude
                + normalized * avoidanceBaseMagnitude;
        }

        public void TurnTowards(Vector3 target)
        {
            Vector3 direction = target - transform.position;
            direction.y = 0f; direction.Normalize();

            Turn(direction);
        }

        private void Turn(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            float totalAngle = Quaternion.Angle(movementRotation, targetRotation);

            if (totalAngle <= Mathf.Epsilon) return;

            float rotationInterpolation = Mathf.Clamp(
                turnSpeed * Time.fixedDeltaTime / totalAngle,
                0, 0.9f);

            movementRotation = Quaternion.Slerp(
                movementRotation,
                targetRotation,
                rotationInterpolation);
        }

        #endregion

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(
                transform.position + movementRotation * (2f * Vector3.forward),
                .15f);
        }

#endif
    }
}
