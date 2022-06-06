using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : DeploymentState.cs
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
