using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : State.cs
==============================
*/
public abstract class State : MonoBehaviour
{
    protected static DeploymentSceneUIManager deploymentSceneUIManager = null;
    protected static BattleSceneUIManager battleSceneUIManager = null;
    protected static SelectManager selectManager = null;
    protected static DeploymentManager deploymentManager = null;
    protected static BattleSceneManager battleSceneManager = null;
    protected static Vector3 leftP1 = default; // 클릭
    protected static Vector3 leftP2 = default; // 드래그
    protected static Vector3 leftP3 = default; // 떼기

    protected static Vector3 rightP1 = default;
    protected static Vector3 rightP2 = default;
    protected static Vector3 rightP3 = default;

    public abstract State RunCurrentState();

    private void Start()
    {
        deploymentSceneUIManager = GameObject.FindObjectOfType<DeploymentSceneUIManager>();
        battleSceneUIManager = GameObject.FindObjectOfType<BattleSceneUIManager>();
        selectManager = GameObject.FindObjectOfType<SelectManager>();
        deploymentManager = GameObject.FindObjectOfType<DeploymentManager>();
        battleSceneManager = GameObject.FindObjectOfType<BattleSceneManager>();
    }

}
