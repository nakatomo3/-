using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionManager : MonoBehaviour {

	public static OptionManager instance;
	private bool isFullScreen = false;
	private int width, height;

	private bool isPause = false;

	private readonly int[,] SCREEN_RESOLUTION_SET = new int[5, 2]{
		{ 960 , 540 },
		{ 1024, 576 },
		{ 1280, 720 },
		{ 1920, 1080 },
		{ 2560, 1440 }
	};

	public GameObject canvas;
	public GameObject isPauseObject;

	private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {
		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad(canvas);
		DontDestroyOnLoad(isPauseObject);
	}

	// Update is called once per frame
	void Update() {
#if UNITY_EDITOR
		ChangeScene();
#endif
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if(isPause == true) {
				isPause = false;
				Time.timeScale = 1f;
				canvas.SetActive(false);
			} else {
				isPause = true;
				Time.timeScale = 0f;
				canvas.SetActive(true);
			}
		}
	}

	/// <summary>
	/// デバッグ用
	/// </summary>
	public void ChangeScene() {
		//if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKey(KeyCode.Return)) {
		//	SceneManager.LoadScene("Game");
		//}
	}

	/// <summary>
	/// 画面サイズの変更
	/// </summary>
	/// <param name="setNum">解像度セットの番号</param>
	public void ChngeScreenSize(int setNum) {
		Screen.SetResolution(SCREEN_RESOLUTION_SET[setNum, 0], SCREEN_RESOLUTION_SET[setNum, 1], isFullScreen);
	}


	/// <summary>
	/// フルスクリーンにするかを変更する
	/// </summary>
	public void ChangeIsFullScreen() {
		isFullScreen = !isFullScreen;
	}
}
