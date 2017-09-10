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


		public Boolean shakeFullPower;
		public float maxDeltaShake;
		private long sinusTime =0;

		public float shakeAmount;
		public float speedShakeMultiplicator;


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
        private void FixedUpdateTest()
        {
		/*	if (shakeFullPower) {
					//lemmingFear
				//	transform.Rotate(new Vector3(0,0,Mathf.Sin(sinusTime++*speedShakeMultiplicator) * shakeAmount)); //new Vector3(0,0,Random.insideUnitSphere * shakeAmount);
				} else {
					//new Vector3(0,0,Mathf.Sin(sinusTime++*speedShakeMultiplicator) * shakeAmount)
					//lemming.transform.localRotation.z = 0;
					//transform.Rotate(new Vector3(0,0,-Mathf.Sin(sinusTime*speedShakeMultiplicator) * shakeAmount));
					sinusTime = 0;
			}
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

			m_LastTargetPosition = target.position;*/

        }

		public float dampTime = 0.15f;
		private Vector3 velocity = Vector3.zero;

		// Update is called once per frame
		void FixedUpdate () 
		{
			if (target)
			{
				Camera cam = this.GetComponent<Camera>();
				Vector3 point = cam.WorldToViewportPoint(target.position);
				Vector3 delta = target.position + new Vector3(5f,0f,0f) - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
				Vector3 destination = transform.position + delta;
				transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
			}

		}


		public void shake(float strenght){

		}
    }
}
