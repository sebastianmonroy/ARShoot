using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cubify : MonoBehaviour {
	private Vector3 modelCenter;
	private Vector3 modelExtents;
	private Vector3 modelBoundMin;
	private Vector3 modelBoundMax;
	private Vector3 boxBoundMin;
	private Vector3 boxBoundMax;
	public float boxScale;
	public float cubeScale;
	public GameObject cubePrefab;
	private Vector3 cubeSize;
	public bool interior;
	public bool enabled;
	private bool done;
	private List<Vector3> cubePositions = new List<Vector3>();
	public int cubesPerFrame;
	private bool dataGathered;
	private GameState STATE = GameState.OFF;

	public enum GameState {
		GATHER_DATA, SPAWN_CUBES, CLEAR, OFF, ON
	}

	void Start () {
		modelCenter = this.renderer.bounds.center;
		modelExtents = this.renderer.bounds.extents;
		modelBoundMin = modelCenter - modelExtents;
		modelBoundMax = modelCenter + modelExtents;

		// only spawn cubes when enabled is made true by user
		enabled = false;
		done = false;
	}

	void Update () {
		if (STATE == GameState.GATHER_DATA) {
			Reset();
			getCubeData();
			STATE = GameState.SPAWN_CUBES;
		} else if (STATE == GameState.SPAWN_CUBES) {
			Spawn (cubesPerFrame);
			if (cubePositions.Count <= 0) {
				STATE = GameState.ON;
			}
		} else if (STATE == GameState.CLEAR) {
			Reset();
			STATE = GameState.OFF;
		}
	}

	public void getCubeData() {
		Reset();
		
		// update box and cube parameters
		boxBoundMin = modelCenter - modelExtents * boxScale;
		boxBoundMax = modelCenter + modelExtents * boxScale;
		cubeSize = cubePrefab.renderer.bounds.size * cubeScale;
		
		print("enable cubes");
		
		// spawn new cubes around bounds of object
		for (float i = boxBoundMin.x; i <= boxBoundMax.x; i += cubeSize.x) {
			for (float j = boxBoundMin.y; j <= boxBoundMax.y; j += cubeSize.y) {
				for (float k = boxBoundMin.z; k <= boxBoundMax.z; k += cubeSize.z) {
					//print(new Vector3(i, j, k));
					bool canSpawnCube = !interior;
					
					Collider[] col = Physics.OverlapSphere(new Vector3(i, j, k) , cubeSize.x/2);
					foreach (Collider c in col) {
						if (c.gameObject == this.gameObject) {
							canSpawnCube = !canSpawnCube;
							break;
						}
					}
					
					if (canSpawnCube) {
						cubePositions.Add(new Vector3(i,j,k));
					}
				}
			}
		}
	}

	public void Reset() {
		GameObject[] oldCubes = GameObject.FindGameObjectsWithTag("Cube");
		if (oldCubes.Length > 0) {
			print("clear cubes");
			for (int i = 0; i < oldCubes.Length; i++) {
				Destroy(oldCubes[i]);
			}
		}

		if (cubePositions.Count > 0) {
			cubePositions.Clear();
		}
	}

	public void Toggle() {
		if (STATE == GameState.OFF) {
			STATE = GameState.GATHER_DATA;
		} else {
			STATE = GameState.CLEAR;
		}
	}

	public void SetMode(bool interior) {
		this.interior = interior;
		STATE = GameState.GATHER_DATA;
	}

	public void ChangeParameters(float[] scales) {
		this.boxScale = scales[0];
		this.cubeScale = scales[1];
		this.cubesPerFrame = (int) (scales[2]);
	}

	public bool GetStatus() {
		if (STATE == GameState.OFF || STATE == GameState.CLEAR) {
			return false;
		} else {
			return true;
		}
	}

	public void Spawn(int numCubes) {
		if (cubePositions.Count < numCubes) {
			numCubes = cubePositions.Count;
			done = true;
		}
		for (int i = 0; i < numCubes; i++) {
			GameObject cube = Instantiate(cubePrefab, cubePositions[i], this.transform.rotation) as GameObject;
			cube.transform.localScale *= cubeScale;
		}

		cubePositions.RemoveRange(0, numCubes);
	}
}
