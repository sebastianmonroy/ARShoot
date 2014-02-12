using UnityEngine;
using System.Collections;

public class spawn : MonoBehaviour {
	public GameObject[] modelPrefabs;
	public int numModels;
	public bool onlyModel;
	private GameObject model;
	private float modelScale;

	void Start () {
		// Prevent invalid number of models to choose from
		if (numModels <= 0) {
			numModels = modelPrefabs.Length-1;
		}

		// Prevent invalid model scale
		if (modelScale <= 0) {
			modelScale = 1;
		}

		// If onlyModel is enabled, do not randomly select model
		int modelIndex;
		if (onlyModel) {
			modelIndex = numModels;
		} else {
			modelIndex = Random.Range(0,numModels+1);
		}

		// Instantiate model based on above options
		model = Instantiate(modelPrefabs[modelIndex], this.transform.position, Random.rotation) as GameObject;
		//model.transform.position = -1 * model.collider.bounds.center;
		model.transform.parent = this.transform;
		// Fixes size of the model based on Spawn size
		model.transform.localScale *= modelScale;
		model.renderer.enabled = true;
	}

	void Update () {
		// Prevent model from ever being rendered
		if (model != null && model.renderer.enabled) {
			//model.renderer.enabled = false;
		}
	}
}
