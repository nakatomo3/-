using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour {

	public static OptionManager instance;
	private bool isFullScreen = false;
	private int width, height;

	[SerializeField]
    private bool isPause = false;

    private int mainCursorPos = 0;
    private int restartCorsorPos = 1;
    private int optionCursorPos = 0;
    private int optionScreenCursorPos = 0;
	/// <summary>
	/// 確認の状態。0がYes、1がNo
	/// </summary>
    private int exitCorsorPos = 1;

	/// <summary>
	/// リスタートなどの確認状態になっているかどうか
	/// </summary>
	private bool isConfirmation = false; 
	/// <summary>
	/// オプション画面を開いているかどうか
	/// </summary>
	private bool isOpeningOptionWindow = false;
	/// <summary>
	/// ウィンドウ設定を開いているかどうか
	/// </summary>
	private bool isScreenSelect = false;
	/// <summary>
	/// BGMやSEの音量操作をしているかどうか
	/// </summary>
	private bool isSelectingBGM_SE = false; 

    public GameObject confirmation;

    public GameObject optionWindow;
    public GameObject optionScreenSelect;

	public Text BGMValue;
	public Text SEValue;

    private readonly int[,] WINDOW_RESOLUTION_SET = new int[5, 2]{
		{ 960 , 540 },
		{ 1024, 576 },
		{ 1280, 720 },
		{ 1920, 1080 },
		{ 2560, 1440 }
	};
	private int windowSizeNum = 0;

	public GameObject canvas;

	private float SEVolume = 50;
	private float BGMVolume = 50;
	private const int VOLUME_MIN = 0;
	private const int VOLUME_MAX = 100;

	private float inputStartTime = 0;

	enum MainCursorManu{
		Continue,
		Option,
		Restart,
		Exit
	};
	enum OptionCursor {
		BGM,
		SE,
		Window,
		Back
	}
	enum WindowOptionCursor {
		IsFullScreen,
		Size,
		Back,
		Aplly
	}

	private static int GEAR_NUM_MAX = 16;
	public GameObject[] GearParents = new GameObject[(int)MainCursorManu.Exit+1];
	private GameObject[,] gears = new GameObject[(int)MainCursorManu.Exit+1, GEAR_NUM_MAX];

	[SerializeField]
	private Sprite[] fullScreenSprite = new Sprite[2];
	public Image fullScreenImage;
	public GameObject[] FullScreenArrow = new GameObject[2];

	private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {
        Time.timeScale = 1f;

		for(int i = 0; i < GearParents.Length; i++) {
			for (int j = 0; j < GEAR_NUM_MAX; j++) {
				gears[i, j] = GearParents[i].transform.GetChild(j).gameObject;
			}
		}
	}

    // Update is called once per frame
    void Update() {
		if(BGMVolume >= VOLUME_MAX) BGMVolume = VOLUME_MAX;
		if(SEVolume >= VOLUME_MAX)	SEVolume = VOLUME_MAX;
		if (BGMVolume <= VOLUME_MIN) BGMVolume = VOLUME_MIN;
		if(SEVolume <= VOLUME_MIN) SEVolume = VOLUME_MIN;

		BGMValue.text = Mathf.RoundToInt(BGMVolume).ToString();
		SEValue.text = Mathf.RoundToInt(SEVolume).ToString();

		bool isInputRightStart = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
		bool isInputLeftStart = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
		bool isInputUpStart = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
		bool isInputDownStart = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);

		bool isInputRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
		bool isInputLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
		bool isInputUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
		bool isInputDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);


		if (Input.GetKeyDown(KeyCode.Escape)) {
			if(isPause == true) {
				isPause = false;
				Time.timeScale = 1f;
				canvas.SetActive(false);
				mainCursorPos = 0;
			} else {
				isPause = true;
				Time.timeScale = 0f;
				canvas.SetActive(true);
			}
		}

		

        //----------ポーズ中の処理------------------
        if (isPause == true) {
			switch (mainCursorPos) {
				case (int)MainCursorManu.Continue:
					Continue();
					break;
				case (int)MainCursorManu.Option:
					Option();
					break;
				case (int)MainCursorManu.Restart:
					Restart();
					break;
				case (int)MainCursorManu.Exit:
					Exit();
					break;
			}

			if (isConfirmation == false && isScreenSelect == false && isOpeningOptionWindow == false) {

				if (isInputUpStart) {
					mainCursorPos--;
				}
				if (isInputDownStart) {
					mainCursorPos++;
				}
				if(mainCursorPos > (int)MainCursorManu.Exit) {
					mainCursorPos = 0;
				}
				if(mainCursorPos < 0) {
					mainCursorPos = (int)MainCursorManu.Exit;
				}
			}

        
        } else {
            isConfirmation = false;
            isScreenSelect = false;
            optionWindow.SetActive(false);
            optionScreenSelect.SetActive(false);
            confirmation.SetActive(false);
            
        }
	}

	/// <summary>
	/// Restartを選択している状態の関数
	/// </summary>
	private void Continue() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			isPause = false;
			Time.timeScale = 1f;
			canvas.SetActive(false);
		}


		for (int i = 0; i < GEAR_NUM_MAX; i++) {
			if (i % 2 == 0) {
				gears[(int)MainCursorManu.Continue, i].transform.Rotate(0, 0, 1);
			} else {
				gears[(int)MainCursorManu.Continue, i].transform.Rotate(0, 0, -1);
			}
		}
	}

	private void Option() {
		bool isEnter = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
		bool isBack = Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);

		for (int i = 0; i < GEAR_NUM_MAX; i++) {
			if (i % 2 == 0) {
				gears[(int)MainCursorManu.Option, i].transform.Rotate(0, 0, 1);
			} else {
				gears[(int)MainCursorManu.Option, i].transform.Rotate(0, 0, -1);
			}
		}

		if (isEnter == true) {
			if (isOpeningOptionWindow == true) {

				switch (optionCursorPos) {
					default:
						Debug.LogError("存在しないOptionCursorを選択しています");
						break;
					case (int)OptionCursor.BGM:
					case (int)OptionCursor.SE:
						break;
					case (int)OptionCursor.Window:
						isScreenSelect = true;
						optionScreenSelect.SetActive(true);
						break;
					case (int)OptionCursor.Back:
						isOpeningOptionWindow = false;
						optionScreenSelect.SetActive(false);
						break;
				}
			} else {
				isOpeningOptionWindow = true;
			}
		}
		if (isBack == true) {
			if (isOpeningOptionWindow == true) {
				switch (optionCursorPos) {
					case (int)OptionCursor.Back:
						break;
				}
			}
		}

		bool isPushingRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
		bool isPushingLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
		bool isStartRight = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
		bool isStartLeft = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);

		bool isStartUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
		bool isStartDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);

		if (isOpeningOptionWindow == true) {
			optionWindow.SetActive(true);

			if (isSelectingBGM_SE == false && isScreenSelect == false) {
				if (isStartUp) {
					optionCursorPos--;
				}
				if (isStartDown) {
					optionCursorPos++;
				}

				optionCursorPos = (optionCursorPos + (int)OptionCursor.Back + 1) % ((int)OptionCursor.Back+1);
			}

			if (optionCursorPos == (int)OptionCursor.BGM) {
				if (isPushingRight && BGMVolume < VOLUME_MAX) {
					BGMVolume += (Time.realtimeSinceStartup - inputStartTime) / 15;
				} else if (isPushingLeft && BGMVolume > VOLUME_MIN) {
					BGMVolume -= (Time.realtimeSinceStartup - inputStartTime) / 15;
				}

				if(isStartRight == true) {
					BGMVolume++;
					inputStartTime = Time.realtimeSinceStartup;
				}
				if(isStartLeft == true) {
					BGMVolume--;
					inputStartTime = Time.realtimeSinceStartup;

				}

			}

			if (optionCursorPos == (int)OptionCursor.SE) {
				if (isPushingRight && SEVolume < VOLUME_MAX) {
					SEVolume += (Time.realtimeSinceStartup - inputStartTime) / 15;
				} else if (isPushingLeft && SEVolume > VOLUME_MIN) {
					SEVolume -= (Time.realtimeSinceStartup - inputStartTime) / 15;
				}

				if(isStartRight == true) {
					SEVolume++;
					inputStartTime = Time.realtimeSinceStartup;

				}
				if (isStartLeft) {
					SEVolume--;
					inputStartTime = Time.realtimeSinceStartup;
				}
			}

			if(optionCursorPos == (int)OptionCursor.Window && isScreenSelect == true) {
				switch (optionScreenCursorPos) {
					case (int)WindowOptionCursor.IsFullScreen:

						Debug.Log(isFullScreen);
						if(isFullScreen == true && isStartLeft == true) {
							isFullScreen = false;
						}
						if(isFullScreen == false && isStartRight == true) {
							isFullScreen = true;
						}
						break;
					case (int)WindowOptionCursor.Size:
						if(isStartLeft == true) {
							windowSizeNum--;
						}
						if(isStartRight == true) {
							windowSizeNum++;
						}
						windowSizeNum = windowSizeNum % WINDOW_RESOLUTION_SET.Length;
						if(windowSizeNum < 0) {
							windowSizeNum = WINDOW_RESOLUTION_SET.Length - 1;
						}
						break;
					case (int)WindowOptionCursor.Back:
						if(isStartLeft || isEnter) {
							isScreenSelect = false;
						}
						break;
					case (int)WindowOptionCursor.Aplly:
						isScreenSelect = false;
						ChangeScreenSize(windowSizeNum);
						break;

				}

				if (isStartUp) {
					optionScreenCursorPos--;
				}
				if (isStartDown) {
					optionScreenCursorPos++;
				}

				optionScreenCursorPos = (optionScreenCursorPos + (int)WindowOptionCursor.Back + 1) % ((int)WindowOptionCursor.Back + 1);

				if(isFullScreen == false) {
					fullScreenImage.sprite = fullScreenSprite[0];
					FullScreenArrow[0].SetActive(false);
					FullScreenArrow[1].SetActive(true);
				} else {
					fullScreenImage.sprite = fullScreenSprite[1];
					FullScreenArrow[0].SetActive(true);
					FullScreenArrow[1].SetActive(false);
				}

			} else {
				optionScreenSelect.SetActive(false);
			}

		} else {
			optionWindow.SetActive(false);
			optionScreenSelect.SetActive(false);
		}


	}

	private void Restart() {
		for (int i = 0; i < GEAR_NUM_MAX; i++) {
			if (i % 2 == 0) {
				gears[(int)MainCursorManu.Restart, i].transform.Rotate(0, 0, 1);
			} else {
				gears[(int)MainCursorManu.Restart, i].transform.Rotate(0, 0, -1);
			}
		}

		if (isConfirmation == true) {
			confirmation.SetActive(true);
		} else {
			confirmation.SetActive(false);
		}

		bool isEnter = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Return);
		bool isBack = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Backspace);

		bool isStartUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
		bool isStartDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);

		if(isEnter == true) {
			if(isConfirmation == true) {
				if(exitCorsorPos == 0) {
					SceneManager.LoadScene("Game");
				} else {
					isConfirmation = false;
				}
			} else {
				isConfirmation = true;
			}
		}

		if (isConfirmation == true) {
			if (isBack == true) {
				isConfirmation = false;
			}
			if(exitCorsorPos == 0) {
				if(isStartDown)	exitCorsorPos = 1;
			} else {
				if (isStartUp) exitCorsorPos = 0;
			}
		}
	}

	private void Exit() {
		for (int i = 0; i < GEAR_NUM_MAX; i++) {
			if (i % 2 == 0) {
				gears[(int)MainCursorManu.Exit, i].transform.Rotate(0, 0, 1);
			} else {
				gears[(int)MainCursorManu.Exit, i].transform.Rotate(0, 0, -1);
			}
		}

		if (isConfirmation == true) {
			confirmation.SetActive(true);
		} else {
			confirmation.SetActive(false);
		}

		bool isEnter = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Return);
		bool isBack = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Backspace);

		bool isStartUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
		bool isStartDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);

		if (isEnter == true) {
			if (isConfirmation == true) {
				if (exitCorsorPos == 0) {
					SceneManager.LoadScene("StageSelect");
				} else {
					isConfirmation = false;
				}
			} else {
				isConfirmation = true;
			}
		}

		if(isConfirmation == true) {
			if(isBack == true) {
				isConfirmation = false;
			}

			if (exitCorsorPos == 0) {
				if (isStartDown) exitCorsorPos = 1;
			} else {
				if (isStartUp) exitCorsorPos = 0;
			}
		}
	}



	/// <summary>
	/// 画面サイズの変更
	/// </summary>
	/// <param name="setNum">解像度セットの番号</param>
	private void ChangeScreenSize(int setNum) {
		Screen.SetResolution(WINDOW_RESOLUTION_SET[setNum, 0], WINDOW_RESOLUTION_SET[setNum, 1], isFullScreen);
	}


	/// <summary>
	/// フルスクリーンにするかを変更する
	/// </summary>
	private void ChangeIsFullScreen() {
		isFullScreen = !isFullScreen;
	}

	private void RotateMainButtonGear() {

	}
}
