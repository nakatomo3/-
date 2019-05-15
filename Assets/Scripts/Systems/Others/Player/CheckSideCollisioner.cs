using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSideCollisioner : MonoBehaviour {

	public bool isRight;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
	}

	private void OnTriggerStay(Collider other) {
		if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Wall")) {
			if (isRight) {
				Player.instance.canRightMove = false;

			} else {
				Player.instance.canLeftMove = false;
			}
		}
	}

	private void OnCollisionStay(Collision collision) {
		if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall")) {
			if (isRight) {
				Player.instance.canRightMove = false;

			} else {
				Player.instance.canLeftMove = false;
			}
		}
	}

	private void OnCollisionExit(Collision collision) {
		if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall")) {
			if (isRight) {
				Player.instance.canRightMove = true;
			} else {
				Player.instance.canLeftMove = true;
			}
		}
		
	}

	private void FixedUpdate() {

	}

}