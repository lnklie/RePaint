using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : DefaultUIManager.cs
==============================
*/
public class DefaultUIManager : MonoBehaviour
{
    private AllyKnightsManager allyKnightsManager = null;

    [Header("Default UI")]
    [SerializeField] 
    private Button menuBtn = null;
    [SerializeField] 
    private Button keepMenuBtn = null;
    [SerializeField] 
    private Button exitBtn = null;
    [SerializeField]
    private Image menuImage = null;
    [SerializeField]
    private Image optionImage = null;

    private void Start()
    {
        allyKnightsManager = FindObjectOfType<AllyKnightsManager>();

        AddMenuButtonListener();
        AddMenuKeepButtonListener();
        AddMenuExitButtonListener();
    }
    private void AddMenuButtonListener()
    {
        // �޴� ��ư Ȱ��ȭ
        menuBtn.onClick.AddListener(() => ActiveMenuOnOffButton(true));
    }
    private void AddMenuKeepButtonListener()
    {
        // �޴� ����ϱ� ��ư Ȱ��ȭ
        keepMenuBtn.onClick.AddListener(() => ActiveMenuOnOffButton(false));
    }

    private void AddMenuExitButtonListener()
    {
        // �޴� �׸��α� ��ư Ȱ��ȭ
        exitBtn.onClick.AddListener(() => ExitBattle());
    }

    private void ExitBattle()
    {
        // �޴� ������ ��ư
        Time.timeScale = 1f;
        GameManager.Instance.LoadSceneWithName("WorldMap");
        allyKnightsManager.OnBattleEnd();
    }
    public void ActiveMenuOnOffButton(bool _isActive)
    {
        // �޴� ���� ��ư Ȱ��ȭ
        menuImage.gameObject.SetActive(_isActive);

        Time.timeScale = _isActive ? 0 : 1;
    }
    public void ActiveOptionOnOffButton(bool _isActive)
    {
        /// �ɼ� ��ư Ȱ��ȭ
        optionImage.gameObject.SetActive(_isActive);
    }
}
