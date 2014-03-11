using UnityEngine;
using System.Collections;

public class TetrisSpawnHandler : MonoBehaviour {
	public GameObject TblockPrefab;
	public GameObject LblockPrefab;
	public GameObject ZblockPrefab;
	public GameObject OblockPrefab;
	public GameObject IblockPrefab;
	public static float SpawnWait;
	public static float TCount;
	public static float LCount;
	public static float ZCount;
	public static float OCount;
	public static float ICount;
	public static float AllCount;
	
	void Start () {
		SpawnWait = 0;
		TCount = 0;
		LCount = 0;
		ZCount = 0;
		OCount = 0;
		ICount = 0;
		AllCount = 0;
	}
	
	void Update () {
		SpawnWait += Time.deltaTime;
	}

	public GameObject SpawnRandom() {
		//Random rand = new Random();
		switch (Random.Range((int) 0, (int) 4)) {
			case 0:
				return SpawnT();
			case 1:
				return SpawnL();
			case 2:
				return SpawnZ();
			case 3:
				return SpawnO();
			case 4:
				return SpawnI();
			default:
				return null;
		}
	}

	public GameObject SpawnT() {
		GameObject tetris = Instantiate(TblockPrefab, this.transform.position, this.transform.rotation) as GameObject;
		TCount++;
		AllCount++;
		SpawnWait = 0;
		return tetris;
	}

	public GameObject SpawnL() {
		GameObject tetris = Instantiate(LblockPrefab, this.transform.position, this.transform.rotation) as GameObject;
		LCount++;
		AllCount++;
		SpawnWait = 0;
		return tetris;
	}

	public GameObject SpawnZ() {
		GameObject tetris = Instantiate(ZblockPrefab, this.transform.position, this.transform.rotation) as GameObject;
		ZCount++;
		AllCount++;
		SpawnWait = 0;
		return tetris;
	}

	public GameObject SpawnO() {
		GameObject tetris = Instantiate(OblockPrefab, this.transform.position, this.transform.rotation) as GameObject;
		OCount++;
		AllCount++;
		SpawnWait = 0;
		return tetris;
	}

	public GameObject SpawnI() {
		GameObject tetris = Instantiate(IblockPrefab, this.transform.position, this.transform.rotation) as GameObject;
		ICount++;
		AllCount++;
		SpawnWait = 0;
		return tetris;
	}
}
