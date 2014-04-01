using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LemmingController : MonoBehaviour {
	public static int HIGHEST_LEVEL;
	public static float DECAY_AMOUNT;
	public List<List<GameObject>> allBlocks = new List<List<GameObject>>();

	// Use this for initialization
	void Start () {
		HIGHEST_LEVEL = 0;
		DECAY_AMOUNT = 0.01f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addBlock(GameObject blockObject) {
		block block = blockObject.GetComponent<block>();
		if (block.level > allBlocks.Count) {
			allBlocks.Add(new List<GameObject> {blockObject});
			HIGHEST_LEVEL = block.level;
		} else {
			allBlocks[block.level].Insert(block.level, blockObject);
		}
	}

	public void getTargetDirection(GameObject lemmingObject) {
		
	}
}