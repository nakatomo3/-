using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearFadeOut : MonoBehaviour
{
    public Image image;
    private float alpha = 0;

    // Start is called before the first frame update
    void Start() {
        //Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update() {
        image.color = new Color(1, 1, 1, alpha);
        alpha += Time.deltaTime * 0.13f;

    }
}
