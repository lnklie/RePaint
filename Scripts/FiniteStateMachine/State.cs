using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : State.cs
==============================
*/
public abstract class State : MonoBehaviour
{
    protected static DeploymentSceneUIManager deploymentSceneUIManager = null;
    protected static BattleSceneUIManager battleSceneUIManager = null;
    protected static SelectManager selectManager = null;
    protected static DeploymentManager deploymentManager = null;
    protected static BattleSceneManager battleSceneManager = null;
    protected static Vector3 leftP1 = default; // Ŭ��
    protected static Vector3 leftP2 = default; // �巡��
    protected static Vector3 leftP3 = default; // ����

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
