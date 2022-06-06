using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : BattleStartState.cs
==============================
*/
public class BattleStartState : State
{
    [Header("State")]
    [SerializeField] 
    private BattleModeState battleModeState = null;
    public override State RunCurrentState()
    {
        battleSceneManager.StartBattle();
        selectManager.ListInitialization();
        return battleModeState;
    }
}
