using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : MultiSelectState.cs
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
