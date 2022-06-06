using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : BattleModeState.cs
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
