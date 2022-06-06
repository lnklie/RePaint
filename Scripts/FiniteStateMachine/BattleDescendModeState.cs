using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : BattleDescendModeState.cs
==============================
*/
public class BattleDescendModeState : State
{
    public override State RunCurrentState()
    {
        battleSceneUIManager.ActiveAscendingBtn(true);
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        return this;
    }
}
