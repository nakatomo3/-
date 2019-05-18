using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectParts : MonoBehaviour {

	private Transform thisTransform;
	private const float ROTATE_SPEED = 80;

	public GameObject CollectPartsUI;
	public GameObject GetAnimationObject;

	// Start is called before the first frame update
	void Start() {
		if(PlayerPrefs.GetInt(SystemManager.instance.stageNum+gameObject.name) == 1) {
			Debug.Log("すでに獲得しているので削除しました");

			var uiObject = Instantiate(CollectPartsUI, CameraManager.instance.gameObject.transform);
			if(gameObject.name == "1") {
				uiObject.transform.localPosition = new Vector3(-6.1f, 3.2f, 9);
			} else {
				uiObject.transform.localPosition = new Vector3(-4.7f, 3.2f, 9);
			}
			uiObject.transform.localScale = Vector3.one * 0.5f;
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
			Player.instance.isCollectGets[int.Parse(gameObject.name)-1] = true;
			var anime = Instantiate(GetAnimationObject, transform.position, Quaternion.identity, CameraManager.instance.gameObject.transform);
			anime.name = gameObject.name;
			Destroy(gameObject);
		}
	}
}