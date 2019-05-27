using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectRotateGear : MonoBehaviour {

	public List<Transform> gears;
	float rotateSpeed;

	// Start is called before the first frame update
	void Start() {
		for(int i = 0; i < transform.childCount; i++) {
			gears.Add(transform.GetChild(i));
		}
	}

	// Update is called once per frame
	void Update() {
		rotateSpeed += 1 * Time.deltaTime ;
		if(rotateSpeed >= 1) {
			rotateSpeed = 1 * Time.deltaTime;
		}
		for (int i = 0; i < gears.Count; i++) {
			gears[i].Rotate(new Vector3(0, 0, rotateSpeed * ((-1 * i % 2) + 0.5f) * 2)*60*Time.deltaTime);
		}
	}
}