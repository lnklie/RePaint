using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : BattleAscendState.cs
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

