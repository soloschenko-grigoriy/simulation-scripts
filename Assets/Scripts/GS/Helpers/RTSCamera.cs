// using System;
// using UnityEngine;
//
// namespace GS.Helpers
// {
//     public class RTSCamera:MonoBehaviour
//     {
//         public static RTSCamera Instance { get; private set; }
//         public Transform followTarget;
//
//         [SerializeField] 
//         private Transform camera;
//         
//         [SerializeField] 
//         private float movementTime;
//         
//         [SerializeField]
//         private float movementSpeed;
//
//         [SerializeField]
//         private float rotationSpeed;
//         
//         [SerializeField]
//         private Vector3 zoomSpeed;
//
//         private Vector3 position;
//         private Quaternion rotation;
//         private Vector3 zoom;
//
//         private void Start()
//         {
//             Instance = this;
//             position = transform.position;
//             rotation = transform.rotation;
//             zoom = camera.localPosition;
//         }
//
//         private void Update()
//         {
//             if (!Instance)
//             {
//                 Instance = this;
//             }
//             if (followTarget != null)
//             {
//                 transform.position = followTarget.position;
//             }
//             else
//             {
//                 HandleInput();
//             }
//
//             if (Input.GetKey(KeyCode.Escape))
//             {
//                 followTarget = null;
//             }
//         }
//
//         private void HandleInput()
//         {
//             if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
//             {
//                 position += (transform.forward * movementSpeed);
//             }
//             if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
//             {
//                 position += (transform.forward * -movementSpeed);
//             }
//             if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
//             {
//                 position += (transform.right * movementSpeed);
//             }
//             if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
//             {
//                 position += (transform.right * -movementSpeed);
//             }
//
//             if (Input.GetKey(KeyCode.Q))
//             {
//                 rotation *= Quaternion.Euler(Vector3.up * rotationSpeed);
//             }
//             if (Input.GetKey(KeyCode.E))
//             {
//                 rotation *= Quaternion.Euler(Vector3.up * -rotationSpeed);
//             }
//             
//             if (Input.GetKey(KeyCode.R))
//             {
//                 zoom += zoomSpeed;
//             }
//             if (Input.GetKey(KeyCode.F))
//             {
//                 zoom -= zoomSpeed;
//             }
//
//             transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * movementTime);
//             transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * movementTime);
//             camera.localPosition = Vector3.Lerp(camera.localPosition, zoom, Time.deltaTime * movementTime);
//         }
//     }
// }
