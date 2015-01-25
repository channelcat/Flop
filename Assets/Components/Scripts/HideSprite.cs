using UnityEngine;
using System.Collections;

public class HideSprite : MonoBehaviour {
	void Start () {
        Destroy(gameObject.GetComponent<SpriteRenderer> ());
	}
}
