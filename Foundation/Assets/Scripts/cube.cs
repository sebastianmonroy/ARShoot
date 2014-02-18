using UnityEngine;
using System.Collections;

public class cube : MonoBehaviour {
	private bool deleteUponCubify;
	public float waitDuration;

	// Use this for initialization
	void Start () {
		deleteUponCubify = true;
	}
	
	// Update is called once per frame
	void Update () {
		waitDuration -= Time.deltaTime;
		if (waitDuration < 0) {
			//deleteUponCubify = false;
		}
	}

	void OnCollisionEnter(Collision collision) {
		print("cube collision");
		if (deleteUponCubify && collision.gameObject.tag == "Pinata") {
			print("cube destroy");
			Destroy(this.gameObject);
		}
	}
}
