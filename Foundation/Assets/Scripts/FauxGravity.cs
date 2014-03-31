using UnityEngine;
using System.Collections;

public class FauxGravity : MonoBehaviour {
	public bool affectBullets;
	public bool affectStructures;
	public float radius = 300.0f;
	public float gravity = -10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//gravity on bullets
		if(affectBullets){
			// Applies an explosion force to all nearby rigidbodies
			Vector3 explosionPos = transform.position;
			Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);
			//gravitate bullets
			foreach (Collider hit in colliders){
				if(hit.tag == "Bullet" && hit.GetComponent("Rigidbody")!=null){
					Vector3 gravityUp = hit.transform.position - transform.position;
	    			gravityUp.Normalize();

					hit.rigidbody.AddForce (gravityUp * gravity);
				}
			}
		}

		//gravity on structures
		if(affectStructures){
			Vector3 explosionPosStruct = transform.position;
			Collider[] structColliders = Physics.OverlapSphere (explosionPosStruct, 1000.0f);
			
			//gravitate bullets
			foreach (Collider hit in structColliders){
				if(hit.tag == "Structure" && hit.GetComponent("Rigidbody")!=null){
					Vector3 gravityUp = hit.transform.position - transform.position;
	    			gravityUp.Normalize();

					hit.rigidbody.AddForce (gravityUp * -300.0f);
				}
			}	
		}
	}
}
