using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : BMPickDeploymentState.cs
==============================
*/
public class BMPickDeploymentState : State
{
    [Header("State")]
    [SerializeField] 
    private BattleModeState battleModeState = null;
    public override State RunCurrentState()
    {
        selectManager.SetPickMode(false);
        deploymentManager.PickDeploy();
        battleSceneUIManager.ActiveBattlePickBtn(false);
        battleSceneUIManager.TurnOffKnightStatus();

        return battleModeState;
        
    }
}
