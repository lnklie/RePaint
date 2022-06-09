using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : SingleTonManager.cs
==============================
*/
public class SingleTonManager<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if(applicationQuitting)
            {
                Debug.Log("[싱글톤] 게임 종료" + typeof(T));
                return null;
            }

            lock(_lock)
            {
                if(_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if(FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        return _instance;
                    }
                    if(_instance == null)
                    {
                        GameObject singleTon = new GameObject();
                        _instance = singleTon.AddComponent<T>();
                        singleTon.name = typeof(T).ToString();

                        DontDestroyOnLoad(singleTon);
                    }
                    else
                    {
                        DontDestroyOnLoad(_instance);
                    }
                }
                return _instance;
            }
        }
    }
    private static bool applicationQuitting = false;

    public void OnDestroy()
    {
        applicationQuitting = true;
    }
}
