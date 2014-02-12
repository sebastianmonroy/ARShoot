using UnityEngine;
using System.Collections;

public class shoot : MonoBehaviour {
	public GameObject[] decalPrefabs;
	//public GameObject pinata;
	//public GameObject bullet;
	//public float speed;
	private float waitToShoot;
	public float waitDuration;

	// Use this for initialization
	void Start () {
		//pinata = GameObject.FindWithTag("pinata");
		waitToShoot = waitDuration;
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && waitToShoot <= 0) {

			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(transform.position, transform.forward, out hit)) {
				if (hit.collider.gameObject.tag == "Pinata") {
					int decalIndex = Random.Range(0,decalPrefabs.Length);
					GameObject decal = Instantiate(decalPrefabs[decalIndex], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
					decal.transform.localScale *= Random.Range(1.0f,3.0f);
					decal.transform.RotateAround(hit.normal, Random.Range(0.0f, 360.0f));
				}
			}

			/*GameObject blah = Instantiate(bullet, transform.position + Vector3.forward * 50, transform.rotation) as GameObject;
			blah.rigidbody.velocity = transform.forward * speed;*/

			waitToShoot = waitDuration;
		}

		waitToShoot -= Time.deltaTime;
	}
}
