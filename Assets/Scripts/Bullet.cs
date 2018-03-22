using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
  
  public CellState home;

  public Player player;
  public Player opponent;
  public Vector2 direction;

  private void Start() {
    lastCell = GetCurrentCell();
  }

  // Update is called once per frame
  void Update () {
    transform.position += transform.up * Time.deltaTime * 6;
    CheckPassedCells();
  }

  Vector2 lastCell;
  void CheckPassedCells() {

    Vector2 current = GetCurrentCell();

    Vector2 movement = current - lastCell;
    Vector2 toCheck = lastCell;

    do {
      toCheck += movement.normalized;
      CheckCell(toCheck, player.Grid.GetCell(toCheck));
    } while (toCheck != current);

    lastCell = current;
  }

  void CheckCell(Vector2 cell, CellState state) {

    if (opponent.Location == cell && opponent.Alive) {
      opponent.Die();
    }

    if (state == CellState.Invalid) {
      player.RemoveBullet(this);
      Destroy(gameObject);
    }
    else if (state != home) {
      player.Grid.SetCell((int)cell.x, (int)cell.y, home);
    }
  }

  public Vector2 GetCurrentCell() {
    return player.Grid.GetGridPosition(transform.position);
  }
}
