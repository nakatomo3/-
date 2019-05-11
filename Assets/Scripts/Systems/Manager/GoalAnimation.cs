using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalAnimation : MonoBehaviour
{
    private float changeSceneCounter = 0;

    const float CHANGE_SCENE = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        changeSceneCounter += Time.deltaTime;
        if(changeSceneCounter >= CHANGE_SCENE) {
            SceneManager.LoadScene("StageSelect");
        }
    }
}
