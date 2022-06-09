using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : JSJ
 * ���ϸ� : MainMenuBtn.cs
==============================
*/
public class MainMenuBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject load = null;
    [SerializeField] 
    private GameObject setting = null;
    [SerializeField] 
    private GameObject creators = null;

    private void Start()
    {
        load.SetActive(false);
        setting.SetActive(false);
        creators.SetActive(false);
    }

    public void LoadSetActive()
    {
        load.SetActive(true);
    }

    public void SettingSetActive()
    {
        setting.SetActive(true);
    }

    public void CreatorsSetActive()
    {
        creators.SetActive(true);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
