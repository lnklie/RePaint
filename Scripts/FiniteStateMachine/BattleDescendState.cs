using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : BattleDescendState.cs
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
