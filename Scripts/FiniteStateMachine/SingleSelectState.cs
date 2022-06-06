using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : SingleSelectState.cs
==============================
*/
public class SingleSelectState : State
{
    [Header("State")]
    [SerializeField]
    private DeploymentModeState deploymentModeState = null;
   
    public override State RunCurrentState()
    {
        selectManager.OnSingleSelect(leftP1);
        if(selectManager.SelectedList.Count == 1)
            deploymentSceneUIManager.ActiveDeploymentPickBtn(true);
        else
        {
            deploymentSceneUIManager.ActiveDeploymentPickBtn(false);
        }
        return deploymentModeState;
    }
}
