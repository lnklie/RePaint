using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : BattleSceneUIManager.cs
==============================
*/
public class BattleSceneUIManager : MonoBehaviour
{
    [Header("State Manager")]
    [SerializeField] private StateManager stateManager = null;
    [Header("InactiveUIs")]
    [SerializeField] private GameObject[] inactiveUI = null;

    private Text[] knightStatus = null;
    private KnightInformation selectedKnight = null;

    [SerializeField] 
    private SelectManager selectManager = null;

    [SerializeField] 
    private CameraController battleSceneCamera = null;

    [SerializeField] 
    private Image knightStatusImage;
    [SerializeField] 
    private Image BattlePickBtn = null;
    [SerializeField]
    private GameObject influenceHolder = null;

    [SerializeField]
    private Slider allyCountGage;
    [SerializeField] 
    private Slider enemyCountGage;

    [SerializeField]
    private Button battlePickBtn;
    [SerializeField]
    private Button ascendingBtn;



    private void Start()
    {
        knightStatus = knightStatusImage.GetComponentsInChildren<Text>();
        stateManager = GameObject.FindObjectOfType<StateManager>();

        battlePickBtn.onClick.AddListener(() => BattlePick());
        InactiveUI();
    }

    private void InactiveUI()
    {
        // UI 모두 끄기
        for (int i = 0; i < inactiveUI.Length; i++)
            inactiveUI[i].SetActive(false);
    }

    public void UpdateAllyCountUI(int _aliveCount,int _totalCount)
    {
        // 아군 유닛 수 업데이트 
        allyCountGage.value = _aliveCount / (float)_totalCount;
    }
    public void UpdateEnemyCountUI(int _aliveCount, int _totalCount)
    {
        // 적 유닛 수 업데이트
        enemyCountGage.value = _aliveCount / (float)_totalCount;
    }

    public void TurnOnKnightStatus()
    {
        // 병사 스테이터스창 열기
        selectedKnight = selectManager.SelectedList[0].GetComponentInChildren<KnightInformation>();
        knightStatus[0].text = selectedKnight.KnightName;
        switch(selectedKnight.KnightType)
        {
            case EKnightType.Default:
                knightStatus[1].text = "인형";
                break;
            case EKnightType.Sword:
                knightStatus[1].text = "검병";
                break;
            case EKnightType.Spear:
                knightStatus[1].text = "창병";
                break;
            case EKnightType.Bow:
                knightStatus[1].text = "궁병";
                break;

        }
        knightStatus[2].text = selectedKnight.KnightRank.ToString();
        knightStatusImage.gameObject.SetActive(true);

        ActiveDescendGodBtn();
    }
    public void TurnOffKnightStatus()
    {
        // 병사 스테이터스 창 닫기
        knightStatusImage.gameObject.SetActive(false);
    }
    public void ActiveBattlePickBtn(bool _bool)
    {
        // 배틀 픽 버튼 활성화
        BattlePickBtn.gameObject.SetActive(_bool);
    }
    public void ActiveInfluenceHolder()
    {
        influenceHolder.SetActive(true);
    }
    public void ActiveAscendingBtn(bool _bool)
    {
        // 올라가기 버튼 활성화
        ascendingBtn.gameObject.SetActive(_bool);
    }
    #region 버튼관리

    public void BattlePick()
    {
        // 상태 변경
        stateManager.SetCurrentState(stateManager.GetComponentInChildren<BMPickState>());
    }
    public void BattleDescend()
    {
        // 상태 변경
        stateManager.SetCurrentState(stateManager.GetComponentInChildren<BattleDescendState>());
    }
    public void BattleAscend()
    {
        // 상태 변경
        stateManager.SetCurrentState(stateManager.GetComponentInChildren<BattleAscendState>());
    }
    public void ActiveDescendGodBtn()
    {
        // 강림 버튼 활성화
        Knight knight = selectManager.SelectedList[0].GetComponentInChildren<Knight>();
        Button button = knightStatusImage.GetComponentInChildren<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => knight.ToggleControll());
        button.onClick.AddListener(() => battleSceneCamera.CameraViewToggle(knight.GetCamHolder()));
        button.onClick.AddListener(() => BattleDescend());

        ActiveAscendGodBtn();
    }
    public void ActiveAscendGodBtn()
    {
        // 올라가기 버튼 활성화
        Knight knight = selectManager.SelectedList[0].GetComponentInChildren<Knight>();
       
        ascendingBtn.onClick.RemoveAllListeners();
        ascendingBtn.onClick.AddListener(() => knight.ToggleControll());
        ascendingBtn.onClick.AddListener(() => battleSceneCamera.CameraViewToggle(knight.GetCamHolder(), BattleAscend));

    }
    #endregion
}
