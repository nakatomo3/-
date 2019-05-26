using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchdownSE : MonoBehaviour
{
    private AudioSource audioSource;
    private bool canSound=false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other) {
        //if (canSound == true) {
        //    audioSource.Play();
        //    canSound = false;
        //}
        if (Player.instance.isJumping == true) {
            if (other.gameObject.CompareTag("Ground")) {
                audioSource.Play();
            }
        }
    }
    //private void OnTriggerExit(Collider other) {
    //    canSound = true;
    //}
}
