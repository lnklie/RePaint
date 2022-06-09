using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : DeploymentSceneUIManager.cs
==============================
*/
public class DeploymentSceneUIManager : MonoBehaviour
{
    [Header("Deployment Mode UI")]
    [SerializeField] private Image dollListHolder = null;
    [SerializeField] private Image chooseJob = null;
    [SerializeField] private Image gameStart = null;
    [SerializeField] private Image listBtn = null;
    [SerializeField] private Image deploymentPickBtn = null;
    

    [Header("Deployment Mode UI Elements")]
    [SerializeField] private List<GameObject> nameTag = new List<GameObject>();
    [SerializeField] private Sprite[] knightSprites = null;
    [SerializeField] private Sprite[] knightMark = null;
    [SerializeField] private Image[] knightImages = null;

    [SerializeField] private GameObject arrows = null;
    [SerializeField] private GameObject lineUpBtns = null;
    [SerializeField] private Text listNumTxt = null;
    [SerializeField] private Text[] nameTagNameText = null;
    [SerializeField] private Text[] nameTagRankText = null;
    [SerializeField] private Image[] KnightMark = null;
    [SerializeField] private Button[] nameTagBtn = null;
    [SerializeField] private Button deployPickBtn = null;

    [SerializeField] private Button startBattleBtn;
    [SerializeField] private Image readyImage = null;

    [Header("Knight Manager")]
    [SerializeField] private KnightManager knightManager = null;

    [Header("Select Manager")]
    [SerializeField] private SelectManager selectManager = null;
    
    [Header("State Manager")]
    [SerializeField] private StateManager stateManager = null;
    [Header("Select Box Holder")]
    [SerializeField] private GameObject selectBoxParent = null;
    [Header("AllyKnightManager")]
    [SerializeField] private AllyKnightsManager allyKnightsManager = null;

    private List<KnightInformation> infos = new List<KnightInformation>();
    private RectTransform listBtnPos = null;

    private Animator anim = null;

    private GameObject selectBox = null;
    private Image selectBoxImage = null;

    private bool isList = false;
    private int listIndexNum = 0;
    private int listNum = 0;
    private int curListType = 0;

    private void Start()
    {
        stateManager = GameObject.FindObjectOfType<StateManager>();
        allyKnightsManager = GameObject.FindObjectOfType<AllyKnightsManager>();
        anim = this.gameObject.GetComponentInChildren<Animator>();
        listBtnPos = listBtn.GetComponent<RectTransform>();
        knightManager = GameManager.Instance.GetComponentInChildren<AllyKnightsManager>();
        infos.AddRange(knightManager.GetComponentsInChildren<KnightInformation>());
        deployPickBtn.onClick.AddListener(() => DeploymentPick());
        startBattleBtn.onClick.AddListener(() => StartBattleMode());
        AddListOnButtonListener();
        AddJobButtonListener();
        AddLineUpButtonListener();
        InitSelectBox();
    }

    private void InitSelectBox()
    {
        // 선택 박스 초기화
        selectBox = new GameObject("SelectBox");
        selectBox.transform.SetParent(selectBoxParent.transform);
        selectBoxImage = selectBox.AddComponent<Image>();
        selectBoxImage.color = new Color(204 / 255, 299 / 255, 1f, 0.1f);
        selectBox.AddComponent<Outline>().effectColor = new Color(0f, 0f, 0f);
        selectBox.GetComponent<Outline>().effectDistance = new Vector2(5f, 5f);
        selectBox.SetActive(false);
    }
    private IEnumerator ActiveReadyImage()
    {
        // 준비 이미지 활성화
        readyImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        readyImage.gameObject.SetActive(false);

    }
    public void DrawSelectBox(Vector3 _p1, Vector3 _p2)
    {
        // 선택 박스 그리기
        selectBox.SetActive(true);
        float smallX = (_p1.x < _p2.x) ? _p2.x - _p1.x : _p1.x - _p2.x;
        float smallY = (_p2.y < _p1.y) ? _p1.y - _p2.y : _p2.y - _p1.y;

        int rectX = (_p1.x < _p2.x) ? 0 : 1;
        int rextY = (_p1.y < _p2.y) ? 0 : 1;
        selectBoxImage.rectTransform.anchorMax = new Vector2(rectX, rextY);
        selectBoxImage.rectTransform.anchorMin = new Vector2(rectX, rextY);
        selectBoxImage.rectTransform.pivot = new Vector2(rectX, rextY);
        selectBoxImage.rectTransform.position = _p1;
        selectBoxImage.rectTransform.sizeDelta = new Vector3(smallX, smallY);
    }
    private void InitBtn()
    {
        // 버튼 초기화
        for (int i = 0; i < 4; i++)
        {
            knightImages[i].sprite = knightSprites[i];
        }
    }

    public void ActiveNameTagging(int _type)
    {
        // 네임태그 활성화
        List<KnightInformation> knightInformation = knightManager.GetKnightInformationListWithType(_type);
        listIndexNum = knightInformation.Count / nameTag.Capacity;

        int temp2 = listNum == listIndexNum? knightInformation.Count % 8 : 8;
        for (int i = 0; i < nameTag.Capacity; i++)
        {
            nameTag[i].SetActive(false);
        }
        for(int j = 0; j < listNum + 1; j++)
        {

            for (int i = 0; i < temp2; i++)
            {
                nameTag[i].SetActive(true);
                
                int temp = i;
                nameTagNameText[i].text = knightInformation[(8 * j) + i].KnightName;
                nameTagRankText[i].text = knightInformation[(8 * j) + i].KnightRank.ToString();
                KnightMark[i].sprite = knightMark[_type];
                nameTagBtn[i].onClick.RemoveAllListeners();
                nameTagBtn[i].onClick.AddListener(() => { NameTagSelect(temp, _type); });
            }
        }
    }


    #region UIActive
    public void UIOnOffWithoutJobList(bool _bool)
    {
        // 선택/배치 드래그를 생성할 때 UI막기
        listBtn.gameObject.SetActive(_bool);

        foreach (Image image in dollListHolder.GetComponentsInChildren<Image>())
        {
            image.enabled = _bool;
        }
        foreach (Text text in dollListHolder.GetComponentsInChildren<Text>())
        {
            text.enabled = _bool;
        }

        gameStart.gameObject.SetActive(_bool);
    }

    public void ActiveSelectBox(bool _bool)
    {
        // 선택 박스 활성화 여부
        selectBox.SetActive(_bool);
    }
    public void ActiveChooseJobList(bool _isJoblist)
    {
        // 병종 선택 리스트 선택 활성화 여부
        chooseJob.gameObject.SetActive(_isJoblist);
    }
    public void ActiveDeploymentPickBtn(bool _bool)
    {
        // 배치 모드 시 집기 버튼 활성화 여부
        deploymentPickBtn.gameObject.SetActive(_bool);
    }
    #endregion

    #region Button Function

    public void StartBattleMode()
    {
        // 배틀 모드 시작
        if (allyKnightsManager.IsReady())
            stateManager.SetCurrentState(stateManager.GetComponentInChildren<BattleStartState>());
        else
            StartCoroutine(ActiveReadyImage());
    }
    public void NameTagSelect(int _index,int _type)
    {
        // 네임 태그 선택
        if (selectManager.IsListSelected(knightManager.GetKnight(_index, _type)))
        {
            knightManager.GetKnight((listNum * nameTag.Capacity) + _index, _type).gameObject.GetComponent<OutlinesController>().enabled = false;
            selectManager.SelectedList.Remove(knightManager.GetKnight(_index, _type).gameObject);
        } 
        else
        {
            selectManager.OnListSelect(knightManager.GetKnight(( listNum * nameTag.Capacity) + _index, _type));
        }
    }
    public void ListOnBtn()
    {
        // 리스트 열기 버츤
        isList = !isList;
        anim.SetBool("ListOpen", isList);
        listNum = 0;
        listNumTxt.text = (listNum + 1).ToString();
        AddListControlListener();
        if (!isList)
        {
            listBtnPos.localScale = new Vector3(-1f, 1f, 1f);
            listBtnPos.position += new Vector3(-40f, 0f, 0f);
            InitBtn();
        }
        else
        {
            listBtnPos.localScale = new Vector3(1f, 1f, 1f);
            listBtnPos.position += new Vector3(40f, 0f, 0f);
            ActiveNameTagging(0);
            knightImages[0].sprite = knightSprites[4];
        }
    }

    public void LineUpButton(int _knightNum)
    {
        // 정렬 버튼
        if(isList)
        {
            InitBtn();
            curListType = _knightNum;
            listNum = 0;
            ActiveNameTagging(curListType);
            knightImages[curListType].sprite = knightSprites[curListType + 4];
            listNumTxt.text = (listNum + 1).ToString();
        }
        selectManager.SetSelected(false);
    }
    private void ArrowButton(int _num)
    {
        // 리스트 선택 바꾸기  
        if (_num == 0)
        {
            if(listNum != 0)
            {
                listNum--;
            }
        }
        else
        {
            if (listNum != listIndexNum )
            {
                listNum++;
            }
        }
        listNumTxt.text = (listNum + 1).ToString();
        ActiveNameTagging(curListType);
    }

    #endregion

    // 버튼 추가해주기
    #region Add Button

    private void AddListOnButtonListener()
    {
        listBtn.GetComponent<Button>().onClick.AddListener(() => ListOnBtn());
    }
    private void AddLineUpButtonListener()
    {
        Button[] button = lineUpBtns.GetComponentsInChildren<Button>();

        for (int i = 0; i < button.Length; i++)
        {
            int tmp = i;
            button[i].onClick.AddListener(() => LineUpButton(tmp));
        }
    }
    private void AddListControlListener()
    {
        Button[] buttons = arrows.GetComponentsInChildren<Button>(); ;
        for (int i= 0; i < buttons.Length; i++)
        {
            int idx = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => ArrowButton(idx));
        }
    }
    private void AddJobButtonListener()
    {
        Button[] jobButtons = chooseJob.GetComponentsInChildren<Button>();
        for (int i = 0; i < jobButtons.Length; i++)
        {
            int knightType = i + 1;
            jobButtons[i].onClick.AddListener(
                () => knightManager.GetComponent<KnightCustomizer>().SetKnightType(knightType));
            jobButtons[i].onClick.AddListener(
                () => knightManager.GetComponent<KnightCustomizer>().CustomizeSelectedKnight());
            jobButtons[i].onClick.AddListener(
                () => knightManager.InputKnight(knightType));
            jobButtons[i].onClick.AddListener(
                () => LineUpButton(knightType));

        }
    }

    #endregion
    public void DeploymentPick()
    {
        // 상태 변경하기
        stateManager.SetCurrentState(stateManager.GetComponentInChildren<DMPickState>());
    }
}
