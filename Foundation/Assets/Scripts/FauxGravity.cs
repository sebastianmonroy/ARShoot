using UnityEngine;
using System.Collections;

public class FauxGravity : MonoBehaviour {

	public float radius = 300.0f;
	public float gravity = -10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Applies an explosion force to all nearby rigidbodies
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);
		
		//gravitate bullets
		foreach (Collider hit in colliders){
			if(hit.tag == "Bullet"){
				Vector3 gravityUp = hit.transform.position - transform.position;
    			gravityUp.Normalize();

				hit.rigidbody.AddForce (gravityUp * gravity);
			}
		}


		colliders = Physics.OverlapSphere (explosionPos, 1000.0f);
		
		//gravitate bullets
		foreach (Collider hit in colliders){
			if(hit.tag == "Structure"){
				Vector3 gravityUp = hit.transform.position - transform.position;
    			gravityUp.Normalize();

				hit.rigidbody.AddForce (gravityUp * -300.0f);
			}
		}
	}
}
