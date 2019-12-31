using UnityEngine;
using System.Linq;

namespace NavGame.Utils
{
	[RequireComponent(typeof(MeshRenderer))]
	public class ColorOnHover : MonoBehaviour {

		public Color color;
		MeshRenderer meshRenderer;

		Color[] originalColours;

		void Start() {
			meshRenderer = GetComponent<MeshRenderer> ();
			originalColours = meshRenderer.materials.Select (x => x.color).ToArray ();
		}

		void OnMouseEnter ()
		{
			foreach (Material mat in meshRenderer.materials) {
				mat.color *= color;
			}

		}

		void OnMouseExit()
		{
			for (int i = 0; i < originalColours.Length; i++) {
				meshRenderer.materials [i].color = originalColours [i];
			}
		}

	}
}
