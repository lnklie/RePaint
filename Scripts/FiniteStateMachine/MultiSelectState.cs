using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : MultiSelectState.cs
==============================
*/
public class MultiSelectState : State
{
    [Header("State")]
    [SerializeField]
    private DeploymentModeState deploymentModeState = null;
    public override State RunCurrentState()
    {
        selectManager.OnDragSelect(leftP1, leftP3);
        deploymentSceneUIManager.UIOnOffWithoutJobList(true);
        return deploymentModeState;
    }
}
