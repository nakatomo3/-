using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPartsAnimation : MonoBehaviour {

	public GameObject CollectPartsUI;

	private float timer = 0;
	private const float TIMER_MAX = 4.5f;
	private Transform thisTransform;
	private Vector3 firstPosition;

	private float rotateSpeed = 10;
	private float rotateAcceralation = 30;

	private const float MOVE_START_COUNT = 1f;
	private const float MOVE_END_COUNT = 3.5f;

	private Vector3 movetoPos;

	// Start is called before the first frame update
	void Start() {
		thisTransform = transform;
		float posX = 0;
		if(gameObject.name =="1") {
			posX = -6.1f;
		} else {
			posX = -4.7f;
		}
		movetoPos = new Vector3(posX,3.2f,9);

		firstPosition = thisTransform.position;
	}

	// Update is called once per frame
	void Update() {
		timer += Time.deltaTime;

		if (timer < MOVE_START_COUNT) {
			rotateSpeed += rotateAcceralation * Time.deltaTime;
			thisTransform.position = firstPosition;

		} else if(timer < MOVE_END_COUNT){
			thisTransform.localPosition += new Vector3((movetoPos.x - thisTransform.localPosition.x)/30, (movetoPos.y - thisTransform.localPosition.y) / 30, (movetoPos.z - thisTransform.localPosition.z) / 30);
			rotateSpeed *= 0.98f;
		} else {
			rotateSpeed *= 0.5f;
		}

		thisTransform.Rotate(0, rotateSpeed, 0);

		if(rotateSpeed < 1) {
			Destroy(gameObject);
			var ui = Instantiate(CollectPartsUI, movetoPos,Quaternion.identity,CameraManager.instance.transform);
			ui.transform.localPosition = movetoPos;
			ui.transform.localScale = Vector3.one * 0.5f;
		}
	}
}