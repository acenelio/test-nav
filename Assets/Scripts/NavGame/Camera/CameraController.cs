using UnityEngine;

namespace NavGame.Character
{
	public class CameraController : MonoBehaviour
	{
		public Transform target;

		public Vector3 offset = new Vector3(0f, -0.5f, -0.4f);
		public float zoomSpeed = 4f;
		public float minZoom = 10f;
		public float maxZoom = 20f;

		public float pitch = 2f;
		public float yawSpeed = 120f;
		public float yawBackSpeed = 6f;
		public float maxYaw = 60f;
		float currentZoom = 20f;
		float currentYaw = 0f;
		
		
		void Update ()
		{
			if (target == null) {
				GameObject player = PlayerManager.instance.GetPlayer();
				if (player != null) {
					target = player.transform;
				}
				return;
			}

			currentZoom -= Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;
			currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

			float h = Input.GetAxisRaw ("Horizontal");
			if (h != 0) {
				currentYaw += h * yawSpeed * Time.deltaTime;
				currentYaw = Mathf.Clamp(currentYaw, -maxYaw, maxYaw);
			}
			else {
				currentYaw = Mathf.Lerp(currentYaw, 0f, yawBackSpeed * Time.deltaTime);
			}
		}

		void LateUpdate () {
			if (target == null) {
				return;
			}
			transform.position = target.position - offset * currentZoom;
			transform.LookAt(target.position + Vector3.up * pitch);

			transform.RotateAround (target.position, Vector3.up, currentYaw);
		}
	}
}
