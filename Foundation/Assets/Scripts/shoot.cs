using UnityEngine;
using System.Collections;

public class shoot : MonoBehaviour {
	public GameObject[] decalPrefabs;
	public bool useBullets;//if true use bullets, if false paint on touch
	//public GameObject pinata;
<<<<<<< HEAD
	public GameObject bullet;
	public float speed;
=======
	public GameObject bulletPrefab;
	public float bulletSpeed;
	//public float speed;
>>>>>>> fa214ac9b6eae67ea9a0b595f856905a96fc3888
	private float waitToShoot;
	public float waitDuration;
	private bool leftClickDown;
	private bool touching;
	private int decalIndex;

	// Use this for initialization
	void Start () {
		//pinata = GameObject.FindWithTag("pinata");
		decalIndex = Random.Range(0,decalPrefabs.Length);
		waitToShoot = waitDuration;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && !leftClickDown) {
			decalIndex = Random.Range(0,decalPrefabs.Length);
			leftClickDown = true;
		}

		if (Input.GetMouseButtonUp(0) && leftClickDown) {
			leftClickDown = false;
		}

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !leftClickDown) {
			decalIndex = Random.Range(0,decalPrefabs.Length);
			touching = true;
		}

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && leftClickDown) {
			touching = false;
		}

		if (waitToShoot <= 0) {
			RaycastHit hit = new RaycastHit();
			if (Input.touchCount > 0) {
				// Handle tablet touch shooting
				Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
				if (Physics.Raycast(ray, out hit)) {
					if (hit.collider.gameObject.tag == "Pinata") {
						GameObject decal = Instantiate(decalPrefabs[decalIndex], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
						decal.transform.localScale *= Random.Range(1.0f,3.0f);
						decal.transform.RotateAround(hit.normal, Random.Range(0.0f, 360.0f));
					}
				}
			} else if (leftClickDown) {
				// Handle mouse left click shooting
				if(useBullets) {
					// use bullets
					GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
					bullet.rigidbody.velocity = transform.forward * bulletSpeed;
				} else {
					// use raycasts
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
					if (Physics.Raycast(ray, out hit)) {
						if (hit.collider.gameObject.tag == "Pinata") {
							GameObject decal = Instantiate(decalPrefabs[decalIndex], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
							decal.transform.localScale *= Random.Range(1.0f,3.0f);
							decal.transform.RotateAround(hit.normal, Random.Range(0.0f, 360.0f));
						}
					}
				}
			}

			waitToShoot = waitDuration;
		}

		/*if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && waitToShoot <= 0) {

			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(transform.position, transform.forward, out hit)) {
				if (hit.collider.gameObject.tag == "Pinata") {
					int decalIndex = Random.Range(0,decalPrefabs.Length);
					GameObject decal = Instantiate(decalPrefabs[decalIndex], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
					decal.transform.localScale *= Random.Range(1.0f,3.0f);
					decal.transform.RotateAround(hit.normal, Random.Range(0.0f, 360.0f));
				}
			}

			waitToShoot = waitDuration;
		}*/
				
		waitToShoot = waitToShoot - 1;
	}
}
