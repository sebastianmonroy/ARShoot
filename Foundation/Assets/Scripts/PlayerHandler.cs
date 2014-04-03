using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour {
	public int PLAYER_NUM;
	public build BuildController;
	public LemmingController LemmingController;

	// Use this for initialization
	void Start () {
		BuildController = this.GetComponent<build>();
		LemmingController = this.GetComponent<LemmingController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
