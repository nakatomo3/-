using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public GameObject BreakPlayer;
    bool isBreakPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isBreakPlayer == true) {
            Instantiate(BreakPlayer, Player.instance.visualTransform.position, Quaternion.Euler(90, 0, 0)); //rigidbodyの空気抵抗を変えてアニメーションさせてる
        }
    }
}
