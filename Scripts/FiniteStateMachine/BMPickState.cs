using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : BMPickState.cs
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
