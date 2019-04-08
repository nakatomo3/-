using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

	private float deathCounter;
	private const float DEATH_INTERVAL = 1;

	// Start is called before the first frame update
	void Start() {
		deathCounter = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	void Update() {
		if(deathCounter + DEATH_INTERVAL <= Time.realtimeSinceStartup) {
			Instantiate(Resources.Load("Prefabs/Systems/FadeOut") as GameObject, transform.position, Quaternion.identity).GetComponent<FadeOut>().isGameOver = true;
			Destroy(gameObject);
		}
	}
}