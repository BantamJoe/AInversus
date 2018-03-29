using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

  [SerializeField]
  CellGrid grid;

  [HideInInspector]
  public List<Bullet> bullets;

  int maxBullets = 1;

  public Vector2 location;
  Vector2 initLocation;

  [SerializeField]
  CellState homeState;

  bool alive = true;

  public Player opponent;
  public GameObject bullet;


  public Vector2 Location {
    get { return location; }
  }

  public CellGrid Grid {
    get { return grid; }
  }

  public Player Opponent {
    get { return opponent; }
  }

  public bool Alive {
    get { return alive; }
  }

  public int MaxBullets {
    get { return maxBullets; }
  }

  public CellState HomeState {
    get { return homeState; }
  }

  // Use this for initialization
  void Start() {
    bullets = new List<Bullet>();
    PlayerReset();
  }

  public void PlayerReset() {

    for (int i = 0; i < bullets.Count; i++) {
      Destroy(bullets[i].gameObject);
    }
    bullets.Clear();
    transform.GetChild(0).gameObject.SetActive(true);

    int x = 0;
    int y = 0;
    do {

      x = Random.Range(0, Grid.Cols);
      y = Random.Range(0, Grid.Rows);

    } while (grid.GetCell(x, y) != homeState);

    SetLocation(new Vector2(x, y));
    alive = true;
  }

  void SetLocation(Vector2 newLocation) {
    location = newLocation;

    transform.position = grid.GetCellPosition((int)location.x, (int)location.y);
  }

  public bool MoveUp(InputManager.InputType type = InputManager.InputType.Down) {
    return Move(Vector2.down, type);
  }

  public bool MoveDown(InputManager.InputType type = InputManager.InputType.Down) {
    return Move(Vector2.up, type);
  }

  public bool MoveLeft(InputManager.InputType type = InputManager.InputType.Down) {
    return Move(Vector2.left, type);
  }

  public bool MoveRight(InputManager.InputType type = InputManager.InputType.Down) {
    return Move(Vector2.right, type);
  }

  bool Move(Vector2 direction, InputManager.InputType type) {
    if (grid.GetCell((int)(location.x + direction.x), (int)(location.y + direction.y)) == homeState) {
      location += direction;
      SetLocation(location);
      return true;
    }
    return false;
  }

  public void rMoveUp() { MoveUp(); }
  public void rMoveDown() { MoveDown(); }
  public void rMoveLeft() { MoveLeft(); }
  public void rMoveRight() { MoveRight(); }

  public void rShootUp() { ShootUp(); }
  public void rShootDown() { ShootDown(); }
  public void rShootLeft() { ShootLeft(); }
  public void rShootRight() { ShootRight(); }

  public bool ShootUp(InputManager.InputType type = InputManager.InputType.Down) {
    return Shoot(Vector3.up, type);
  }

  public bool ShootDown(InputManager.InputType type = InputManager.InputType.Down) {
    return Shoot(Vector3.down, type);
  }

  public bool ShootLeft(InputManager.InputType type = InputManager.InputType.Down) {
    return Shoot(Vector3.left, type);
  }

  public bool ShootRight(InputManager.InputType type = InputManager.InputType.Down) {
    return Shoot(Vector3.right, type);
  }
  

  public bool Shoot(Vector3 direction, InputManager.InputType type) {
    
    if(bullets.Count >= maxBullets) {
      return false;
    }
    
    Quaternion rotation = Quaternion.identity;

    if (direction == Vector3.up)
      rotation = Quaternion.identity;
    if (direction == Vector3.down)
      rotation = Quaternion.Euler(0, 0, 180);
    if (direction == Vector3.right)
      rotation = Quaternion.Euler(0, 0, -90);
    if (direction == Vector3.left)
      rotation = Quaternion.Euler(0, 0, 90);

    GameObject obj = Instantiate(bullet, transform.position, rotation);
    obj.GetComponent<Bullet>().home = this.homeState;
    obj.GetComponent<Bullet>().player = this;
    obj.GetComponent<Bullet>().opponent = this.opponent;
    obj.GetComponent<Bullet>().direction = direction;
    bullets.Add(obj.GetComponent<Bullet>());

    return true;
  }

  public void Die() {
    alive = false;
  }

  public void RemoveBullet(Bullet bullet) {
    bullets.Remove(bullet);
  }
}
