using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gameBeba
{
    public class PlayerRaycast : MonoBehaviour
    {
        public static float distanceFromTarget;
        public static string objectHit;
        private float toTarget;

        private void FixedUpdate()
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit))
            {
                toTarget = hit.distance;
                distanceFromTarget = toTarget;
                objectHit = hit.collider.name;
            }
        }
    }
}


