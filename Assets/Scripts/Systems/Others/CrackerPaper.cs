using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackerPaper : MonoBehaviour {

	public bool isA;
	private Transform thisTransform;
	private float rotateSpeed = 0;
	private const float ROTATE_SPEED_MAX = 10;
	private float fallSpeed = 0;
	private const float FALL_SPEED_MAX = 5;

	private Renderer renderer;

	// Start is called before the first frame update
	void Start() {
		renderer = GetComponent<Renderer>();
		renderer.material.color = new Color(Random.value, Random.value, Random.value, 1);
		rotateSpeed = (Random.value - 0.5f) * 2 * ROTATE_SPEED_MAX;

		fallSpeed = Random.Range(0, FALL_SPEED_MAX);

		thisTransform = transform;

		transform.localPosition = new Vector3((Random.value - 0.5f) * 2 * 20, Random.value * 15 + 10, 23);

        Destroy(gameObject, 30);

	}

	// Update is called once per frame
	void Update() {
		thisTransform.Rotate(rotateSpeed+1,rotateSpeed /3,0);
		thisTransform.position += Vector3.down * (fallSpeed + 5) * Time.deltaTime;

		if(thisTransform.position.y < -15) {
			Destroy(gameObject);
		}
	}
}
