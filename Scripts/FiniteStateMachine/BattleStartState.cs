using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : BattleStartState.cs
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
