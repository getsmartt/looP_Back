using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {

	public Transform DotPrefab;
	public float speed = 3.0F;
	public float lifetime = 2.0f;
	private Vector3 targetPos;
	private Vector3 targetPos2;
	public float mySize = .6f;
	private float startTime;
	private float journeyLength;
	public float smooth = 5.0F;
	Vector3 lastDotPosition;
	float distance = 0;
	bool lastPointExists;
	string lastCreated;
	void Start()
	{
		startTime = Time.time;
		
		targetPos = transform.position; 
		lastPointExists = false;
		/*MakeADot(new Vector3(10, 0, 0));
		MakeADot(new Vector3(5, 10, 0));
		MakeADot(new Vector3(7, 7, 0));
		MakeADot(new Vector3(0, 0, 0));
		MakeADot(new Vector3(0, 10, 0));*/
		
		
	}
	
	// Update is called once per frame
	void Update () {
		targetPos = transform.position;
		Debug.Log ("" + targetPos.x);
		if (targetPos != lastDotPosition)
						{
							MakeADot(targetPos);
						}
	}

	void MakeADot(Vector3 newDotPosition)
	{

		
		if (lastPointExists)
		{
			GameObject colliderKeeper = new GameObject("collider");
			BoxCollider2D bc = colliderKeeper.AddComponent<BoxCollider2D>();
			journeyLength = (Vector3.Distance(lastDotPosition, newDotPosition));
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			colliderKeeper.transform.position = Vector3.Lerp(newDotPosition, lastDotPosition, fracJourney);
			colliderKeeper.transform.LookAt(newDotPosition);
			bc.size = new Vector3(0.1f, 0.1f, Vector3.Distance(newDotPosition, lastDotPosition));
			bc.tag= "Trail";
			bc.isTrigger = true;
			Rigidbody2D rb = colliderKeeper.AddComponent<Rigidbody2D>();
			rb.gravityScale=0;
			
			Destroy (colliderKeeper,lifetime);
			Transform dot =(Transform) Instantiate(DotPrefab, colliderKeeper.transform.position, Quaternion.identity); //use random identity to make dots looks more different
			Destroy (dot.gameObject,lifetime);
			
			lastCreated = "id" + Time.time;
			colliderKeeper.name = lastCreated;
			//dot.name = lastCreated;
			//dot.LookAt (transform);
		}
		lastDotPosition = newDotPosition;
		
		lastPointExists = true;
	}
}
