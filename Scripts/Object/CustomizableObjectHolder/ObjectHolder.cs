using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : JSJ
 * 파일명 : ObjectHolder.cs
==============================
*/
public class ObjectHolder : MonoBehaviour
{
    private GameObject activeObject = null;

    [SerializeField] 
    private GameObject[] objects;
    public void ActivateObject(int _index)
    {
        if (_index < 0 || _index >= objects.Length) return;

        for(int i = 0; i < objects.Length; i++)
            objects[i].SetActive((i == _index));

        activeObject = objects[_index];
    }

    public void InactiveAll()
    {
        for (int i = 0; i < objects.Length; i++)
            objects[i].SetActive(false);

        activeObject = null;
    }

    public GameObject GetActiveObject()
    {
        return activeObject;
    }
}
