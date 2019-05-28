using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour {

	public static OptionManager instance;
	private bool isFullScreen = false;
	private int width, height;

    [HideInInspector]
    public bool isPause = false;

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
	public Text[] screenWidthText = new Text[2];
	public Text[] screenHeightText = new Text[2];

	public GameObject canvas;

	[HideInInspector]
	public float SEVolume = 50;
	[HideInInspector]
	public float BGMVolume = 50;
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

	public Transform[] LightParent = new Transform[8];
	private Image[,] Light;

	public GameObject[] ArrowParent = new GameObject[4];

	[SerializeField]
	private bool isSelectScene = true;

    private bool joyStickVirticalUp = false;
    private bool joyStickVirticalDown = false;
    private bool joyStickHorizonLeft = false;
    private bool joyStickHorizonRight = false;

    private bool inputOnceUp = false;
    private bool inputOnceDown = false;
    private bool inputOnceLeft = false;
    private bool inputOnceRight = false;

    private AudioSource GearAudioSource;
    private AudioSource CorsorAudioSorce;
    public AudioClip CorsorMoveSound;
    public AudioClip CorsorEnterSound;
    private bool isAudioOnece = true;
    private const float ENTER_PITCH = 0.85f;
    private const float BACK_PITCH = 0.5f;
    private const float MOVE_PITCH = 1;
    private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        GearAudioSource = audioSources[0];
        CorsorAudioSorce = audioSources[1];

        Time.timeScale = 1f;

		BGMVolume = PlayerPrefs.GetInt("BGMVolume", 50);
		SEVolume = PlayerPrefs.GetInt("SEVolume", 50);

		for(int i = 0; i < GearParents.Length; i++) {
			for (int j = 0; j < GEAR_NUM_MAX; j++) {
				gears[i, j] = GearParents[i].transform.GetChild(j).gameObject;
			}
		}

		Light = new Image[LightParent.Length,2];
		for(int i= 0; i < LightParent.Length; i++) {
			Light[i, 0] = LightParent[i].GetChild(0).GetComponent<Image>();
			Light[i, 1] = LightParent[i].GetChild(1).GetComponent<Image>();
		}

		if(SceneManager.GetActiveScene().name == "Game") {
			isSelectScene = false;
		}
	}

    // Update is called once per frame
    void Update() {
        if (isAudioOnece == true) {
            GearAudioSource.Play();
            isAudioOnece = false;

        } else if (isPause == false) {
            GearAudioSource.Pause();
        }

		if(BGMVolume >= VOLUME_MAX) BGMVolume = VOLUME_MAX;
		if(SEVolume >= VOLUME_MAX)	SEVolume = VOLUME_MAX;
		if (BGMVolume <= VOLUME_MIN) BGMVolume = VOLUME_MIN;
		if(SEVolume <= VOLUME_MIN) SEVolume = VOLUME_MIN;

		BGMValue.text = Mathf.RoundToInt(BGMVolume).ToString();
		SEValue.text = Mathf.RoundToInt(SEVolume).ToString();

        //-----------------------ジョイスティックの横軸入力
        if (Input.GetAxis("GamePadStickHolizontal") > 0.5 && Input.GetAxis("GamePadStickHolizontal") <= 1) {
            if (inputOnceRight == false) {
                joyStickHorizonRight = true;
            } else {
                joyStickHorizonRight = false;
            }
            inputOnceRight = true;
        } else {
            joyStickHorizonRight = false;
            inputOnceRight = false;
        }
        if (Input.GetAxis("GamePadStickHolizontal") < -0.5 && Input.GetAxis("GamePadStickHolizontal") >= -1) {
            if (inputOnceLeft == false) {
                joyStickHorizonLeft = true;
            } else {
                joyStickHorizonLeft = false;
            }
            inputOnceLeft = true;

        } else {
            joyStickHorizonLeft = false;
            inputOnceLeft = false;
        }

        //--------------------ジョイスティックの縦軸の入力
        if (Input.GetAxis("GamePadStickVirtical") < -0.5 && Input.GetAxis("GamePadStickVirtical") >= -1) {
            if(inputOnceUp == false) {
                joyStickVirticalUp = true;
            } else {
                joyStickVirticalUp = false;
            }
            inputOnceUp = true;

        } else {
            joyStickVirticalUp = false;
            inputOnceUp = false;

        }
        if (Input.GetAxis("GamePadStickVirtical") > 0.5 && Input.GetAxis("GamePadStickVirtical") <= 1 ) {
            if (inputOnceDown == false) {
                joyStickVirticalDown = true;
            } else {
                joyStickVirticalDown = false;
            }
            inputOnceDown = true;

        } else {
            joyStickVirticalDown = false;
            inputOnceDown = false;
        }

        bool isInputRightStart = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || joyStickHorizonRight;
		bool isInputLeftStart = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || joyStickHorizonLeft;
		bool isInputUpStart = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || joyStickVirticalUp;
		bool isInputDownStart = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || joyStickVirticalDown;

		bool isInputRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetAxis("GamePadStickHolizontal") > 0.3;
		bool isInputLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetAxis("GamePadStickHolizontal") < -0.3;
		bool isInputUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetAxis("GamePadStickVirtical") < -0.3;
		bool isInputDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetAxis("GamePadStickVirtical") > 0.3;

        //ＳＥ再生
        if (isPause == true) {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || joyStickHorizonRight ||
                Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || joyStickHorizonLeft ||
                Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || joyStickVirticalUp ||
                Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || joyStickVirticalDown) {
                CorsorAudioSorce.PlayOneShot(CorsorMoveSound);

            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("GamePadB") ||
             Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("GamePadA")) {
                CorsorAudioSorce.PlayOneShot(CorsorEnterSound);

            }
        }




        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("GamePadStart")) {
			if(isPause == true) {////////////////////////////////////////////////////////////////////////
                CorsorAudioSorce.pitch = BACK_PITCH;
                CorsorAudioSorce.PlayOneShot(CorsorEnterSound);
                isPause = false;
				Time.timeScale = 1f;
				canvas.SetActive(false);
				mainCursorPos = 0;
				isConfirmation = false;
				isOpeningOptionWindow = false;
				isSelectingBGM_SE = false;
				isScreenSelect = false;

			} else {/////////////////////////////////////////////////////////////////////////////////
                CorsorAudioSorce.pitch = ENTER_PITCH;
                CorsorAudioSorce.PlayOneShot(CorsorEnterSound);
                isPause = true;
				Time.timeScale = 0f;
				canvas.SetActive(true);
                isAudioOnece = true;

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

				if (isInputUpStart) {///////////////////////////////////////////////////////
                    CorsorAudioSorce.pitch = MOVE_PITCH;
                    CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                    mainCursorPos--;
				}
				if (isInputDownStart) {/////////////////////////////////////////////////////////////
                    CorsorAudioSorce.pitch = MOVE_PITCH;
                    CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                    mainCursorPos++;
				}


				if (isSelectScene == false) {
					if (mainCursorPos > (int)MainCursorManu.Exit) {
						mainCursorPos = 0;
					}
					if (mainCursorPos < 0) {
						mainCursorPos = (int)MainCursorManu.Exit;
					}
				} else {
					if (mainCursorPos > (int)MainCursorManu.Option) {
						mainCursorPos = 0;
					}
					if (mainCursorPos < 0) {
						mainCursorPos = (int)MainCursorManu.Option;
					}
				}
			}

			
				for (int i = 1; i < CameraManager.instance.gameObject.transform.childCount; i++) {
				CameraManager.instance.gameObject.transform.GetChild(i).gameObject.SetActive(false);
			}

        
        } else {
            isConfirmation = false;
            isScreenSelect = false;
            optionWindow.SetActive(false);
            optionScreenSelect.SetActive(false);
            confirmation.SetActive(false);

			if (SceneManager.GetActiveScene().name == "Game") {
				for (int i = 1; i < CameraManager.instance.gameObject.transform.childCount; i++) {
					CameraManager.instance.gameObject.transform.GetChild(i).gameObject.SetActive(true);
				}
			}
		}
	}

	/// <summary>
	/// Restartを選択している状態の関数
	/// </summary>
	private void Continue() {
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("GamePadB")) {/////////////////////////////////////////////////////////////////
            CorsorAudioSorce.pitch = ENTER_PITCH;
            CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
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

		for(int i = 0; i < (int)MainCursorManu.Exit+1; i++) {
			if(i == (int)MainCursorManu.Continue) {
				Light[i, 0].color = Color.yellow;
				Light[i, 1].color = Color.yellow;
			} else {
				Light[i, 0].color = Color.white;
				Light[i, 1].color = Color.white;
			}
			if(i >= (int)MainCursorManu.Option && isSelectScene) {
				break;
			}
		}
		
	}

	private void Option() {
		bool isEnter = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || joyStickHorizonRight || Input.GetButtonDown("GamePadB");
		bool isBack = Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || joyStickHorizonLeft || Input.GetButtonDown("GamePadA");

		for (int i = 0; i < GEAR_NUM_MAX; i++) {
			if (i % 2 == 0) {
				gears[(int)MainCursorManu.Option, i].transform.Rotate(0, 0, 1);
			} else {
				gears[(int)MainCursorManu.Option, i].transform.Rotate(0, 0, -1);
			}
		}

		for (int i = 0; i < (int)MainCursorManu.Exit + 1; i++) {
			if (i == (int)MainCursorManu.Option) {
				Light[i, 0].color = Color.yellow;
				Light[i, 1].color = Color.yellow;
			} else {
				Light[i, 0].color = Color.white;
				Light[i, 1].color = Color.white;
			}
			if (i >= (int)MainCursorManu.Option && isSelectScene) {
				break;
			}

		}

		if (isEnter == true) {/////////////////////////////////////////
            CorsorAudioSorce.pitch = ENTER_PITCH;
            if (isOpeningOptionWindow == true) {

				switch (optionCursorPos) {
					default:
						Debug.LogError("存在しないOptionCursorを選択しています");
						break;
					case (int)OptionCursor.BGM:
                        CorsorAudioSorce.pitch = ENTER_PITCH;
                        CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                        break;
					case (int)OptionCursor.SE:
                        CorsorAudioSorce.pitch = ENTER_PITCH;
                        CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                        break;
					case (int)OptionCursor.Window:
                        CorsorAudioSorce.pitch = ENTER_PITCH;
                        CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                        if (isScreenSelect == false) {
							isScreenSelect = true;
							optionScreenCursorPos = (int)WindowOptionCursor.IsFullScreen;
							optionScreenSelect.SetActive(true);
							windowSizeNum = 0;
						}
						break;
					case (int)OptionCursor.Back:
                        CorsorAudioSorce.pitch = BACK_PITCH;
                        CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                        isOpeningOptionWindow = false;
						optionScreenSelect.SetActive(false);
						break;
				}
			} else {
				isOpeningOptionWindow = true;
			}
		}
		if (isBack == true) {//////////////////////////////////////////////////
            CorsorAudioSorce.pitch = BACK_PITCH;
            CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
            if (isOpeningOptionWindow == true) {
				switch (optionCursorPos) {
					case (int)OptionCursor.Back:
						break;
				}
			}
		}

		bool isPushingRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetAxis("GamePadStickHolizontal") > 0.3;
        bool isPushingLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetAxis("GamePadStickHolizontal") < -0.3;
		bool isStartRight = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || joyStickHorizonRight;
		bool isStartLeft = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || joyStickHorizonLeft;

		bool isStartUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || joyStickVirticalUp;
		bool isStartDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || joyStickVirticalDown;

		if (isOpeningOptionWindow == true) {
			optionWindow.SetActive(true);

			if (isSelectingBGM_SE == false && isScreenSelect == false) {
				if (isStartUp) {///////////////////////////////////////////////////////////////////////////////////////////
                    CorsorAudioSorce.pitch = MOVE_PITCH;
                    CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                    optionCursorPos--;
				}
				if (isStartDown) {///////////////////////////////////////////////////////////////////////////////////////////
                    CorsorAudioSorce.pitch = MOVE_PITCH;
                    CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                    optionCursorPos++;
				}

				optionCursorPos = (optionCursorPos + (int)OptionCursor.Back + 1) % ((int)OptionCursor.Back+1);
			}

			switch (optionCursorPos) {
				case (int)OptionCursor.BGM:
					if (isPushingRight && BGMVolume < VOLUME_MAX) {///////////////////////////////////////////////////////////////////////////////
						BGMVolume += (Time.realtimeSinceStartup - inputStartTime) / 15;
					} else if (isPushingLeft && BGMVolume > VOLUME_MIN) {
						BGMVolume -= (Time.realtimeSinceStartup - inputStartTime) / 15;
					}

					if (isStartRight == true) {//////////////////////////////////////////////////////////
                        CorsorAudioSorce.pitch = MOVE_PITCH;
                        CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                        BGMVolume++;
						inputStartTime = Time.realtimeSinceStartup;
					}
					if (isStartLeft == true) {////////////////////////////////////////////////////////////
                        CorsorAudioSorce.pitch = MOVE_PITCH;
                        CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                        BGMVolume--;
						inputStartTime = Time.realtimeSinceStartup;
					}
					PlayerPrefs.SetInt("BGMVolume", Mathf.FloorToInt(BGMVolume));


					for (int i = 0; i < ArrowParent.Length; i++) {
						if (i == 0) {
							ArrowParent[i].SetActive(true);
						} else {
							ArrowParent[i].SetActive(false);
						}
					}
					for (int i = 4; i < 5 + 1; i++) {
						Light[i, 0].color = Color.white;
						Light[i, 1].color = Color.white;
					}
					break;
				case (int)OptionCursor.SE:
					if (isPushingRight && SEVolume < VOLUME_MAX) {/////////////////////////////////////////////////////////////////
						SEVolume += (Time.realtimeSinceStartup - inputStartTime) / 15;
					} else if (isPushingLeft && SEVolume > VOLUME_MIN) {
						SEVolume -= (Time.realtimeSinceStartup - inputStartTime) / 15;
					}

					if (isStartRight == true) {/////////////////////////////////////////////////////
                        CorsorAudioSorce.pitch = MOVE_PITCH;
                        CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                        SEVolume++;
						inputStartTime = Time.realtimeSinceStartup;

					}
					if (isStartLeft) {///////////////////////////////////////////////////////////
                        CorsorAudioSorce.pitch = MOVE_PITCH;
                        CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                        SEVolume--;
						inputStartTime = Time.realtimeSinceStartup;
					}
					PlayerPrefs.SetInt("SEVolume", Mathf.FloorToInt(SEVolume));

					for (int i = 0; i < ArrowParent.Length; i++) {
						if (i == 1) {
							ArrowParent[i].SetActive(true);
						} else {
							ArrowParent[i].SetActive(false);
						}
					}
					for (int i = 4; i < 5 + 1; i++) {
						Light[i, 0].color = Color.white;
						Light[i, 1].color = Color.white;
					}
					break;
				case (int)OptionCursor.Window:
					for (int i = 4; i < 5+1; i++) {
						if (i == 4) {
							Light[i, 0].color = Color.yellow;
							Light[i, 1].color = Color.yellow;
						} else {
							Light[i, 0].color = Color.white;
							Light[i, 1].color = Color.white;
						}
					}
					for (int i = 0; i < ArrowParent.Length; i++) {						
						ArrowParent[i].SetActive(false);
					}
					break;
				case (int)OptionCursor.Back:
					for(int i = 4; i < 5+1; i++) {
						if(i == 5) {
							Light[i, 0].color = Color.yellow;
							Light[i, 1].color = Color.yellow;
						} else {
							Light[i, 0].color = Color.white;
							Light[i, 1].color = Color.white;
						}
					}
					for (int i = 0; i < ArrowParent.Length; i++) {
						ArrowParent[i].SetActive(false);
					}
					break;
			}

			if(optionCursorPos == (int)OptionCursor.Window && isScreenSelect == true) {
				switch (optionScreenCursorPos) {
					case (int)WindowOptionCursor.IsFullScreen:
						if(isFullScreen == true && isStartLeft == true) {
							isFullScreen = false;
						}
						if(isFullScreen == false && isStartRight == true) {
							isFullScreen = true;
						}
						for (int i = 0; i < ArrowParent.Length; i++) {
							if (i == 2) {
								ArrowParent[i].SetActive(true);
							} else {
								ArrowParent[i].SetActive(false);
							}
						}
						for (int i = 6; i < 7 + 1; i++) {
							Light[i, 0].color = Color.white;
							Light[i, 1].color = Color.white;
						}
						break;
					case (int)WindowOptionCursor.Size://///////////////////////////////////////////////
						if(isStartLeft == true) {
                            CorsorAudioSorce.pitch = MOVE_PITCH;
                            CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                            windowSizeNum--;
						}
						if(isStartRight == true) {
                            CorsorAudioSorce.pitch = MOVE_PITCH;
                            CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                            windowSizeNum++;
						}
						windowSizeNum = (windowSizeNum + WINDOW_RESOLUTION_SET.Length/2) % (WINDOW_RESOLUTION_SET.Length/2);
						screenWidthText[0].text = WINDOW_RESOLUTION_SET[windowSizeNum, 0].ToString();
						screenWidthText[1].text = WINDOW_RESOLUTION_SET[windowSizeNum, 0].ToString();
						screenHeightText[0].text = WINDOW_RESOLUTION_SET[windowSizeNum, 1].ToString();
						screenHeightText[1].text = WINDOW_RESOLUTION_SET[windowSizeNum, 1].ToString();

						for (int i = 0; i < ArrowParent.Length; i++) {
							if (i == 3) {
								ArrowParent[i].SetActive(true);
							} else {
								ArrowParent[i].SetActive(false);
							}
						}
						for (int i = 6; i < 7 + 1; i++) {
							Light[i, 0].color = Color.white;
							Light[i, 1].color = Color.white;
						}
						break;
					case (int)WindowOptionCursor.Back:///////////////////////////////////////////////////
						if(isStartRight || isEnter) {
                            CorsorAudioSorce.pitch = BACK_PITCH;
                            CorsorAudioSorce.PlayOneShot(CorsorEnterSound);
                            isScreenSelect = false;
							optionCursorPos = (int)OptionCursor.Window;
						}
						for (int i = 6; i < 7 + 1; i++) {
							if (i == 6) {
								Light[i, 0].color = Color.yellow;
								Light[i, 1].color = Color.yellow;
							} else {
								Light[i, 0].color = Color.white;
								Light[i, 1].color = Color.white;
							}
						}
						for (int i = 0; i < ArrowParent.Length; i++) {
							ArrowParent[i].SetActive(false);
						}
						break;
					case (int)WindowOptionCursor.Aplly://///////////////////////////////////////
						if (isStartRight || isEnter) {
                            CorsorAudioSorce.pitch = ENTER_PITCH;
                            CorsorAudioSorce.PlayOneShot(CorsorEnterSound);
                            isScreenSelect = false;
							ChangeScreenSize(windowSizeNum);
						}
						for (int i = 6; i < 7 + 1; i++) {
							if (i == 7) {
								Light[i, 0].color = Color.yellow;
								Light[i, 1].color = Color.yellow;
							} else {
								Light[i, 0].color = Color.white;
								Light[i, 1].color = Color.white;
							}
						}
						for (int i = 0; i < ArrowParent.Length; i++) {
							ArrowParent[i].SetActive(false);
						}
						break;
				}

                //////////////////////////////////////////////////////////////////////////////////////
				if (isStartUp) {
                    CorsorAudioSorce.pitch = MOVE_PITCH;
                    CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                    optionScreenCursorPos--;
				}
				if (isStartDown) {
                    CorsorAudioSorce.pitch = MOVE_PITCH;
                    CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                    optionScreenCursorPos++;
				}

				optionScreenCursorPos = (optionScreenCursorPos + (int)WindowOptionCursor.Aplly + 1) % ((int)WindowOptionCursor.Aplly + 1);

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

		for (int i = 0; i < (int)MainCursorManu.Exit + 1; i++) {
			if (i == (int)MainCursorManu.Restart) {
				Light[i, 0].color = Color.yellow;
				Light[i, 1].color = Color.yellow;
			} else {
				Light[i, 0].color = Color.white;
				Light[i, 1].color = Color.white;
			}

		}

		bool isEnter = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("GamePadB");
		bool isBack = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("GamePadA");

		bool isStartUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)||joyStickVirticalUp;
		bool isStartDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)||joyStickVirticalDown;

		if(isEnter == true) {
            CorsorAudioSorce.pitch = ENTER_PITCH;
            if (isConfirmation == true) {//////////////////////////////////////////////////
				if(exitCorsorPos == 0) {
                    CorsorAudioSorce.pitch = ENTER_PITCH;
                    CorsorAudioSorce.PlayOneShot(CorsorEnterSound);
                    SceneManager.LoadScene("Game");
				} else {
                    CorsorAudioSorce.pitch = BACK_PITCH;
                    CorsorAudioSorce.PlayOneShot(CorsorEnterSound);
                    isConfirmation = false;
				}
			} else {
                         isConfirmation = true;
			}
        }

		if (isConfirmation == true) {
			if (isBack == true) {//////////////////////////////////////////////////////
                CorsorAudioSorce.pitch = BACK_PITCH;
                CorsorAudioSorce.PlayOneShot(CorsorEnterSound);
                isConfirmation = false;
			}
			if(exitCorsorPos == 0) {
                if (isStartDown) {
                    CorsorAudioSorce.pitch = MOVE_PITCH;
                    CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                    exitCorsorPos = 1;//////////////////////////////////////////////////////////
                }
				for(int i = 8; i < 9+1; i++) {
					if(i == 8) {
						Light[i, 0].color = Color.yellow;
						Light[i, 1].color = Color.yellow;
					} else {
						Light[i, 0].color = Color.white;
						Light[i, 1].color = Color.white;
					}
				}
			} else {
                if (isStartUp) {
                    CorsorAudioSorce.pitch = MOVE_PITCH;
                    CorsorAudioSorce.PlayOneShot(CorsorMoveSound);
                    exitCorsorPos = 0;////////////////////////////////////////////////////////////////////////////
                }
				for (int i = 8; i < 9 + 1; i++) {
					if (i == 9) {
						Light[i, 0].color = Color.yellow;
						Light[i, 1].color = Color.yellow;
					} else {
						Light[i, 0].color = Color.white;
						Light[i, 1].color = Color.white;
					}
				}
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

		for (int i = 0; i < (int)MainCursorManu.Exit + 1; i++) {
			if (i == (int)MainCursorManu.Exit) {
				Light[i, 0].color = Color.yellow;
				Light[i, 1].color = Color.yellow;
			} else {
				Light[i, 0].color = Color.white;
				Light[i, 1].color = Color.white;
			}

		}

		if (isConfirmation == true) {
			confirmation.SetActive(true);
		} else {
			confirmation.SetActive(false);
		}

		bool isEnter = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Return)|| joyStickHorizonRight || Input.GetButtonDown("GamePadB");
		bool isBack = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Backspace)|| joyStickHorizonLeft || Input.GetButtonDown("GamePadA");

		bool isStartUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)|| joyStickVirticalUp;
		bool isStartDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)|| joyStickVirticalDown;

		if (isEnter == true) {
            CorsorAudioSorce.pitch = ENTER_PITCH;
            if (isConfirmation == true) {
				if (exitCorsorPos == 0) {/////////////////////////////////////////////////
                    CorsorAudioSorce.pitch = ENTER_PITCH;
                    CorsorAudioSorce.PlayOneShot(CorsorEnterSound);
                    SceneManager.LoadScene("StageSelect");
				} else {
                    CorsorAudioSorce.pitch = BACK_PITCH;
                    CorsorAudioSorce.PlayOneShot(CorsorEnterSound);
                    isConfirmation = false;
				}
			} else {
				isConfirmation = true;
			}
        }

		if(isConfirmation == true) {
			if(isBack == true) {
                CorsorAudioSorce.pitch = BACK_PITCH;
                CorsorAudioSorce.PlayOneShot(CorsorEnterSound);
                isConfirmation = false;
			}

			if (exitCorsorPos == 0) {
				if (isStartDown) exitCorsorPos = 1;///////////////////////////////////////////////////////
				for (int i = 8; i < 9 + 1; i++) {
					if (i == 8) {
						Light[i, 0].color = Color.yellow;
						Light[i, 1].color = Color.yellow;
					} else {
						Light[i, 0].color = Color.white;
						Light[i, 1].color = Color.white;
					}
				}
			} else {
				if (isStartUp) exitCorsorPos = 0;/////////////////////////////////////////////////////////////
				for (int i = 8; i < 9 + 1; i++) {
					if (i == 9) {
						Light[i, 0].color = Color.yellow;
						Light[i, 1].color = Color.yellow;
					} else {
						Light[i, 0].color = Color.white;
						Light[i, 1].color = Color.white;
					}
				}
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
