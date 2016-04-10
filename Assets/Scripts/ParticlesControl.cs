using UnityEngine;
using System.Collections;

public class ParticlesControl : MonoBehaviour {

	public string sortingLayer;

	private ParticleSystem particles;

	void Start () {
		particles = GetComponent<ParticleSystem> ();
    	particles.GetComponent<Renderer>().sortingLayerName = sortingLayer;
	}

	void Update () {
		if (!particles.IsAlive ()) {
			Destroy (gameObject);
		}
	}
}
