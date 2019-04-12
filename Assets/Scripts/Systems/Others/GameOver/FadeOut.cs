using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {

	public Image image;
	private float alpha = 0;

	public GameObject GameOverUI;

	[HideInInspector]
	public bool isGameOver;

	// Start is called before the first frame update
	void Start() {
		//Time.timeScale = 0;
	}

	// Update is called once per frame
	void Update() {
		image.color = new Color(0, 0, 0, alpha);
		alpha += Time.deltaTime * 0.6f;
		if(alpha >= 1) {
			if (isGameOver) {
				Instantiate(GameOverUI);
			}
			Destroy(this);
		}
	}
}
