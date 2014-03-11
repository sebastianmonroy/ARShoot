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

	public void SpawnT() {
		Instantiate(TblockPrefab, this.transform.position, this.transform.rotation);
		TCount++;
		AllCount++;
		SpawnWait = 0;
	}

	public void SpawnL() {
		Instantiate(LblockPrefab, this.transform.position, this.transform.rotation);
		LCount++;
		AllCount++;
		SpawnWait = 0;
	}

	public void SpawnZ() {
		Instantiate(ZblockPrefab, this.transform.position, this.transform.rotation);
		ZCount++;
		AllCount++;
		SpawnWait = 0;
	}

	public void SpawnO() {
		Instantiate(OblockPrefab, this.transform.position, this.transform.rotation);
		OCount++;
		AllCount++;
		SpawnWait = 0;
	}

	public void SpawnI() {
		Instantiate(IblockPrefab, this.transform.position, this.transform.rotation);
		ICount++;
		AllCount++;
		SpawnWait = 0;
	}
}
