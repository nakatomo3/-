using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

	public static TitleManager instance;

	private float demoTimer = 0;
	private float demoInterval = 10f;

	private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {
		if(PlayerPrefs.HasKey("freeAnimation") == false) {
			PlayerPrefs.SetInt("freeAnimation", 0);
		}
	}

	// Update is called once per frame
	void Update() {
		demoTimer += Time.deltaTime;
		if(demoTimer >= demoInterval) {
			demoMovie();
		}

		if(Input.anyKey == true) {
			PlayerPrefs.SetInt("stage", 1);
			PlayerPrefs.SetInt("isTutorial", 1);
			SceneManager.LoadScene("Game");
		}
	}

	private void demoMovie() {

	}
}