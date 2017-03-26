using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Croc : MonoBehaviour {
    private Animator myAnim;
	// Use this for initialization
	void Start () {
        myAnim = GetComponent<Animator>();
	}
	
    void OnMouseDown() {
        myAnim.SetTrigger("IsTap");
    }
}
