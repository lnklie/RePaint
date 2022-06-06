using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : BattleDescendState.cs
==============================
*/
public class BattleDescendState : State
{
    [Header("State")]
    [SerializeField]
    private BattleDescendModeState battleDescendModeState = null;
    public override State RunCurrentState()
    {
        selectManager.ListInitialization();
        battleSceneUIManager.ActiveBattlePickBtn(false);

        battleSceneUIManager.TurnOffKnightStatus();

        return battleDescendModeState;
    }
}
