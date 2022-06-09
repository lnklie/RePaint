using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-06
 * �ۼ��� : Inklie
 * ���ϸ� : JSJ.cs
==============================
*/
public class ArrowPool : ObjectPool
{
    protected override void Awake()
    {
        onInstantiate += ReturnToPool;
        base.Awake();
    }

    private void ReturnToPool(GameObject _go)
    {
        _go.GetComponent<Arrow>().onHit += () =>
        {
            StartCoroutine(ReturnToPoolCoroutine(_go));
        };
    }

    private IEnumerator ReturnToPoolCoroutine(GameObject _go)
    {
        yield return new WaitForSeconds(6f);
        _go.transform.SetParent(transform);
        ReturnObject(_go);
    }
}
