using UnityEngine;
using System.Collections.Generic;

public class Mover : MonoBehaviour {
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
	bool processing = false;
	int lastCreated;
	private float delay =0f;
	public float delay_length = .5f;
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
	void Update()
	{
		if (delay > 0)
		{ delay = delay - Time.time;
			if (delay <0 ) {delay = 0;}
		}

		if (Input.GetMouseButton(0) && transform.hasChanged)
		{
			//Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

			//Vector3 newDotPosition = mouseRay.origin - mouseRay.direction / mouseRay.direction.y * mouseRay.origin.y;
			//Debug.Log (newDotPosition.x);
			Vector3 wantedPosition;


			distance = transform.position.z - Camera.main.transform.position.z;
			targetPos2 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
			targetPos2 = Camera.main.ScreenToWorldPoint(targetPos2);
		
			targetPos = new Vector3(Mathf.Lerp( Input.mousePosition.x, Input.mousePosition.x + mySize, .5f), Mathf.Lerp( Input.mousePosition.y, Input.mousePosition.y + mySize, .5f), distance);
			//Mathf.Lerp( Input.mousePosition.x, Input.mousePosition.x + mySize, Time.deltaTime * .03)
			targetPos = Camera.main.ScreenToWorldPoint(targetPos);


			transform.position = targetPos2;
			wantedPosition = transform.TransformPoint(-mySize, 0, 0);
			DistanceJoint2D dj = FindObjectOfType(typeof(DistanceJoint2D))as DistanceJoint2D;

			Vector3 newDotPosition = dj.transform.position;

			//Vector3 newDotPosition =  Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * smooth);
			if (newDotPosition != lastDotPosition)
			{
				MakeADot(newDotPosition);
			}
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
			//colliderKeeper.transform.position = lastDotPosition;
			colliderKeeper.transform.LookAt(newDotPosition);
			bc.size = new Vector3(0.1f, 0.1f, Vector3.Distance(newDotPosition, lastDotPosition));
			bc.tag= "Trail";
			bc.isTrigger = true;
			Rigidbody2D rb = colliderKeeper.AddComponent<Rigidbody2D>();
			rb.gravityScale=0;

			Destroy (colliderKeeper,lifetime);
			//Transform dot =(Transform) Instantiate(DotPrefab, colliderKeeper.transform.position, Quaternion.identity); //use random identity to make dots looks more different
			//Destroy (dot.gameObject,lifetime);

			lastCreated = colliderKeeper.GetInstanceID();
			colliderKeeper.name = "" + colliderKeeper.GetInstanceID();

			//dot.name = lastCreated;
			//dot.LookAt (transform);
		}
		lastDotPosition = newDotPosition;

		lastPointExists = true;
	}
	void OnTriggerEnter2D (Collider2D other){

		if (other.tag != "Trail" && other.name != "capture")
		{
		Destroy (gameObject);
		
		}

		if (other.tag == "Trail" && delay == 0)
						Debug.Log ("" + lastCreated + " , " + other.GetInstanceID());

		{
			processing = true;
			delay = Time.time + delay_length;
			GameObject[] points;
			List<Vector2> verticesList = new List<Vector2>();

			points = GameObject.FindGameObjectsWithTag("Trail");
			
			foreach (GameObject point in points){

				if (point.GetInstanceID()>= lastCreated && point.GetInstanceID()<= other.GetInstanceID())
				{
					verticesList.Add(new Vector2(point.transform.position.x,point.transform.position.y)) ;
				}
				//Destroy(point);
			}
			if(verticesList.Count >2)
			{
				// Use the triangulator to get indices for creating triangles

				Vector2[] vertices2D = verticesList.ToArray();
				//Triangulator tr = new Triangulator(vertices2D);
				//int[] indices = tr.Triangulate();

				// Create the Vector3 vertices
				//Vector3[] vertices = new Vector3[vertices2D.Length];
				//for (int i=0; i<vertices.Length; i++) {
//					vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
//				}
//
//				Vector2[] uvs = new Vector2[vertices.Length];
//				int ii = 0;
//				while (ii < uvs.Length) {
//					uvs[ii] = new Vector2(vertices[ii].x, vertices[ii].z);
//					ii++;
//				}
//				
//				// Create the mesh
//				Mesh msh = new Mesh();
//				msh.vertices = vertices;
//				msh.triangles = indices;
//				msh.RecalculateNormals();
//				msh.RecalculateBounds();
//				msh.uv = uvs;
				
				// Set up game object with mesh;
				GameObject colliderMesh = new GameObject("capture");
				PolygonCollider2D bc = colliderMesh.AddComponent<PolygonCollider2D>();
				bc.points = vertices2D;
				//MeshRenderer mr = colliderMesh.AddComponent<MeshRenderer>();

				//MeshFilter filter = colliderMesh.AddComponent<MeshFilter>();
				//filter.mesh = msh;

				bc.isTrigger = true;
				Rigidbody2D rb = colliderMesh.AddComponent<Rigidbody2D>();
				rb.gravityScale=0;
				Destroy (colliderMesh,.5f);
				//bc.tag = "Capture";
			}
			processing = false;
		}
	}
}