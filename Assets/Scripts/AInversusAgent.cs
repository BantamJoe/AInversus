using System.Collections;
using System.Collections.Generic;
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

    
    float[] cells = player.Grid.GetCellsFloat(player.HomeState);
    AddVectorObs(player.Location.x / player.Grid.Cols);
    AddVectorObs(player.Location.y / player.Grid.Rows);
    AddVectorObs(player.Opponent.location.x / player.Grid.Cols);
    AddVectorObs(player.Opponent.location.y / player.Grid.Rows);
    AddVectorObs(cells);
    


    for (int i = 0; i < player.MaxBullets; i++) {
      if (player.bullets.Count <= i || player.bullets[i] == null) {
        AddVectorObs(player.Location.x / player.Grid.Cols); //Position
        AddVectorObs(player.Location.y / player.Grid.Rows);
        AddVectorObs(0); //Direction
        AddVectorObs(0);
      }
      else {
        Vector2 c = player.bullets[i].GetCurrentCell();
        AddVectorObs(c.x / player.Grid.Cols); //Position
        AddVectorObs(c.y / player.Grid.Rows);
        AddVectorObs(player.bullets[i].direction.x); //Direction
        AddVectorObs(player.bullets[i].direction.y);
      }
    }

    for (int i = 0; i < player.opponent.MaxBullets; i++) {
      if (player.opponent.bullets.Count <= i || player.opponent.bullets[i] == null) {
        AddVectorObs(player.opponent.Location.x / player.Grid.Cols); //Position
        AddVectorObs(player.opponent.Location.y / player.Grid.Rows);
        AddVectorObs(0); //Direction
        AddVectorObs(0);
      }
      else {
        Vector2 c = player.opponent.bullets[i].GetCurrentCell();
        AddVectorObs(c.x / player.Grid.Cols); //Position
        AddVectorObs(c.y / player.Grid.Rows);
        AddVectorObs(player.opponent.bullets[i].direction.x); //Direction
        AddVectorObs(player.opponent.bullets[i].direction.y);
      }
    }
  }

  public override void AgentAction(float[] vectorAction, string textAction) {

    //Rewards
    //AddReward(-0.1f);
    episodeTime += 1;

    if(!player.Alive) {
      Done();
      AddReward(-1f);
    }

    if(!player.Opponent.Alive) {
      Done();
      AddReward(2 + 1/episodeTime * timeReward);
    }

    //Positioning
    float diffX = Mathf.Abs(player.location.y - player.opponent.location.x);
    float diffY = Mathf.Abs(player.location.y - player.opponent.location.x);

    if(diffX < 2 && diffY < 2) {
      AddReward(0.1f);
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
        break;
      case 5:
        actionSucceeded = player.ShootDown();
        break;
      case 6:
        actionSucceeded = player.ShootLeft();
        break;
      case 7:
        actionSucceeded = player.ShootRight();
        break;
      default:
        AddReward(-0.1f);
        break;
    }

    //if(!actionSucceeded) {
      //AddReward(-0.1f);
    //}
  }
}
