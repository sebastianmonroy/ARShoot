using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
	public GameObject decalPrefab;

	//public GameObject bulletPrefab;
	//public float paintRange;
	//public float paintCount;

	public Material MaterialA, MaterialB, MaterialC;
	Material targetMaterial;
	
	// Use this for initialization
	void Start () {
		Destroy(gameObject, 10.0f);
		int randomIndex = Random.Range(0, 3);
		switch(randomIndex){
		case 0:
			targetMaterial = MaterialA;
			break;
		case 1:
			targetMaterial = MaterialB;
			break;
		case 2:
			targetMaterial = MaterialC;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter(Collision collision) {
		print ("collision");
		if (collision.gameObject.tag == "Pinata") {
			ContactPoint contact = collision.contacts[0];
			
			GameObject decal = Instantiate(decalPrefab, contact.point, Quaternion.FromToRotation(Vector3.up, contact.normal)) as GameObject;
			decal.renderer.material = targetMaterial;
			decal.transform.RotateAround(transform.position, contact.normal, Random.Range (0.0f,360.0f));
			decal.transform.localScale *= Random.Range(2, 4);
			Destroy(this.collider);
			
			/*GameObject mySphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			mySphere.transform.localScale = Vector3.one * thing*5;
			mySphere.transform.position = contact.point + contact.normal*thing;*/
			
			//print(hit.Length);
			//foreach (RaycastHit h in hit) {
			//print (h.point);
			//if (h.transform.tag == "Pinata") {
			//GameObject decal = Instantiate(decalPrefab, h.point, Quaternion.FromToRotation(Vector3.up, h.normal)) as GameObject;
			//decal.transform.RotateAround(transform.position, h.normal, Random.Range (0.0f,360.0f));
			//}
			//}



			/*RaycastHit hit = new RaycastHit();
			
			int i = 0;
			while (i < paintCount) {
				print (i);
				Vector3 direction = this.transform.TransformDirection(Random.onUnitSphere);
				direction = new Vector3(direction.x, direction.y, Random.Range(0f, 0.5f));
				Debug.DrawRay(contact.point + contact.normal * 100, direction, Color.red);
				
				if (Physics.Raycast(contact.point + contact.normal * 100, direction, out hit, paintRange)) {
					Debug.DrawLine (contact.point + contact.normal * 100, hit.point);
					if (hit.transform.tag == "Pinata") {
						GameObject decal = Instantiate(decalPrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
						decal.transform.RotateAround(transform.position, hit.normal, Random.Range(0.0f,360.0f));
						decal.transform.localScale *= Random.Range(1.0f,4.0f);
						i++;
					}
				}
			}*/
			
			Destroy(this.gameObject);
		}
	}
}
