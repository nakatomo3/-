using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GetSoundSetting : MonoBehaviour {

	public bool isBGM;
	private AudioSource audioSource;

	// Start is called before the first frame update
	void Start() {
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update() {
		if(isBGM == true) {
			audioSource.volume = OptionManager.instance.BGMVolume / 100;
		} else {
			audioSource.volume = OptionManager.instance.SEVolume / 100;
		}
	}
}