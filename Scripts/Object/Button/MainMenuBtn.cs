using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : JSJ
 * 파일명 : MainMenuBtn.cs
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
