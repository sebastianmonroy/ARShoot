using UnityEngine;
using System.Collections;

public class GameHandler : MonoBehaviour {
	public GameObject TetrisSpawn;
	private TetrisSpawnHandler TetrisSpawnHandler;
	private build BuildHandler;
	public float spawnInterval;

	void Start () {
		TetrisSpawnHandler = TetrisSpawn.GetComponent<TetrisSpawnHandler>();
		BuildHandler = Camera.main.GetComponent<build>();
	}
	
	void Update () {
		if (TetrisSpawnHandler.SpawnWait >= spawnInterval) {
			GameObject tetris = TetrisSpawnHandler.SpawnRandom();
			BuildHandler.selectedTetris = tetris;
		}
	}
}
