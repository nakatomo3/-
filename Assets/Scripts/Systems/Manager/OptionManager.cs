using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour {

	public static OptionManager instance;
	private bool isFullScreen = false;
	private int width, height;

    private bool isPause = false;


    private int mainCursorPos = 0;
    private int restartCorsorPos = 1;
    private int optionSoundCorsorPos = 0;
    private int optionScreenCrsorPos = 0;
    private int exitCorsorPos = 1;
    private bool isConfirmation = false;
    private bool isScreenSelect = false;

    public GameObject mainRestartCursor;
    public GameObject restartConfirmation;
    public GameObject restartYesCorsor;
    public GameObject restartNoCorsor;

    public GameObject mainOptionCursor;
    public GameObject optionSound;
    public GameObject optionBGMCorsor;
    public GameObject optionSECorsor;
    public GameObject optionScreenCorsor;
    public GameObject optionSoundBackCorsor;
    public GameObject optionScreenSelect;
    public GameObject optionFullScreenCorsor;
    public GameObject optionWindowCorsor;
    public GameObject optionScreenBack;

    public GameObject mainExitCursor;
    public GameObject exitConfirmation;
    public GameObject exitYesCorsor;
    public GameObject exitNoCorsor;

	public Text BGMValue;
	public Text SEValue;

    private readonly int[,] SCREEN_RESOLUTION_SET = new int[5, 2]{
		{ 960 , 540 },
		{ 1024, 576 },
		{ 1280, 720 },
		{ 1920, 1080 },
		{ 2560, 1440 }
	};

	public GameObject canvas;

	private float SEVolume = 50;
	private float BGMVolume = 50;
	private const int VOLUME_MIN = 0;
	private const int VOLUME_MAX = 100;

	private float inputStartTime = 0;

	private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {
        Time.timeScale = 1f;
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
			} else {
				isPause = true;
				Time.timeScale = 0f;
				canvas.SetActive(true);
			}
		}

        //----------ポーズ中の処理------------------
        if (isPause == true) {
            if (isConfirmation == false) {
                //メインカーソル移動
                if (isInputUpStart) {
                    if (mainCursorPos <= 0) {
                        //none
                    } else {
                        mainCursorPos -= 1;
                    }

                } else if (isInputDownStart) {
                    if (mainCursorPos >= 2) {
                        //none
                    } else {
                        mainCursorPos += 1;
                    }
                }
            }

            //カーソルの位置を表示
            if (mainCursorPos == 0) {
                //-----------Restart-----------
                if (isConfirmation == false) {
                    mainRestartCursor.SetActive(true);
                    mainOptionCursor.SetActive(false);
                    mainExitCursor.SetActive(false);
                }
                
                if (isConfirmation == true) {
                    //カーソルの移動
                    if (isInputUpStart) {
                        if (restartCorsorPos == 0) {
                            //none
                        } else {
                            restartCorsorPos--;
                        }

                    } else if (isInputDownStart) {
                        if (restartCorsorPos == 1) {
                            //none
                        } else {
                            restartCorsorPos++;
                        }
                    }
                    //カーソルの表示
                    if (restartCorsorPos == 0) {
                        restartYesCorsor.SetActive(true);
                        restartNoCorsor.SetActive(false);

                    } else if (restartCorsorPos == 1) {
                        restartYesCorsor.SetActive(false);
                        restartNoCorsor.SetActive(true);
                    }

                    //Yes or Noを選択した時の処理
                    if (restartCorsorPos == 0 && Input.GetKeyDown(KeyCode.Return)) {
                        Debug.Log("Restartを選択した");
                        SceneManager.LoadScene("Game");

                    } else if (restartCorsorPos == 1 && Input.GetKeyDown(KeyCode.Return)) {
                        restartConfirmation.SetActive(false);
                        isConfirmation = false;
                        mainRestartCursor.SetActive(true);
                    }

                } else if (Input.GetKeyDown(KeyCode.Return)) {
                    restartConfirmation.SetActive(true);
                    isConfirmation = true;
                    mainRestartCursor.SetActive(false);
                }
                
            } else if (mainCursorPos == 1) {
                //------------Option
                if (isConfirmation == false) {
                    mainRestartCursor.SetActive(false);
                    mainOptionCursor.SetActive(true);
                    mainExitCursor.SetActive(false);
                }

                if (isConfirmation == true) {
                    if (isScreenSelect == false) {
                        //カーソルの移動
                        if (isInputUpStart) {
                            if (optionSoundCorsorPos == 0) {
                                //none
                            } else {
                                optionSoundCorsorPos--;
                            }

                        } else if (isInputDownStart) {
                            if (optionSoundCorsorPos == 3) {
                                //none
                            } else {
                                optionSoundCorsorPos++;
                            }
                        }

                        //カーソルの表示
                        if (optionSoundCorsorPos == 0) {
                            optionBGMCorsor.SetActive(true);
                            optionSECorsor.SetActive(false);
                            optionScreenCorsor.SetActive(false);
                            optionSoundBackCorsor.SetActive(false);

                        } else if (optionSoundCorsorPos == 1) {
                            optionBGMCorsor.SetActive(false);
                            optionSECorsor.SetActive(true);
                            optionScreenCorsor.SetActive(false);
                            optionSoundBackCorsor.SetActive(false);

                        } else if (optionSoundCorsorPos == 2) {
                            optionBGMCorsor.SetActive(false);
                            optionSECorsor.SetActive(false);
                            optionScreenCorsor.SetActive(true);
                            optionSoundBackCorsor.SetActive(false);

                        } else if (optionSoundCorsorPos == 3) {
                            optionBGMCorsor.SetActive(false);
                            optionSECorsor.SetActive(false);
                            optionScreenCorsor.SetActive(false);
                            optionSoundBackCorsor.SetActive(true);

                        }

                        //OptionSound
                        if (optionSoundCorsorPos == 0) {
							if (isInputRightStart) {
								inputStartTime = Time.realtimeSinceStartup;
								BGMVolume++;
							}
							if (isInputLeftStart) {
								BGMVolume--;
							}

                            //BGMの音量変更
                            //Debug.Log("BGMの音量を調節することが出来ます");
							if (isInputRight && BGMVolume < VOLUME_MAX) {
								BGMVolume += (Time.realtimeSinceStartup - inputStartTime) /15;
							} else if(isInputLeft && BGMVolume > VOLUME_MIN) {
								BGMVolume -= (Time.realtimeSinceStartup - inputStartTime) /15;
							}


                        } else if (optionSoundCorsorPos == 1) {
							if (isInputRightStart) {
								inputStartTime = Time.realtimeSinceStartup;
								SEVolume++;
							}
							if (isInputLeftStart) {
								inputStartTime = Time.realtimeSinceStartup;
								SEVolume--;
							}

							//SEの音量変更
							//Debug.Log("SEの音量を調整することが出来ます");
							if(isInputRight && SEVolume < VOLUME_MAX) {
								SEVolume += (Time.realtimeSinceStartup - inputStartTime) /20;
							} else if (isInputLeft && SEVolume > VOLUME_MIN) {
								SEVolume -= (Time.realtimeSinceStartup - inputStartTime) /20;
							}

						} else if (optionSoundCorsorPos == 2 && Input.GetKeyDown(KeyCode.Return)) {
                            //Screenの大きさを変更
                            isScreenSelect = true;
                            optionScreenCorsor.SetActive(false);
                            optionScreenSelect.SetActive(true);

                        } else if (optionSoundCorsorPos == 3 && Input.GetKeyDown(KeyCode.Return)) {
                            //戻る
                            optionSound.SetActive(false);
                            isConfirmation = false;
                            mainOptionCursor.SetActive(true);
                        }

                        //スクリーンの大きさを選択する
                    } else if(isScreenSelect==true) {
                        //カーソルの移動
                        if (isInputUpStart) {
                            if (optionScreenCrsorPos == 0) {
                                //none
                            } else {
                                optionScreenCrsorPos--;
                            }

                        } else if (isInputDownStart) {
                            if (optionScreenCrsorPos == 2) {
                                //none
                            } else {
                                optionScreenCrsorPos++;
                            }
                        }

                        //カーソルの表示
                        if (optionScreenCrsorPos == 0) {
                            optionFullScreenCorsor.SetActive(true);
                            optionWindowCorsor.SetActive(false);
                            optionScreenBack.SetActive(false);

                        } else if (optionScreenCrsorPos == 1) {
                            optionFullScreenCorsor.SetActive(false);
                            optionWindowCorsor.SetActive(true);
                            optionScreenBack.SetActive(false);

                        } else if (optionScreenCrsorPos == 2) {
                            optionFullScreenCorsor.SetActive(false);
                            optionWindowCorsor.SetActive(false);
                            optionScreenBack.SetActive(true);

                        }

                        //OptionSound
                        if (optionScreenCrsorPos == 0) {
                            //FullScreenに変更
                            Debug.Log("FullScreenにできます");
							isFullScreen = true;

                        } else if (optionScreenCrsorPos == 1) {
                            //Windowに変更
                            Debug.Log("Windowにできます");
							isFullScreen = false;

                        } else if (optionScreenCrsorPos == 2 && Input.GetKeyDown(KeyCode.Return)) {
                            //戻る
                            optionScreenSelect.SetActive(false);
                            isScreenSelect = false;
                            optionScreenCorsor.SetActive(true);
                        }
                    }

                } else if (Input.GetKeyDown(KeyCode.Return)) {
                    optionSound.SetActive(true);
                    isConfirmation = true;
                    mainOptionCursor.SetActive(false);
                }



            } else if (mainCursorPos == 2) {
                //-----------Exit------------
                if (isConfirmation == false) {
                    mainRestartCursor.SetActive(false);
                    mainOptionCursor.SetActive(false);
                    mainExitCursor.SetActive(true);
                }

                if (isConfirmation == true) {
                    //カーソルの移動
                    if (isInputUpStart) {
                        if (exitCorsorPos == 0) {
                            //none
                        } else {
                            exitCorsorPos--;
                        }

                    } else if (isInputDownStart) {
                        if (exitCorsorPos == 1) {
                            //none
                        } else {
                            exitCorsorPos++;
                        }
                    }
                    //カーソルの表示
                    if (exitCorsorPos == 0) {
                        exitYesCorsor.SetActive(true);
                        exitNoCorsor.SetActive(false);

                    } else if (exitCorsorPos == 1) {
                        exitYesCorsor.SetActive(false);
                        exitNoCorsor.SetActive(true);
                    }

                    //Yes or Noを選択した時の処理
                    if (exitCorsorPos == 0 && Input.GetKeyDown(KeyCode.Return)) {
                        Debug.Log("Exitを選択した");
                        SceneManager.LoadScene("StageSelect");

                    } else if (exitCorsorPos == 1 && Input.GetKeyDown(KeyCode.Return)) {
                        exitConfirmation.SetActive(false);
                        isConfirmation = false;
                        mainExitCursor.SetActive(true);
                    }

                } else if (Input.GetKeyDown(KeyCode.Return)) {
                    exitConfirmation.SetActive(true);
                    isConfirmation = true;
                    mainExitCursor.SetActive(false);
                }
            }

        } else {
            isConfirmation = false;
            isScreenSelect = false;
            restartConfirmation.SetActive(false);
            optionSound.SetActive(false);
            optionScreenSelect.SetActive(false);
            exitConfirmation.SetActive(false);
            
        }
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
