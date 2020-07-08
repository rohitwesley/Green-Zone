using System.Collections;
using UnityEngine;

namespace MovementTools
{
    public class HoverMovement : MonoBehaviour
    {
        /// <summary>
        /// Hover Model
        /// </summary>
        [Tooltip("Rotate in X Axis")]
        [SerializeField] private bool xRotation;
        [Tooltip("Rotate in Y Axis")]
        [SerializeField] private bool yRotation = true;
        [Tooltip("Rotate in Z Axis")]
        [SerializeField] private bool zRotation;

        [Tooltip("Oscillate in X Axis")]
        [SerializeField] private bool xOscillate;
        [Tooltip("Oscillate in Y Axis")]
        [SerializeField] private bool yOscillate = true;
        [Tooltip("Oscillate in Z Axis")]
        [SerializeField] private bool zOscillate;

        [Tooltip("Oscillate Amplitude")]
        [Range (0,100)]
        [SerializeField] private float amplitude = 0.5f;
        [Tooltip("Oscillate Frequency")]
        [Range(0, 10)]
        [SerializeField] private float frequency = 0.1f;
        [Tooltip("Oscillate Phase")]
        [Range(-1, 1)]
        [SerializeField] private float phase = 0.0f;
        [Tooltip("Oscillate Center Point")]
        [SerializeField] private Transform hoverPivot;
            
        [Tooltip("Show Debug Gizmos")]
        [SerializeField] private bool isDebug = true;

        /// <summary>
        /// Hover Object on specific axes
        /// </summary>
        void Update()
        {
            

            // Rotate
            if (xRotation)
            {
                transform.Rotate(new Vector3(15, 0, 0) * Time.deltaTime, Space.Self);
            }
            if (yRotation)
            {
                transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime, Space.Self);
            }
            if (zRotation)
            {
                transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime, Space.Self);
            }
            // Oscillate
            float Oscillaton = amplitude * Mathf.Sin((2*Mathf.PI*frequency*Time.time) + phase);
            Vector3 updatePosition = transform.position;
            if (xOscillate || yOscillate || zOscillate)
            {
                updatePosition = hoverPivot.position;
                if (xOscillate)
                {
                    updatePosition += new Vector3(Oscillaton, 0, 0);
                }
                if (yOscillate)
                {
                    updatePosition += new Vector3(0, Oscillaton, 0);
                }
                if (zOscillate)
                {
                    updatePosition += new Vector3(0, 0, Oscillaton);
                }
            }

            transform.position = updatePosition;
        }


        /// <summary>
        /// Hover Debuger 
        /// </summary>
        private void OnDrawGizmosOnDrawGizmosSelected()
        {
            if (isDebug)
            {
                Gizmos.color = Color.white;

                Gizmos.DrawWireCube(hoverPivot.position, hoverPivot.localScale * 2);

            }

        }

    }

}
