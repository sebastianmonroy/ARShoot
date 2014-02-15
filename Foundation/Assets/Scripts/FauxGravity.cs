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
		
		foreach (Collider hit in colliders){
			if(hit.tag == "Bullet"){
				Vector3 gravityUp = hit.transform.position - transform.position;
    			gravityUp.Normalize();

				hit.rigidbody.AddForce (gravityUp * gravity);
			}
		}
	}
}
