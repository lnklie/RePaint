using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : BattleModeState.cs
==============================
*/
public class BattleModeState : State
{
    [Header("State")]
    [SerializeField]
    private BattleSelectState battleSelectState = null;
    public override State RunCurrentState()
    {
        if(!selectManager.IsSelected())
            battleSceneUIManager.ActiveBattlePickBtn(false);
        if (Input.GetMouseButtonUp(0))
        {
            leftP3 = Input.mousePosition;
            return battleSelectState;
        }
        
        return this;
    }
}
