using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : BattleAscendState.cs
==============================
*/
public class BattleAscendState : State
{
    [Header("State")]
    [SerializeField]
    private BattleModeState battleModeState = null;
    public override State RunCurrentState()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        battleSceneUIManager.ActiveAscendingBtn(false);
        return battleModeState;
    }
}

