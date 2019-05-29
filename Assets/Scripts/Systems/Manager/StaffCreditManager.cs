using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StaffCreditManager : MonoBehaviour {

	private bool isFade = false;

	private float FadeTimer = 0;
	private const float FADE_TIMER_MAX = 3;

	private float timer = 0;

	public Image fade;
	public GameObject credit;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (Input.anyKey) {
			isFade = true;
		}

		if(isFade == true) {
			FadeTimer += Time.deltaTime;
			fade.color = new Color(0,0,0,FadeTimer/FADE_TIMER_MAX);
		}

		timer += Time.deltaTime;

		Debug.Log(timer);

		if (credit.transform.localPosition.y < 2482) {
			credit.transform.position += Vector3.up * 50 * Time.deltaTime;
		}

		if(FadeTimer >= FADE_TIMER_MAX) {
			SceneManager.LoadScene("StageSelect");
		}

	}
}