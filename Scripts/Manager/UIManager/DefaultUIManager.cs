using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : DefaultUIManager.cs
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
        // 메뉴 버튼 활성화
        menuBtn.onClick.AddListener(() => ActiveMenuOnOffButton(true));
    }
    private void AddMenuKeepButtonListener()
    {
        // 메뉴 계속하기 버튼 활성화
        keepMenuBtn.onClick.AddListener(() => ActiveMenuOnOffButton(false));
    }

    private void AddMenuExitButtonListener()
    {
        // 메뉴 그만두기 버튼 활성화
        exitBtn.onClick.AddListener(() => ExitBattle());
    }

    private void ExitBattle()
    {
        // 메뉴 나가기 버튼
        Time.timeScale = 1f;
        GameManager.Instance.LoadSceneWithName("WorldMap");
        allyKnightsManager.OnBattleEnd();
    }
    public void ActiveMenuOnOffButton(bool _isActive)
    {
        // 메뉴 끄기 버튼 활성화
        menuImage.gameObject.SetActive(_isActive);

        Time.timeScale = _isActive ? 0 : 1;
    }
    public void ActiveOptionOnOffButton(bool _isActive)
    {
        /// 옵션 버튼 활성화
        optionImage.gameObject.SetActive(_isActive);
    }
}
