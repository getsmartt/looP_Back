using UnityEngine;
using System.Collections.Generic;

public class Rotator : MonoBehaviour 
{
	Vector2 previousMousePosition = Vector2.zero;
	Vector2 currentMousePosition = Vector2.zero;
	bool previousFrameMouseDown = false;
	
	private void Update() 
	{
		Vector2 mousePos = new Vector2(Input.mousePosition.x, 
		                               Input.mousePosition.y);
		
		if (Input.GetMouseButtonDown(0) && !previousFrameMouseDown)
		{
			previousMousePosition = mousePos;
			currentMousePosition = mousePos;
			previousFrameMouseDown = true;
		}
		else if (Input.GetMouseButton(0) && previousFrameMouseDown)
		{
			previousMousePosition = currentMousePosition;
			currentMousePosition = mousePos;
		}
		else if (!Input.GetMouseButton(0))
		{
			previousFrameMouseDown = false;	
		}
		
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
		Vector2 screenPositionXY = new Vector2(screenPosition.x, screenPosition.y);
		Vector2 previousPositionVector = previousMousePosition - screenPositionXY;
		Vector2 currentPositionVector = currentMousePosition - screenPositionXY;
		
		if (previousPositionVector != -currentPositionVector && previousFrameMouseDown)
		{
			float rotationAmount = ReturnSignedAngleBetweenVectors(previousPositionVector,
			                                                       currentPositionVector);
			
			transform.RotateAroundLocal(Vector3.forward, rotationAmount * Time.deltaTime);
			rigidbody2D.AddForce(mousePos);
		}
	}
	
	
	private float ReturnSignedAngleBetweenVectors(Vector2 vectorA, Vector2 vectorB)
	{
		Vector3 vector3A = new Vector3(vectorA.x, vectorA.y, 0f);
		Vector3 vector3B = new Vector3(vectorB.x, vectorB.y, 0f);
		
		if (vector3A == vector3B)
			return 0f;
		
		// refVector is a 90cw rotation of vector3A
		Vector3 refVector = Vector3.Cross(vector3A, Vector3.forward);
		float dotProduct = Vector3.Dot(refVector, vector3B);
		
		if (dotProduct > 0)
			return -Vector3.Angle(vector3A, vector3B);
		else if (dotProduct < 0)
			return Vector3.Angle(vector3A, vector3B);
		else
			throw new System.InvalidOperationException("the vectors are opposite");
	}
}
