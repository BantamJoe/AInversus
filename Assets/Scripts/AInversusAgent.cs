using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AInversusAgent : Agent {
  
  Player player;

  float episodeTime = 0;
  float timeReward = 5;
  
  void Start() {
    player = GetComponent<Player>();
  }

  public override void AgentReset() {
    player.Grid.ReInitialize();
    player.PlayerReset();
    player.opponent.PlayerReset();
    episodeTime = 0;
  }

  public override void CollectObservations() {

    AddVectorObs(new Vector2(player.opponent.location.x - player.location.x, player.opponent.location.y - player.location.y));
    AddVectorObs(GetOpponentBulletPosition());
    AddVectorObs(new float[] { CellToFloat(player.location + Vector2.up), CellToFloat(player.location + Vector2.right), CellToFloat(player.location + Vector2.down), CellToFloat(player.location + Vector2.left) });
  }

  Vector2 GetOpponentBulletPosition() {

    if (player.opponent.bullets.Count == 0)
      return player.opponent.location - player.location;

    return player.opponent.bullets[0].GetCurrentCell() - player.location;

  }

  float CellToFloat(Vector2 v) {

    switch(player.Grid.GetCell((int)player.location.x, (int)player.location.y - 1)) {
      case CellState.Black: return player.HomeState == CellState.Black ? 1 : -1;
      case CellState.White: return player.HomeState == CellState.White ? 1 : -1;
      default: return 0;
    } 
  }

  void CollectObservationArray() {

    float[] cells = player.Grid.GetCellsFloat(player.HomeState);
    /*AddVectorObs(player.Location.x / player.Grid.Cols);
    AddVectorObs(player.Location.y / player.Grid.Rows);
    AddVectorObs(player.Opponent.location.x / player.Grid.Cols);
    AddVectorObs(player.Opponent.location.y / player.Grid.Rows);
    AddVectorObs(cells);*/

    float[] playerPositions = new float[cells.Length];
    float[] bulletPositions = new float[cells.Length];

    float[] observations = new float[cells.Length];

    for (int i = 0; i < cells.Length; i++) {

      observations[i] = cells[i];
    }


    for (int i = 0; i < player.MaxBullets; i++) {
      if (player.bullets.Count > i && player.bullets[i] != null) {
        /* AddVectorObs(c.x / player.Grid.Cols); //Position
        AddVectorObs(c.y / player.Grid.Rows);
        AddVectorObs(player.bullets[i].direction.x); //Direction
        AddVectorObs(player.bullets[i].direction.y); */

        Vector2 c = player.bullets[i].GetCurrentCell();
        observations[(int)(c.x * player.Grid.Rows + c.y)] = 4;
      }
    }

    for (int i = 0; i < player.opponent.MaxBullets; i++) {
      if (player.opponent.bullets.Count > i && player.opponent.bullets[i] != null) {
        /* AddVectorObs(c.x / player.Grid.Cols); //Position
        AddVectorObs(c.y / player.Grid.Rows);
        AddVectorObs(player.opponent.bullets[i].direction.x); //Direction
        AddVectorObs(player.opponent.bullets[i].direction.y);
        */

        Vector2 c = player.opponent.bullets[i].GetCurrentCell();
        observations[(int)(c.x * player.Grid.Rows + c.y)] = 8;
      }
    }


    observations[(int)(player.Location.x * player.Grid.Rows + player.Location.y)] = 5;
    observations[(int)(player.opponent.Location.x * player.Grid.Rows + player.opponent.Location.y)] = 9;

    AddVectorObs(observations);

  }
  
  void WriteInFile(float[] observations) {
    StreamWriter writer = new StreamWriter("observations.txt", true);

    for(int i = 0; i < observations.Length; i++) {
      if (i != 0 && i % (player.Grid.Rows) == 0)
        writer.WriteLine();
      writer.Write(observations[i]);

    }
    writer.WriteLine();
    writer.Close();
  }

  public override void AgentAction(float[] vectorAction, string textAction) {

    //Rewards
    AddReward(-0.1f);
    episodeTime += 1;

    if(!player.Alive) {
      Done();
      AddReward(-3f);
    }

    if(!player.Opponent.Alive) {
      Done();
      AddReward(2 + 1/episodeTime * timeReward);
    }

    //Positioning
    float diffX = Mathf.Abs(player.location.y - player.opponent.location.x);
    float diffY = Mathf.Abs(player.location.y - player.opponent.location.x);

    if(diffX < 2 && diffY < 2) {
      //AddReward(0.1f);
    }

    //Rewards for stage control
    /*
     * float[] cells = player.Grid.GetCellsFloat(player.HomeState);
    for (int i = 0; i < cells.Length; i++)
      if (cells[i] == 1)
        AddReward(0.0001f);
        */

    bool actionSucceeded = true;


    //Actions
    switch((int)Mathf.Floor(vectorAction[0])) {
      case 0:
        actionSucceeded = player.MoveUp();
        break;
      case 1:
        actionSucceeded = player.MoveDown();
        break;
      case 2:
        actionSucceeded = player.MoveLeft();
        break;
      case 3:
        actionSucceeded = player.MoveRight();
        break;
      case 4:
        actionSucceeded = player.ShootUp();

        if (actionSucceeded && player.opponent.location.y < player.location.y) {
          AddReward(0.1f);
        }
        break;
      case 5:
        actionSucceeded = player.ShootDown();
        if (actionSucceeded && player.opponent.location.y > player.location.y) {
          AddReward(0.1f);
        }
        break;
      case 6:
        actionSucceeded = player.ShootLeft();
        if (actionSucceeded && player.opponent.location.x < player.location.x) {
          AddReward(0.1f);
        }
        break;
      case 7:
        actionSucceeded = player.ShootRight();
        if (actionSucceeded && player.opponent.location.x > player.location.x) {
          AddReward(0.1f);
        }
        break;
      default:
        AddReward(-0.1f);
        break;
    }

    if(!actionSucceeded) {
      AddReward(-0.3f);
    }
  }
}
