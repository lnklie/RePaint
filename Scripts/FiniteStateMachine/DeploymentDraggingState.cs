using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : DeploymentDraggingState.cs
==============================
*/
public class DeploymentDraggingState : State
{
    [Header("State")]
    [SerializeField] 
    private DeploymentState deploymentState = null;

    public override State RunCurrentState()
    {
        if (Input.GetMouseButtonUp(1))
        {
            rightP3 = Input.mousePosition;
            return deploymentState;
        }

        if(Input.GetMouseButton(1))
        {
            rightP2 = Input.mousePosition;
            deploymentManager.OnDeployDrag(rightP1, rightP2);
            deploymentSceneUIManager.UIOnOffWithoutJobList(false);
            deploymentSceneUIManager.ActiveChooseJobList(false);
            deploymentSceneUIManager.ActiveDeploymentPickBtn(false);
            return this;
        }
        else 
            return this;
    }
}
