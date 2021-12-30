using UnityEngine;

namespace GS.Helpers
{
    public class Billboard : MonoBehaviour
    {
        private Camera cam;

        // Start is called before the first frame update
        private void Awake()
        {
            cam = Camera.main;
        }

        // Update is called once per frame
        private void Update()
        {
            transform.LookAt(transform.position + cam.transform.forward);
        }
    }
}
