using UnityEngine;
using System.Collections;

public class block : MonoBehaviour {
	public float delayDuration;
	private float delayCount;
	private bool removeable;

	// Use this for initialization
	void Start () {
		delayCount = 0;
		removeable = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!removeable) {
			delayCount += Time.deltaTime;
			if (delayCount >= delayDuration) {
				removeable = true;
			}
		}
	}

	public void Remove() {
		if (removeable) {
			Destroy(this.gameObject);
		}
	}
}
