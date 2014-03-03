using UnityEngine;
using System.Collections;

public class cell : MonoBehaviour {
	public GameObject blockPrefab;
	private bool canSpawn;

	void Start() {
		print("block spawned");
		canSpawn = true;
	}

	void Update() {

	}

	void SpawnBlock() {
		if (canSpawn) {
			GameObject block = Instantiate(blockPrefab, this.transform.position, this.transform.rotation) as GameObject;
			block.transform.parent = GameObject.Find("ImageTarget").transform;
			block.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.x, this.transform.localScale.x) ;
			block.transform.position = this.transform.position + new Vector3(0, block.renderer.bounds.size.y/2, 0);
			//canSpawn = false;
		}
	}
}