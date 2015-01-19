using UnityEngine;
using System.Collections;

public class ChooseLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown () {

		Debug.Log (Application.loadedLevel + 1);
		Debug.Log (Application.levelCount);

		if (Application.loadedLevel + 1 < Application.levelCount ) {
						//Goto next level in index
						Application.LoadLevel (Application.loadedLevel + 1);
				} else {
						// No more Levels? Go back to first level
						Application.LoadLevel (0);
						Debug.Log ("hey");
				}

	}
}
