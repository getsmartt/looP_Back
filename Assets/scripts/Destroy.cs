using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {


	
	void OnTriggerEnter2D (Collider2D other){
		Debug.Log ("Hit - " + other.name);
		if (other.name == "capture")
		{
			Destroy (gameObject);
			
		}
}
}
