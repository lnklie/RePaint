using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : BattleSelectState.cs
==============================
*/
public class BattleSelectState : State
{
    [Header("State")]
    [SerializeField]
    private BattleModeState battleModeState = null;
    public override State RunCurrentState()
    {
        selectManager.OnBattleSingleSelect(leftP3);
        if (selectManager.IsSelected())
        {
            battleSceneUIManager.ActiveBattlePickBtn(true);
            battleSceneUIManager.TurnOnKnightStatus();
        }
        else
        {
            battleSceneUIManager.TurnOffKnightStatus();
            battleSceneUIManager.ActiveBattlePickBtn(false);
        }
        return battleModeState;
    }
}
