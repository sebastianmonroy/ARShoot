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
			allBlocks.Insert(block.level, new List<GameObject> {blockObject});
			HIGHEST_LEVEL = block.level;
		} else {
			allBlocks[block.level].Insert(block.level, blockObject);
		}
	}

	public void removeBlock(GameObject blockObject) {
		block block = blockObject.GetComponent<block>();
		allBlocks[block.level].Remove(blockObject);
	}

	public Vector3 getTargetDirection(GameObject lemmingObject) {
		lemming lemming = lemmingObject.GetComponent<lemming>();
		Vector3 vectorSum = Vector3.zero;
		//float prioritySum = 0;
		foreach (GameObject blockObject in allBlocks[lemming.level]) {
			block block = blockObject.GetComponent<block>();
			vectorSum += block.priority * (blockObject.transform.position - lemmingObject.transform.position);
			//prioritySum += block.priority;
		}
		Vector3 targetDirection = new Vector3(vectorSum.x, 0, vectorSum.z).normalized;
		return targetDirection;
	}