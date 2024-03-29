﻿using UnityEngine;
using System.Collections;

public class robotAnimScript : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Awake () {
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		CheckKey ();
	}

	void CheckKey(){
		// Walk
		if (Input.GetKey (KeyCode.W)) {
			anim.SetBool ("Walk_Anim", true);
		} 
		else if (Input.GetKeyUp (KeyCode.W)) {
			anim.SetBool ("Walk_Anim", false);
		}

		// Roll
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (anim.GetBool ("Roll_Anim")) {
				anim.SetBool ("Roll_Anim", false);
			}
			else {
				anim.SetBool ("Roll_Anim", true);
			}
		} 

		// Close
		if(Input.GetKeyDown(KeyCode.LeftControl)){
			if (!anim.GetBool ("Open_Anim")) {
				anim.SetBool ("Open_Anim", true);
			} 
			else {
				anim.SetBool ("Open_Anim", false);
			}
		}
	}
}
