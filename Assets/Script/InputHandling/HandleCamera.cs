using UnityEngine;
using System.Collections;

namespace InputHandling
{
    public class HandleCamera : MonoBehaviour
    {
        private float _rotX;
        private float _rotY;


        // Use this for initialization
        void Start()
        {
            _rotX = Camera.main.transform.rotation.eulerAngles.z;
            _rotY = Camera.main.transform.rotation.eulerAngles.x;
        }

        // Update is called once per frame
        void Update()
        {
            MoveCamera();
            RotateCamera();
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveCamera()
        {
            var pos = Camera.main.transform.position;
            var rot = Camera.main.transform.rotation.eulerAngles;
            var moveFactor = 0.5f;
            var wheelAction = Input.GetAxis("Mouse ScrollWheel") * -3f;

            /* Move
            */
            if (Input.GetMouseButton(2))
            {
                // Mouse control
                var vertAction = Input.GetAxis("Mouse X") * 2f;
                var horzAction = Input.GetAxis("Mouse Y") * 2f;

                pos = pos + (Camera.main.transform.forward * horzAction);
                pos = pos + (Camera.main.transform.right * vertAction);
            }
            else
            {
                // Keyboard check.
                float oldY = pos.y;
                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow))
                {
                    pos = Input.GetKey(KeyCode.DownArrow)
                        ? pos - (Camera.main.transform.forward * moveFactor)
                        : pos + (Camera.main.transform.forward * moveFactor);
                }

                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                {
                    pos = Input.GetKey(KeyCode.LeftArrow)
                        ? pos - (Camera.main.transform.right * moveFactor)
                        : pos + (Camera.main.transform.right * moveFactor);
                }
                pos = new Vector3(pos.x, oldY, pos.z);
            }

            if (wheelAction != 0)
            {
                pos = new Vector3(pos.x, pos.y + wheelAction, pos.z);
            }

            Camera.main.transform.position = pos;
            Camera.main.transform.rotation = Quaternion.Euler(rot);
        }

        /// <summary>
        /// 
        /// </summary>
        private void RotateCamera()
        {
            var sens = 100f;

            if (Input.GetMouseButton(1))
            {
                // Mouse control
                _rotX += Input.GetAxis("Mouse X") * sens * Time.deltaTime;
                _rotY += Input.GetAxis("Mouse Y") * sens * Time.deltaTime * -1;
                _rotY = Mathf.Clamp(_rotY, 0f, 90f);
                transform.localEulerAngles = new Vector3(_rotY, _rotX, 0);
            }
        }
    }
}