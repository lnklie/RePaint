using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : DeploymentState.cs
==============================
*/
public class DeploymentState : State
{
    [Header("State")]
    [SerializeField] 
    DeploymentModeState deploymentModeState = null;

    public override State RunCurrentState()
    {
        selectManager.SetSelected(false);
        deploymentManager.DeployUnits();
        deploymentSceneUIManager.UIOnOffWithoutJobList(true);
        return deploymentModeState;
    }
}
