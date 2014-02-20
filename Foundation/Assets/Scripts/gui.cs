using UnityEngine;
using System.Collections;

public class gui : MonoBehaviour {
	public GameObject cubifyObject;
	public float sliderBoxSize = 1.0f;
	public float sliderCubeSize = 5.0f;
	public int sliderCubeSpawnRate = 5;
	public bool toggleInterior = false;
	private string mode = "Interior";
	public bool cubify = false;
	private string status = "Disabled";

	// Use this for initialization
	void Start () {
		cubifyObject = GameObject.FindWithTag("Pinata");
		cubifyObject.SendMessage("SetMode", toggleInterior);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		if (cubify) {
			status = "Enabled";
		} else {
			status = "Disabled";
		}

		if (toggleInterior) {
			mode = "Interior";
		} else {
			mode = "Exterior";
		}

		if (GUI.Button(new Rect(200,10,150,100), mode)) {
			toggleInterior = !toggleInterior;
			cubifyObject.SendMessage("SetMode", toggleInterior);
		}

		sliderBoxSize = GUI.HorizontalSlider(new Rect (10, 150, 600, 50), sliderBoxSize, 0.0f, 5.0f);
		sliderCubeSize = GUI.HorizontalSlider(new Rect (10, 200, 600, 50), sliderCubeSize, 1.0f, 15.0f);
		sliderCubeSpawnRate = (int) GUI.HorizontalSlider(new Rect (10, 250, 600, 50), sliderCubeSpawnRate, 1, 50);
	
		GUI.Label (new Rect (630, 150, 30, 30), "" + ((float) ((int) (sliderBoxSize*100)))/100);
		GUI.Label (new Rect (630, 200, 30, 30), "" + ((float) ((int) (sliderCubeSize*100)))/100);
		GUI.Label (new Rect (630, 250, 30, 30), "" + sliderCubeSpawnRate);

		if (GUI.Button(new Rect(10,10,150,100), status)) {
			cubifyObject.SendMessage("ChangeParameters", new float[]{sliderBoxSize, sliderCubeSize, sliderCubeSpawnRate});
			cubifyObject.SendMessage("Toggle");
			cubify = !cubify;
		}
	}
}
