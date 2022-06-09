using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-06
 * �ۼ��� : JSJ
 * ���ϸ� : BattleBtn.cs
==============================
*/
public class BattleBtn : MonoBehaviour
{
    private Camera main = null;
    void Start()
    {
        main = Camera.main;
    }

    void Update()
    {
        BtnRotate();
    }

    private void BtnRotate()
    {
        Vector3 dir = transform.position - main.transform.position;
        transform.LookAt(transform.position + dir);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }
}
