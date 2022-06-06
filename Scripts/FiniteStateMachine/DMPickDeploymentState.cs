using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : DMPickDeploymentState.cs
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
