using UnityEngine;
using System.Collections;

public class BuildGUI : MonoBehaviour {
	public GameObject mainGameObject;
	private build bScript; 
	
	void Start(){
		bScript = mainGameObject.transform.GetComponent<build>();
	}
		

	void OnGUI () {
		GUI.Box(new Rect(10,10,100,90), "Build GUI");
		
		if(GUI.Button(new Rect(20,40,80,20), "Clear")) {
			//bScript.reset();
		}
		if(GUI.Button(new Rect(20,70,80,20), "Simulate")) {
			//bScript.simulate();
		}
	}
}