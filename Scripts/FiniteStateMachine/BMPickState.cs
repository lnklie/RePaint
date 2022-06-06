using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : BMPickState.cs
==============================
*/
public class BMPickState : State
{
    [Header("State")]
    [SerializeField] 
    private BMPickDeploymentState bmpickDeploymentState = null;
    public override State RunCurrentState()
    {
        if (Input.GetMouseButtonUp(1))
        {
            return bmpickDeploymentState;
        }
        else
        {
            selectManager.SetPickMode(true);
            selectManager.Pick();
            battleSceneUIManager.ActiveBattlePickBtn(false);
            battleSceneUIManager.TurnOffKnightStatus();

            return this;
        }

    }
}
