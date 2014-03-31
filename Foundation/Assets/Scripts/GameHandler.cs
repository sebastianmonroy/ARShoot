using UnityEngine;
using System.Collections;

public class GameHandler : MonoBehaviour {
	private build BuildHandler;
	public GameObject blockPrefab;
	public static float BLOCK_SIZE = 15;
	public GameObject FloorObject;
	public static int PLAYER_NUM;
	public static Vector3 FLOOR_MIN;
	public static Vector3 FLOOR_MAX;


	void Start () {
		PLAYER_NUM = 0;
		BuildHandler = Camera.main.GetComponent<build>();
		FloorObject = GameObject.FindWithTag("Floor");

		float tilingScale = (FloorObject.transform.parent.localScale.x * FloorObject.transform.localScale.x) / BLOCK_SIZE;
		
		FloorObject.renderer.material.mainTextureScale = new Vector2(tilingScale, tilingScale);
		Vector3 corner = new Vector3(FloorObject.renderer.bounds.min.x + BLOCK_SIZE/2, FloorObject.transform.position.y + FloorObject.renderer.bounds.max.y + BLOCK_SIZE/2, FloorObject.renderer.bounds.min.z + BLOCK_SIZE/2);
		
		GameObject originBlock = Instantiate(blockPrefab, corner, this.transform.rotation) as GameObject;
		originBlock.transform.localScale = Vector3.one * BLOCK_SIZE;
		originBlock.transform.parent = FloorObject.transform.parent;

		FLOOR_MIN = FloorObject.renderer.bounds.min;
		FLOOR_MAX = FloorObject.renderer.bounds.max;
	}

	void Update () {

	}

	public static bool isInBounds(Vector3 test) {
		return (test.x > FLOOR_MIN.x && test.x < FLOOR_MAX.x && test.z > FLOOR_MIN.z && test.z < FLOOR_MAX.z && test.y > FLOOR_MIN.y);
	}

	public static bool isInBounds(GameObject test) {
		return (test.transform.position.x > FLOOR_MIN.x && test.transform.position.x < FLOOR_MAX.x && test.transform.position.z > FLOOR_MIN.z && test.transform.position.z < FLOOR_MAX.z && test.transform.position.y > FLOOR_MIN.y);
	}
}
