﻿using UnityEngine;
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
		SpawnWait = 5;
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
				return Spawn(0);
			case 1:
				return Spawn(1);
			case 2:
				return Spawn(2);
			case 3:
				return Spawn(3);
			case 4:
				return Spawn(4);
			default:
				return null;
		}
	}

	public GameObject Spawn(int tetrisPrefab) {
		GameObject newTetris = TblockPrefab;
		switch (tetrisPrefab) {
			case 0:
				newTetris = SpawnT();
				break;
			case 1:
				newTetris = SpawnL();
				break;
			case 2:
				newTetris = SpawnZ();
				break;
			case 3:
				newTetris = SpawnO();
				break;
			case 4:
				newTetris = SpawnI();
				break;
			default:
				break;
		}

		Transform bestTransform = newTetris.transform;
		bool initialized = false;
		foreach (Transform t in newTetris.transform) {
			if (t.gameObject.tag == "Block") {
				if (!initialized || t.gameObject.renderer.bounds.min.y < bestTransform.gameObject.renderer.bounds.min.y) {
					bestTransform = t;
					initialized = true;
				}
			}
		}

		newTetris.transform.position += new Vector3(0, (this.transform.position.y - bestTransform.gameObject.renderer.bounds.min.y), 0);

		return newTetris;
	}

	public GameObject SpawnT() {
		GameObject tetris = Instantiate(TblockPrefab, this.transform.position, this.transform.rotation) as GameObject;
		tetris.transform.parent = this.transform;
		TCount++;
		AllCount++;
		SpawnWait = 0;
		return tetris;
	}

	public GameObject SpawnL() {
		GameObject tetris = Instantiate(LblockPrefab, this.transform.position, this.transform.rotation) as GameObject;
		tetris.transform.parent = this.transform;
		LCount++;
		AllCount++;
		SpawnWait = 0;
		return tetris;
	}

	public GameObject SpawnZ() {
		GameObject tetris = Instantiate(ZblockPrefab, this.transform.position, this.transform.rotation) as GameObject;
		tetris.transform.parent = this.transform;
		ZCount++;
		AllCount++;
		SpawnWait = 0;
		return tetris;
	}

	public GameObject SpawnO() {
		GameObject tetris = Instantiate(OblockPrefab, this.transform.position, this.transform.rotation) as GameObject;
		tetris.transform.parent = this.transform;
		OCount++;
		AllCount++;
		SpawnWait = 0;
		return tetris;
	}

	public GameObject SpawnI() {
		GameObject tetris = Instantiate(IblockPrefab, this.transform.position, this.transform.rotation) as GameObject;
		tetris.transform.parent = this.transform;
		ICount++;
		AllCount++;
		SpawnWait = 0;
		return tetris;
	}
}
