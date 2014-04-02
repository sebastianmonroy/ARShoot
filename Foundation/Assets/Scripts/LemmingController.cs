using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LemmingController : MonoBehaviour {
	public static int HIGHEST_LEVEL;
	public static float DECAY_AMOUNT;
	public static List<List<GameObject>> allBlocks = new List<List<GameObject>>();

	// Use this for initialization
	void Start () {
		HIGHEST_LEVEL = 0;
		DECAY_AMOUNT = 0.01f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void addBlock(GameObject blockObject) {
		block block = blockObject.GetComponent<block>();
		if (block.level >= allBlocks.Count) {
			for (int i = 0; i < block.level - allBlocks.Count; i++) {
				allBlocks.Add(new List<GameObject>());
			}
			allBlocks.Insert(block.level, new List<GameObject> {blockObject});
			HIGHEST_LEVEL = block.level;
		} else {
			allBlocks[block.level].Add(blockObject);
		}
		printList();
	}

	public static void removeBlock(GameObject blockObject) {
		block block = blockObject.GetComponent<block>();
		//print (allBlocks.Count + " >= " + block.level);
		if (allBlocks.Count > block.level) {
			allBlocks[block.level].Remove(blockObject);
			printList();
		}
	}

	public static Vector3 getTargetDirection(GameObject lemmingObject) {
		lemming lemming = lemmingObject.GetComponent<lemming>();
		Vector3 vectorSum = Vector3.zero;
		//float prioritySum = 0;
		if (allBlocks.Count > lemming.level) {
			foreach (GameObject blockObject in allBlocks[lemming.level]) {
				block block = blockObject.GetComponent<block>();
				vectorSum += block.priority * (blockObject.transform.position - lemmingObject.transform.position);
				//prioritySum += block.priority;
			}
		}
		Vector3 targetDirection = new Vector3(vectorSum.x, 0, vectorSum.z).normalized;
		return targetDirection;
	}

	private static void printList() {
		string str = "List = {";
		foreach (List<GameObject> list in allBlocks) {
			str += list.Count + " ";
		}
		str += "}";
		print(str);
	}
}