using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
==============================
 * ���������� : 2022-06-05
 * �ۼ��� : Inklie
 * ���ϸ� : BattleSceneUIManager.cs
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
        // UI ��� ����
        for (int i = 0; i < inactiveUI.Length; i++)
            inactiveUI[i].SetActive(false);
    }

    public void UpdateAllyCountUI(int _aliveCount,int _totalCount)
    {
        // �Ʊ� ���� �� ������Ʈ 
        allyCountGage.value = _aliveCount / (float)_totalCount;
    }
    public void UpdateEnemyCountUI(int _aliveCount, int _totalCount)
    {
        // �� ���� �� ������Ʈ
        enemyCountGage.value = _aliveCount / (float)_totalCount;
    }

    public void TurnOnKnightStatus()
    {
        // ���� �������ͽ�â ����
        selectedKnight = selectManager.SelectedList[0].GetComponentInChildren<KnightInformation>();
        knightStatus[0].text = selectedKnight.KnightName;
        switch(selectedKnight.KnightType)
        {
            case EKnightType.Default:
                knightStatus[1].text = "����";
                break;
            case EKnightType.Sword:
                knightStatus[1].text = "�˺�";
                break;
            case EKnightType.Spear:
                knightStatus[1].text = "â��";
                break;
            case EKnightType.Bow:
                knightStatus[1].text = "�ú�";
                break;

        }
        knightStatus[2].text = selectedKnight.KnightRank.ToString();
        knightStatusImage.gameObject.SetActive(true);

        ActiveDescendGodBtn();
    }
    public void TurnOffKnightStatus()
    {
        // ���� �������ͽ� â �ݱ�
        knightStatusImage.gameObject.SetActive(false);
    }
    public void ActiveBattlePickBtn(bool _bool)
    {
        // ��Ʋ �� ��ư Ȱ��ȭ
        BattlePickBtn.gameObject.SetActive(_bool);
    }
    public void ActiveInfluenceHolder()
    {
        influenceHolder.SetActive(true);
    }
    public void ActiveAscendingBtn(bool _bool)
    {
        // �ö󰡱� ��ư Ȱ��ȭ
        ascendingBtn.gameObject.SetActive(_bool);
    }
    #region ��ư����

    public void BattlePick()
    {
        // ���� ����
        stateManager.SetCurrentState(stateManager.GetComponentInChildren<BMPickState>());
    }
    public void BattleDescend()
    {
        // ���� ����
        stateManager.SetCurrentState(stateManager.GetComponentInChildren<BattleDescendState>());
    }
    public void BattleAscend()
    {
        // ���� ����
        stateManager.SetCurrentState(stateManager.GetComponentInChildren<BattleAscendState>());
    }
    public void ActiveDescendGodBtn()
    {
        // ���� ��ư Ȱ��ȭ
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
        // �ö󰡱� ��ư Ȱ��ȭ
        Knight knight = selectManager.SelectedList[0].GetComponentInChildren<Knight>();
       
        ascendingBtn.onClick.RemoveAllListeners();
        ascendingBtn.onClick.AddListener(() => knight.ToggleControll());
        ascendingBtn.onClick.AddListener(() => battleSceneCamera.CameraViewToggle(knight.GetCamHolder(), BattleAscend));

    }
    #endregion
}
