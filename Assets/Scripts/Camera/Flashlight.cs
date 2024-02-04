using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Camera
{
    public class Flashlight : MonoBehaviour
    {
        [Header("Runtime")]
        [SerializeField] private UnityEngine.Camera scanCamera;
        [SerializeField] private List<Transform> followTransforms;
        [Range(0, 1)]
        [SerializeField] private float followLerp;
        [SerializeField] private float followSpeed;
        [Range(0, 1)]
        [SerializeField] private float lookLerp;
        [SerializeField] private LayerMask scanMask;

        private int followIndex = 0;
        private Vector3 targetPosition;
        private Quaternion targetRotation;

        private void Awake()
        {
            targetPosition = transform.position;
            targetRotation = transform.rotation;
            followIndex = 0;
        }

        private void Update()
        {
            HandleInput();
            UpdatePosition();
            UpdateRotation();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                UpdateFollowTransform();
        }

        private void UpdatePosition()
        {
            float lerpAmount = followLerp * followSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpAmount);
            targetPosition = followTransforms[followIndex].position;
        }

        private void UpdateFollowTransform()
        {
            followIndex++;
            followIndex %= followTransforms.Count;
            transform.position = followTransforms[followIndex].position;
        }

        private void UpdateRotation()
        {
            Ray scanRay = scanCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(scanRay, out RaycastHit hitInfo, 128f, scanMask)) return;
            
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lookLerp);
            
            Vector3 lookDirection = (hitInfo.point - transform.position).normalized;
            targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }
    }
}
