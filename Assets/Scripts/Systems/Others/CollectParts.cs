using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectParts : MonoBehaviour {

	private Transform thisTransform;
	private const float ROTATE_SPEED = 80;

	// Start is called before the first frame update
	void Start() {
		if(PlayerPrefs.GetInt(SystemManager.instance.stageNum+gameObject.name) == 1) {
			Debug.Log("すでに獲得しているので削除しました");
			Destroy(gameObject);
		}
		thisTransform = transform;
	}

	// Update is called once per frame
	void Update() {
		thisTransform.Rotate(0, ROTATE_SPEED * Time.deltaTime, 0);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			Player.instance.isCollectGets[int.Parse(gameObject.name)] = true;
			Destroy(gameObject);
		}
	}
}