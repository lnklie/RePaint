using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : DMPickState.cs
==============================
*/
public class DMPickState : State
{
    [Header("State")]
    [SerializeField] 
    private DMPickDeploymentState dmpickDeploymentState = null;
    public override State RunCurrentState()
    {
        if (Input.GetMouseButtonUp(1))
        {
            return dmpickDeploymentState;
        }
        else
        {
            selectManager.SetPickMode(true);
            selectManager.Pick();
            deploymentSceneUIManager.ActiveDeploymentPickBtn(false);
            deploymentSceneUIManager.UIOnOffWithoutJobList(false);
            deploymentSceneUIManager.ActiveChooseJobList(false);

            
            return this;
        }

    }
}
