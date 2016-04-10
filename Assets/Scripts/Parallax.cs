using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	public Transform background;
	public Transform midground;

	public Vector3 backgroundParallaxScale;
	public Vector3 midgroundParallaxScale;

	private Vector3 lastPosition;

	void Start () {
		lastPosition = transform.position;
	}

	void Update () {
		Vector3 move = transform.position - lastPosition;
		background.position += Vector3.Scale (move, backgroundParallaxScale);
		midground.position += Vector3.Scale (move, midgroundParallaxScale);
		lastPosition = transform.position;
	}
}
