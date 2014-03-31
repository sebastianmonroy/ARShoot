﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class block : MonoBehaviour {
	public GameObject BlockPreviewPrefab;
	public bool colliding = false;
	public bool outOfBounds = false;
	//public List<GameObject> collisionObjects;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (this.transform.parent.gameObject.tag == "Tetris") {
			if (this.transform.parent.GetComponent<TetriminoHandler>().isPreview) {
				checkCollisions();
				checkBounds();
			}
		}
	}
	
	public bool hasBlockAbove(){
		RaycastHit hit;
		Ray r = new Ray(transform.position, Vector3.up);
		Physics.Raycast(r, out hit, GameHandler.BLOCK_SIZE);
		if(hit.transform.gameObject.tag == "Block"){
			return true;
		}
		return false;
	}

	public void previewSides() {
		destroyPreview();

		Vector3[] directions = {Vector3.left, Vector3.right, Vector3.forward, -Vector3.forward, -Vector3.up, Vector3.up};
		List<Vector3> potentials = new List<Vector3>();

		RaycastHit hit;
		foreach (Vector3 v in directions) {
			if (!Physics.Raycast(this.transform.position, v, out hit, this.renderer.bounds.size.x, 1 << 9)) {
				potentials.Add(v);
			}
		}

		foreach (Vector3 p in potentials) {
			Vector3 potentialPosition = this.transform.position + p * this.renderer.bounds.size.x;
			if (GameHandler.isInBounds(potentialPosition)) {
				GameObject previewBlock = Instantiate(BlockPreviewPrefab, potentialPosition, this.transform.rotation) as GameObject;
				previewBlock.transform.localScale = Vector3.one * GameHandler.BLOCK_SIZE/2;
				//previewBlock.transform.parent = this.transform;
				previewBlock.GetComponent<block_preview>().direction = p;
				previewBlock.transform.parent = this.transform;
			}
		}
	}

	public void destroyPreview() {
		foreach (Transform t in this.transform) {
			if (t.gameObject.tag == "Preview") {
				Destroy(t.gameObject);
			}
		}
	}

	public void checkBounds() {
		if (!outOfBounds && !GameHandler.isInBounds(this.transform.position)) {
			outOfBounds = true;
		} else if (outOfBounds && GameHandler.isInBounds(this.transform.position)) {
			outOfBounds = false;
		}
	}

	public void checkCollisions() {
		if (!colliding && isColliding()) {
			colliding = true;
		} else if (colliding && !isColliding()) {
			colliding = false;
		}
	}

	public bool isColliding() {
		GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

		foreach (GameObject b in blocks) {
			if (b != this.gameObject) {
				if (this.collider.bounds.Intersects(b.collider.bounds)) {
					print("collision with " + b.gameObject.name);
					return true;
				}
			}
		}

		return false;
	}

	void OnCollisionEnter(Collision collision) {
		print("collision with " + collision.gameObject.tag);
		if (collision.gameObject.tag == "Block") {
			//collisionObjects.Add(collision.gameObject);
			colliding = true;
		}
	}

	void OnCollisionStay(Collision collision) {
		print("collision with " + collision.gameObject.tag);
		if (!colliding && collision.gameObject.tag == "Block") {
			colliding = true;
		}
	}

	void OnCollisionExit(Collision collision) {
		if (collision.gameObject.tag == "Block") {
			//collisionObjects.Remove(collision.gameObject);
			colliding = false;
		}
	}
}
