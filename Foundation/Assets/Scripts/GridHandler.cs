using UnityEngine;
using System.Collections;

public class GridHandler : MonoBehaviour {
	private Vector3 targetCenter;
	private Vector3 targetExtents;
	private Vector3 targetBoundMin;
	private Vector3 targetBoundMax;
	public Vector3 cellSize;
	public GameObject cellPrefab;
	private float currentCellScale;
	private float originalCellScale = 1;

	// Use this for initialization
	void Start () {
		currentCellScale = originalCellScale;
		targetCenter = this.transform.parent.gameObject.renderer.bounds.center;
		print(targetCenter);
		targetExtents = this.transform.parent.gameObject.renderer.bounds.extents;
		targetBoundMin = targetCenter - targetExtents;
		targetBoundMax = targetCenter + targetExtents;
		print(targetBoundMin);
		print(targetBoundMax);
		SpawnGrid(originalCellScale);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnGrid(float scaleFactor) {
		GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");
		foreach (GameObject c in cells) {
			Destroy(c);
		}

		currentCellScale = Mathf.Clamp(currentCellScale * scaleFactor, originalCellScale * 0.5f, originalCellScale * 2.0f);
		cellSize = cellPrefab.renderer.bounds.size * currentCellScale;

		for (float i = targetBoundMin.x + cellSize.x/2; i <= targetBoundMax.x - cellSize.x/2; i += cellSize.x) {
			for (float k = targetBoundMin.z + cellSize.z/2; k <= targetBoundMax.z - cellSize.z/2; k += cellSize.z) {
				Vector3 cellPosition = this.transform.position + new Vector3(i, 0, k);
				GameObject cell = Instantiate(cellPrefab, cellPosition, this.transform.rotation) as GameObject;
				cell.transform.localScale = new Vector3(cell.transform.localScale.x * currentCellScale, cell.transform.localScale.y, cell.transform.localScale.z * currentCellScale);
				cell.transform.parent = this.transform;
				
				print(cellPosition);
				//break;
			}
			//break;
		}
	}

	public void SpawnGrid() {
		this.SpawnGrid(currentCellScale);
	}
}
