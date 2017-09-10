using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;

		public float dampTime = 0.15f;
		public Vector2 percentCamera = new Vector2(0.25f,0.55f);
		private Vector3 velocity = Vector3.zero;

		// Update is called once per frame
		void FixedUpdate () 
		{
			if (target)
			{
				Camera cam = this.GetComponent<Camera>();
				Vector3 point = cam.WorldToViewportPoint(target.position);
				Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(percentCamera.x, percentCamera.y, point.z));
				Vector3 destination = transform.position + delta;
				transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
			}

		}


		public void shake(float strenght){

		}
    }
}
