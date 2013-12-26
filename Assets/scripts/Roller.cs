using UnityEngine;
using System.Collections.Generic;

public class Roller : MonoBehaviour 
{

	public Transform target;
	public float Speed = .1f;


	private void Update() 


	{
		if (!target)return; 
		float posX = transform.position.x - target.transform.position.x;

		transform.position = Vector3.Lerp(transform.position,target.position,Time.deltaTime * Speed);
		if (posX > 0) {
			transform.Rotate (Vector3.forward * Time.deltaTime*500);} 
		else {
			transform.Rotate (Vector3.back * Time.deltaTime*500);}

	}
	
	

}
