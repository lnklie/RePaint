using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : DMPickDeploymentState.cs
==============================
*/
public class DMPickDeploymentState : State
{
    [Header("State")]
    [SerializeField] 
    private DeploymentModeState deploymentModeState = null;
    
    public override State RunCurrentState()
    {
        deploymentSceneUIManager.UIOnOffWithoutJobList(true);
        selectManager.SetPickMode(false);
        deploymentSceneUIManager.ActiveDeploymentPickBtn(false);
        deploymentManager.PickDeploy();
        return deploymentModeState;
        
    }
}
