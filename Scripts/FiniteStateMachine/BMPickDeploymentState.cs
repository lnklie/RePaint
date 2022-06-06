using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : BMPickDeploymentState.cs
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
