using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 0.4f;
        public float lookAheadFactor = 0.6f;
        public float lookAheadReturnSpeed = 0.6f;
        public float lookAheadMoveThreshold = 0.6f;

		public float bottomThresholdPercent =0.0f;
		public float rightThresholdPercent =0.0f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        // Use this for initialization
        private void Start()
        {
            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }


        // Update is called once per frame
        private void Update()
        {
			//float bottomThresholdWithCameraSize = bottomThreshold + this.GetComponent<Camera> ().orthographicSize;
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;

            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

			float cameraSize = this.GetComponent<Camera> ().orthographicSize/2;

			float rightPosition = cameraSize * (rightThresholdPercent / 100);
			float bottomPosition = cameraSize * (bottomThresholdPercent / 100);
			transform.position = newPos  + new Vector3(rightPosition,bottomPosition,0);

			m_LastTargetPosition = target.position;
        }
    }
}
