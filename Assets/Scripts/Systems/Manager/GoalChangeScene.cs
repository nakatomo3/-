using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalChangeScene : MonoBehaviour
{
    private float changeSceneCounter = 0;
    public GameObject crackerPaperA;
    const float CHANGE_SCENE = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        CameraManager.instance.GetComponent<AudioSource>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        changeSceneCounter += Time.deltaTime;
        if(changeSceneCounter >= CHANGE_SCENE) {
            SceneManager.LoadScene("StageSelect");
        }
        if(changeSceneCounter>= CHANGE_SCENE - 1.5f) {
            Instantiate(Resources.Load("Prefabs/Systems/ClearFadeOut") as GameObject, transform.position, Quaternion.identity);
        }
        
        Instantiate(crackerPaperA, CameraManager.instance.transform);


    }
}
