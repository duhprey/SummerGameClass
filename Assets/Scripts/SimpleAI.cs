using UnityEngine;
using System.Collections;

using UnityStandardAssets._2D;

public class SimpleAI : MonoBehaviour 
{
	public Transform player;
	public float jumpDelay = 3.0f;
	private float nextJump = 0;

	private PlatformerCharacter2D m_Character;
	private bool m_Jump;
	private float direction;

	private void Awake () {
		m_Character = GetComponent<PlatformerCharacter2D> ();
	}

	private void Update () {
		if (Time.time > nextJump) {
			direction = Mathf.Sign (player.position.x - transform.position.x);
			m_Jump = true;
			nextJump = Time.time + jumpDelay;
		}
	}

	private void FixedUpdate () {
		if (m_Jump) {
			m_Character.Move (direction, false, m_Jump);
			m_Jump = false;
		}
	}
}
