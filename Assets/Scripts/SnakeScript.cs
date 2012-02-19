using UnityEngine;
using System.Collections;

public class SnakeScript : MonoBehaviour {

	public Rigidbody ownHead;
	public Rigidbody ownTail;

	public float torqueFactor = 10f;
	public float moveFactor = 10f;
	public float buttThresh = 10f;

	[HideInInspector]
	public bool isFollowed;

	Rigidbody targetTail;
	SnakeScript targetScript = null;

	bool isAttachedToButt = false;

	public Vector3 attachOffset;
	public Vector3 eulerOffset;

	void Start () {
		
	}

	
	void FixedUpdate () {
	
		//if (targetTail == null) {
			
			//acquire target

			var snakes = GameObject.FindGameObjectsWithTag("snake");

			float distance = Mathf.Infinity;

			foreach (GameObject go in snakes) {

				var s = go.GetComponentInChildren<SnakeScript>();

			
				Vector3 d = transform.position - s.ownTail.transform.position;

				if (!s.isFollowed && distance > d.sqrMagnitude) {

					distance = d.sqrMagnitude;

					if (targetTail != null)
						targetScript.isFollowed = false;

					targetTail = s.ownTail;

					targetScript = s;

				}

			}

			if (targetScript != null && targetTail != ownTail) targetScript.isFollowed = true;
		//}
		//else {

			Vector3 dist = targetTail.transform.position - transform.position;
			float scalarDistance = dist.sqrMagnitude;





			if (scalarDistance < buttThresh && !isAttachedToButt) {

				isAttachedToButt = true;

				transform.position = targetTail.transform.position;// + attachOffset;
				transform.rotation = targetTail.transform.rotation;

				//rigidbody.MovePosition(targetTail.transform.position);
				//rigidbody.MoveRotation(targetTail.transform.rotation);	

				var c = targetTail.gameObject.AddComponent<CharacterJoint>();
				//var c = targetTail.gameObject.AddComponent<FixedJoint>();

				c.anchor = new Vector3(-0.7f,0,0);
				c.axis = new Vector3(0,0,1);
				
				c.connectedBody = ownHead;

				//Physics.IgnoreCollision(collider, targetTail.collider);


			}


			Debug.DrawRay(transform.position, dist, Color.red);


			Vector3 cross = Vector3.Cross(transform.right, dist);
			cross.Normalize();

			Debug.DrawRay(transform.position, cross, Color.blue);

			//if(!isAttachedToButt) {
				rigidbody.AddTorque(cross * torqueFactor, ForceMode.Acceleration);
				rigidbody.AddForce(transform.right * moveFactor);
			//}

		//}
	}



}
