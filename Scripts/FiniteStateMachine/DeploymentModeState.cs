using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : DeploymentModeState.cs
==============================
*/
public class DeploymentModeState : State
{
    [Header("State")]
    [SerializeField] 
    private SelectDraggingState selectDraggingState = null;
    [SerializeField] 
    private SingleSelectState singleSelectState = null;
    [SerializeField] 
    private DeploymentDraggingState deploymentDraggingState = null;
    [SerializeField] 
    private DeploymentState deploymentState = null;

    public override State RunCurrentState()
    {
        deploymentSceneUIManager.ActiveSelectBox(false);
        if(selectManager.SelectedList.Count == 1)
            deploymentSceneUIManager.ActiveDeploymentPickBtn(true);
        deploymentSceneUIManager.ActiveChooseJobList(selectManager.IsSelected());

        if (Input.GetMouseButtonDown(0))
        {
            leftP1 = Input.mousePosition;
            return this;
        }

            if (Input.GetMouseButton(0)&& !EventSystem.current.IsPointerOverGameObject(-1))
            {
                leftP2 = Input.mousePosition;

                if (Mathf.Abs((leftP1 - leftP2).magnitude) > 40f)
                {
                    return selectDraggingState;
                }
                else
                    return this;
            }
        
        if (!EventSystem.current.IsPointerOverGameObject(-1))
        {
            if (Input.GetMouseButtonUp(0))
            {
                return singleSelectState;
            }
        }

            if(selectManager.IsSelected())
            {
            if (!EventSystem.current.IsPointerOverGameObject(-1))
            {
                if (Input.GetMouseButtonDown(1))
                {
                    rightP1 = Input.mousePosition;
                }
            }
                if (Input.GetMouseButton(1) && selectManager.SelectedList.Count > 0)
                {
                    rightP2 = Input.mousePosition;
 
                    return deploymentDraggingState;
                }
            if (!EventSystem.current.IsPointerOverGameObject(-1))
            {
                if (Input.GetMouseButtonUp(1))
                {
                    rightP3 = Input.mousePosition;

                    return deploymentState;
                }
            }
        }
        return this;  
    }
}
