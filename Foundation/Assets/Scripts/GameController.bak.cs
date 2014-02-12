using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public int modelCount = 3;//will search from model 1 to this number
	public GameObject model1, model2, model3;
	GameObject targetModel;

	// Use this for initialization
	void Start () {
		int randomIndex = Random.Range(0, modelCount);
		SwitchToModel(randomIndex);
		
		Debug.Log("Using model"+randomIndex);
	}

	void TurnOffMeshRenderer(){
		int cCount = targetModel.transform.childCount;
		if(cCount <= 0){
			targetModel.renderer.enabled = false;
			print(targetModel.renderer.isVisible);
		}else{
			foreach(Transform child in targetModel.transform){
				child.renderer.enabled = false;
			}
		}
	}
	//switch to a given model
	void SwitchToModel(int index){
		TurnOffModels();
		switch(index){
		case 0:
			targetModel = model1;
			break;
		case 1:
			targetModel = model2;
			break;
		case 2:
			targetModel = model3;
			break;
		}

		targetModel.SetActive(true);
		TurnOffMeshRenderer();
	}
	//turns off all models
	void TurnOffModels(){
		model1.SetActive(false);
		model2.SetActive(false);
		model3.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
