using UnityEngine;
using System.Collections;

public class GameHandler : MonoBehaviour {
	public GameObject TetrisSpawn;
	private TetrisSpawnHandler TetrisSpawnHandler;
	public float spawnInterval;

	void Start () {
		TetrisSpawnHandler = TetrisSpawn.GetComponent<TetrisSpawnHandler>();
	}
	
	void Update () {
		if (TetrisSpawnHandler.SpawnWait >= spawnInterval) {
			TetrisSpawnHandler.SpawnT();
		}
	}
}
