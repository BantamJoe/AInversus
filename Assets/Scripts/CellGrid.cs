using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour {

  [SerializeField]
  Camera observingCamera;

  [SerializeField]
  int rows = 5, cols = 5;

  public int Cols {
    get { return cols; }
  }

  public int Rows {
    get { return rows; }
  }

  float padding = 0.5f;

  [SerializeField]
  GameObject cell;

  CellState[,] cells;
  Cell[,] cellObjects;

	// Use this for initialization
	void Awake () {
    Initialize();
	}

  void Initialize() {

    int whiteCount = Cols * Rows / 2;

    cells = new CellState[cols, rows];
    cellObjects = new Cell[cols, rows];

    Vector3 upLeft = new Vector3(cols / -2f, rows / 2f);

    for(int y = 0; y < rows; y++) {
      for(int x = 0; x < cols; x++) {
        GameObject newCell = Instantiate(cell, transform);
        newCell.transform.localPosition = upLeft + Vector3.right * x + Vector3.down * y;
        cellObjects[x,y] = newCell.GetComponent<Cell>();

        cells[x,y] = CellState.Black;
        cellObjects[x,y].initialState = cells[x,y];
      }
    }

    while(whiteCount > 0) {
      int x = Random.Range(0, Cols);
      int y = Random.Range(0, Rows);

      if(cells[x,y] == CellState.Black) {
        whiteCount--;
        cells[x, y] = CellState.White;
      }
    }

    if(cols > rows * Camera.main.aspect)
      observingCamera.orthographicSize = (cols / 2f + padding) / Camera.main.aspect;
    else
      observingCamera.orthographicSize = (rows / 2f + padding);
  }

  public void ReInitialize() {

    for (int y = 0; y < rows; y++) {
      for (int x = 0; x < cols; x++) {
        cells[x, y] = CellState.Black;
        cellObjects[x, y].SetStateInstant(cells[x, y]);
      }
    }

    int whiteCount = Cols * Rows / 2;
    while (whiteCount > 0) {
      int x = Random.Range(0, Cols);
      int y = Random.Range(0, Rows);

      if (cells[x, y] == CellState.Black) {
        whiteCount--;
        cells[x, y] = CellState.White;
        cellObjects[x, y].SetStateInstant(cells[x, y]);
      }
    }
  }

  public void SetCell(int x, int y, CellState state) {
    cells[x,y] = state;
    cellObjects[x, y].SetState(state, () => { });
  }
  
  public CellState GetCell(int x, int y) {

    if (x < 0 || x >= cols || y < 0 || y >= rows)
      return CellState.Invalid;
    return cells[x, y];
  }

  public CellState GetCell(Vector2 xy) {
    return GetCell((int)xy.x, (int)xy.y);
  }

  public CellState[,] GetCells() {
    return cells;
  }

  public float[] GetCellsFloat(CellState home) {

    float[] values = new float[Rows * Cols];

    for(int x = 0; x < Cols; x++) {
      for(int y = 0; y < Rows; y++) {
        values[x * Rows + y] = cells[x, y] == home ? 1 : 0;
      }
    }
    return values;
  }

  public Vector3 GetCellPosition(int x, int y) {
    return cellObjects[x, y].transform.position;
  }

  public Vector2 GetGridPosition(Vector3 position) {
    Vector3 upLeft = new Vector3(cols / -2f, rows / 2f);
    position -= upLeft;// + transform.position;

    position.x = Mathf.Floor(position.x);
    position.y = Mathf.Floor(-position.y);
    
    return position;
  }
	
}
