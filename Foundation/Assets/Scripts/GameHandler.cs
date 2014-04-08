using UnityEngine;
using System.Collections;

public class GameHandler : MonoBehaviour {
	public GameObject blockPrefab;
	public GameObject playerPrefab;
	public static float BLOCK_SIZE = 15;
	private GameObject FloorObject;
	public static Vector3 FLOOR_MIN;
	public static Vector3 FLOOR_MAX;
	public static float PRIORITY_DECAY_PERIOD = 2.0f;
	public static float PRIORITY_DECAY_AMOUNT = 0.01f;
	private int NUM_PLAYERS = 0;

	void Start () {
		FloorObject = GameObject.FindWithTag("Floor");

		setFloorTiling();

		FLOOR_MIN = FloorObject.renderer.bounds.min;
		FLOOR_MAX = FloorObject.renderer.bounds.max;
	}

	void Update () {

	}

	public void NewPlayer() {
		NUM_PLAYERS++;
		GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		newPlayer.name = "Player " + NUM_PLAYERS;
		newPlayer.GetComponent<PlayerHandler>().PLAYER_NUM = NUM_PLAYERS;
		spawnOriginBlock(NUM_PLAYERS);
	}

	private void setFloorTiling() {
		float tilingScale = (FloorObject.transform.parent.localScale.x * FloorObject.transform.localScale.x) / BLOCK_SIZE;
		
		FloorObject.renderer.material.mainTextureScale = new Vector2(tilingScale, tilingScale);
	}

	private void spawnOriginBlock(int playerNum) {
		Vector3 corner = new Vector3(FloorObject.renderer.bounds.min.x + BLOCK_SIZE/2, FloorObject.transform.position.y + FloorObject.renderer.bounds.max.y + BLOCK_SIZE/2, FloorObject.renderer.bounds.min.z + BLOCK_SIZE/2);
		GameObject originBlock = Instantiate(blockPrefab, corner, this.transform.rotation) as GameObject;
		originBlock.transform.localScale = Vector3.one * BLOCK_SIZE;
		originBlock.transform.parent = FloorObject.transform.parent;
		originBlock.GetComponent<block>().playerNum = playerNum;

		GameObject.Find("Player " + playerNum).GetComponent<PlayerHandler>().LemmingController.addBlock(originBlock);
	}

	public static bool isInBounds(Vector3 test) {
		return (test.x > FLOOR_MIN.x && test.x < FLOOR_MAX.x && test.z > FLOOR_MIN.z && test.z < FLOOR_MAX.z && test.y > FLOOR_MIN.y);
	}

	public static bool isInBounds(GameObject test) {
		return (test.transform.position.x > FLOOR_MIN.x && test.transform.position.x < FLOOR_MAX.x && test.transform.position.z > FLOOR_MIN.z && test.transform.position.z < FLOOR_MAX.z && test.transform.position.y > FLOOR_MIN.y);
	}
}
