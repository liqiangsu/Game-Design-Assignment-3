using System;
using UnityEngine;


namespace UnityStandardAssets.Utility
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform Target;
        public Vector3 Offset = new Vector3(0f, 7.5f, 0f);


        private void LateUpdate()
        {
            transform.position = Target.position + Offset;
        }
    }
}
