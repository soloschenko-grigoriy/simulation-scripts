using System;
using UnityEngine;

namespace GS.Helpers
{
    [RequireComponent(typeof(Camera))]
    public class RTSCameraController : MonoBehaviour
    {
        [SerializeField]
        private float speed = 10f;

        [SerializeField] 
        private Vector3 minBoundaries = default;
        
        [SerializeField]
        private Vector3 maxBoundaries = default;
        
        [SerializeField] 
        private Vector3 targetOffset;

        [SerializeField] 
        private float followSpeed;
        
        private Transform target;

        public void SetTarget(Transform t)
        {
            target = t;
        }

        public void ResetTarget()
        {
            target = null;
        }
        
        private void Update()
        {
            if (target != null)
            {
                FollowTarget();
            }
            else
            {
                Move();
            }
            
            Zoom();
            LimitMove();
        }

        private void Move()
        {
            var position = transform.position;

            if (Input.GetKey(KeyCode.A))
            {
                position.x -= speed * Time.deltaTime;
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                position.x += speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.W))
            {
                position.z += speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S))
            {
                position.z -= speed * Time.deltaTime;
            }
            
            transform.position = position;
        }

        private void Zoom()
        {
            var position = transform.position;
            var y = position.y;
            if (Input.GetKey(KeyCode.E))
            {
                y += speed * Time.deltaTime;
            }
            
            if (Input.GetKey(KeyCode.Q))
            {
                y -= speed * Time.deltaTime;
            }
            
            float zoom = Mathf.Clamp(y, minBoundaries.y, maxBoundaries.y);
            transform.position = new Vector3(position.x, zoom, position.z);
        }
        
        private void LimitMove()
        {
            var x = Mathf.Clamp(transform.position.x, minBoundaries.x, maxBoundaries.x);
            var y = transform.position.y;
            var z = Mathf.Clamp(transform.position.z, minBoundaries.z, maxBoundaries.z);
            
            transform.position = new Vector3(x, y, z);
        }

        private void FollowTarget()
        {
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z) + targetOffset;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }
    }
}
