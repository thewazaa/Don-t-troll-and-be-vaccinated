using System.Collections;
using UnityEngine;

interface IMovement
{
    bool CheckNextState(bool isInFear);

    bool CheckNeedToMove();

    void GetNextPosition();

    PlayerController.EState GetNextState();

    void InitState();
    void ConfigState();
}