using UnityEngine;
using System.Collections;

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
	public bool reset;

	void Start () {
		modelCenter = this.renderer.bounds.center;
		modelExtents = this.renderer.bounds.extents;
		modelBoundMin = modelCenter - modelExtents;
		modelBoundMax = modelCenter + modelExtents;
		
		reset = true;
	}

	void Update () {
		if (reset) {
			// update box and cube parameters
			boxBoundMin = modelCenter - modelExtents * boxScale;
			boxBoundMax = modelCenter + modelExtents * boxScale;
			cubeSize = cubePrefab.renderer.bounds.size * cubeScale;

			print("reset cubes");

			// destroy old cubes before spawning new ones
			GameObject[] oldCubes = GameObject.FindGameObjectsWithTag("Cube");
			for (int i = 0; i < oldCubes.Length; i++) {
				Destroy(oldCubes[i]);
			}

			// spawn new cubes around bounds of object
			for (float i = boxBoundMin.x; i <= boxBoundMax.x; i += cubeSize.x) {
				for (float j = boxBoundMin.y; j <= boxBoundMax.y; j += cubeSize.y) {
					for (float k = boxBoundMin.z; k <= boxBoundMax.z; k += cubeSize.z) {
						//print(new Vector3(i, j, k));
						bool canSpawnCube = false;
						
						Collider[] col = Physics.OverlapSphere(new Vector3(i, j, k) , cubeSize.x/2);
						foreach (Collider c in col) {
							if (c.gameObject == this.gameObject) {
								canSpawnCube = true;
								break;
							}
						}

						if (canSpawnCube) {
							GameObject cube = Instantiate(cubePrefab, new Vector3(i, j, k), this.transform.rotation) as GameObject;
							cube.transform.localScale *= cubeScale;
						}
					}
				}
			}

			// flip flag to prevent repetition
			reset = false;
		}
	}
}
