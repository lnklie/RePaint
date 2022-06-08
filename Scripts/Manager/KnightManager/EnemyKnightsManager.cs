using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : JSJ
 * ���ϸ� : EnemyKnightsManager.cs
==============================
*/
public class EnemyKnightsManager : KnightManager
{
    [SerializeField]
    private KnightCustomizer customizer = null;

    protected override void Awake()
    {
        knights = new List<Knight>(GetComponentsInChildren<Knight>());
    }

    protected override void Start()
    {
        if(customizer)
            customizer.CustomizeEnemyKnight(knights);

        gameObject.SetActive(false);
    }
}