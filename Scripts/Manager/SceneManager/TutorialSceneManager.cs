using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : JSJ
 * 파일명 : TutorialSceneManager.cs
==============================
*/
public class TutorialSceneManager : MonoBehaviour
{
    [Header("InactiveButtonList")]
    [SerializeField] List<Button> inactiveButtons = new List<Button>();

        
    private Vector2 originCursorScale = Vector2.zero;
    private TutorialExplain tutorialExplain = null;
    private AllyKnightsManager knightsManager = null;
    private bool isQuestComplete = false;
    private int idx = 0;

    private Knight descendKnight = null;

    private float elapsed = 0f;
    private float elapsed2 = 0f;

    [SerializeField] private Text explain = null;
    [SerializeField] private Image explainPanel = null;
    [SerializeField] private Image emphasizeCircle = null;
    [SerializeField] private Image tutorialBattlePanel = null;
    [SerializeField] private Button tutorialButton = null;
    [SerializeField] private Image cursorImage = null;
    [SerializeField] private Image tutorialBattleButtonImg = null;

    [SerializeField] private DeploymentSceneUIManager deploymentSceneUIManager = null;
    [SerializeField] private BattleSceneUIManager battleSceneUIManager = null;
    [SerializeField] private StateManager stateManager = null;
    [SerializeField] private SelectManager selectManager = null;
    [SerializeField] private CameraController battleSceneCamera = null;
    [SerializeField] private AllyKnightsManager allyKnightsManager = null;

    [SerializeField] private WorldTrigger worldTrigger = null;
    [SerializeField] private DollManagePanel dollManagePanel = null; 

    private void Awake()
    {
        allyKnightsManager = GameObject.FindObjectOfType<AllyKnightsManager>();
        var objs = FindObjectsOfType<TutorialSceneManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        tutorialExplain = GetComponent<TutorialExplain>();
        knightsManager = GameObject.FindObjectOfType<AllyKnightsManager>();
        worldTrigger = GameObject.FindObjectOfType<WorldTrigger>();
        dollManagePanel = GameObject.FindObjectOfType<DollManagePanel>();
        allyKnightsManager = GameObject.FindObjectOfType<AllyKnightsManager>();
        inactiveButtons.AddRange(worldTrigger.GetComponentsInChildren<Button>());
        inactiveButtons.AddRange(dollManagePanel.GetComponentsInChildren<Button>());
        originCursorScale = cursorImage.rectTransform.sizeDelta;
        ButtonInactive(false);
        StartCoroutine(Conversation());
    }
    private void Update()
    {
        ClickCursor(new Vector2(150, -150), originCursorScale);

        if (Input.GetMouseButtonUp(0) && isQuestComplete)
        {
            isQuestComplete = false;
            idx++;
        }
    }
    private void ButtonInactive(bool _bool)
    {
        // 버튼 활성화 여부
        for(int i= 0; i < inactiveButtons.Count; i++)
        {
            inactiveButtons[i].interactable = _bool;
        }
    }
    private IEnumerator Conversation()
    {
        // 듀토리얼 대화문
        while (true)
        {
            switch (idx)
        {
            case 0:
                explain.text = tutorialExplain.Explain[0];
                isQuestComplete=true;
                break;
            case 1:
                explain.text = tutorialExplain.Explain[1];
                isQuestComplete = true;
                break;
            case 2:
                explain.text = tutorialExplain.Explain[2];
                isQuestComplete = true;
                break;
            case 3:
                explain.text = tutorialExplain.Explain[3];
                isQuestComplete = true;
                break;
            case 4:
                explain.text = tutorialExplain.Explain[4];
                isQuestComplete = true;
                break;
            case 5:
                explain.text = tutorialExplain.Explain[5];
                isQuestComplete = true;
                break;
            case 6:
                explain.text = tutorialExplain.Explain[6];
                isQuestComplete = true;
                break;
            case 7:
                EmphasizeCircle(new Vector2(430, -20), new Vector2(200, 200));
                isQuestComplete = true;
                break;
            case 8:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[7];
                isQuestComplete = true;
                break;
            case 9:
                explain.text = tutorialExplain.Explain[8];
                isQuestComplete = true;
                break;
            case 10:
                explain.text = tutorialExplain.Explain[9];
                isQuestComplete = true;
                break;
            case 11:
                explain.text = tutorialExplain.Explain[10];
                isQuestComplete = true;
                break;
            case 12:
                explain.text = tutorialExplain.Explain[11];
                ButtonSetting(new Vector2(-390, -120), new Vector2(100, 100), ActiveTutorialBattlePanel);
                isQuestComplete = true;
                break;
            case 13:
                EmphasizeCircle(new Vector2(-390, -120), new Vector2(100, 100));
                break;
            case 14:
                tutorialButton.gameObject.SetActive(false);
                InitEmphasizeCircle();
                ActiveExplainPanel(true);
                explain.text = tutorialExplain.Explain[12];
                isQuestComplete = true;
                break;
            case 15:
                explain.text = tutorialExplain.Explain[13];
                isQuestComplete = true;
                break;
            case 16:
                EmphasizeCircle(Vector2.zero,new Vector2(500, 500));
                isQuestComplete = true;
                break;
            case 17:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[14];
                isQuestComplete = true;
                break;
            case 18:
                explain.text = tutorialExplain.Explain[15];
                ButtonSetting(new Vector2(0, -195), new Vector2(50, 50),MoveToTutorialBattle);
                isQuestComplete = true;
                break;
            case 19:
                EmphasizeCircle(new Vector2(0,-195),new Vector2(50,50));
                break;
            case 20:
                emphasizeCircle.gameObject.SetActive(true);
                tutorialButton.gameObject.SetActive(false);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[16];
                isQuestComplete = true;
                break;
            case 21:
                explain.text = tutorialExplain.Explain[17];
                isQuestComplete = true;
                break;
            case 22:
                EmphasizeCircle(new Vector2(0, 0), new Vector2(200, 200));
                isQuestComplete = true;
                break;
            case 23:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[18];
                isQuestComplete = true;
                break;
            case 24:
                explain.text = tutorialExplain.Explain[19];
                isQuestComplete = true;
                stateManager.SetCurrentState(stateManager.GetComponentInChildren<DeploymentModeState>());
                break;
            case 25:
                ActiveExplainPanel(false);
                emphasizeCircle.gameObject.SetActive(false);
                if (selectManager.SelectedList.Count > 0)
                    idx++; 
                break;
            case 26:
                ActiveExplainPanel(true);
                explain.text = tutorialExplain.Explain[20];
                if (stateManager.GetCurrentState() == stateManager.GetComponentInChildren<DeploymentState>())
                    idx++;
                break;
            case 27:
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[21];
                isQuestComplete = true;
                break;
            case 28:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[22];
                if (selectManager.SelectedList.Count == 1)
                    idx++;
                break;
            case 29:
                emphasizeCircle.gameObject.SetActive(true);
                EmphasizeCircle(new Vector2(870, -170), new Vector2(50, 50));
                isQuestComplete = true;
                break;
            case 30:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[23];
                isQuestComplete = true;
                break;
            case 31:
                explain.text = tutorialExplain.Explain[24];
                isQuestComplete = true;
                break;
            case 32:
                ActiveExplainPanel(false);
                emphasizeCircle.gameObject.SetActive(false);
                if (selectManager.SelectedList.Count == allyKnightsManager.GetKnightAll().Length)
                    idx++;
                break;
            case 33:
                stateManager.SetCurrentState(stateManager.GetComponentInChildren<TutorialState>());
                emphasizeCircle.gameObject.SetActive(true);
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[25];
                isQuestComplete = true;
                break;
            case 34:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[26];
                isQuestComplete = true;
                break;
            case 35:
                explain.text = tutorialExplain.Explain[27];
                ButtonSetting(new Vector2(660, -380), new Vector2(50, 50), () =>
                {
                    knightsManager.GetComponent<KnightCustomizer>().SetKnightType(1);
                    knightsManager.GetComponent<KnightCustomizer>().CustomizeSelectedKnight();
                    idx++;
                });
                isQuestComplete = true;
                break;
            case 36:
                EmphasizeCircle(new Vector2(660, -380), new Vector2(50, 50));
                break;
            case 37:
                tutorialButton.gameObject.SetActive(false);
                emphasizeCircle.gameObject.SetActive(true);
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[28];
                isQuestComplete = true;
                break;
            case 38:
                explain.text = tutorialExplain.Explain[29];
                isQuestComplete = true;
                break;
            case 39:
                explain.text = tutorialExplain.Explain[30];
                ButtonSetting(new Vector2(-940, 400), new Vector2(50, 50), BattleSceneListOnButton);
                isQuestComplete = true;
                break;
            case 40:
                EmphasizeCircle(new Vector2(-940, 400), new Vector2(50, 50));
                break;
            case 41:
                tutorialButton.gameObject.SetActive(false);
                EmphasizeCircle(new Vector2(-700, 0), new Vector2(200, 200));
                ButtonSetting(new Vector2(-400, 400), new Vector2(50, 50), BattleSceneListOnButton);
                isQuestComplete = true;
                break;
            case 42:
                EmphasizeCircle(new Vector2(-400, 400), new Vector2(50, 50));
                break;
            case 43:
                tutorialButton.gameObject.SetActive(false);
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[31];
                isQuestComplete = true;
                break;
            case 44:
                explain.text = tutorialExplain.Explain[32];
                isQuestComplete = true;
                break;
            case 45:
                explain.text = tutorialExplain.Explain[33];
                ButtonSetting(new Vector2(0, 430), new Vector2(50, 50), StartBattle);
                isQuestComplete = true;
                break;
            case 46:
                EmphasizeCircle(new Vector2(0, 430), new Vector2(50, 50));
                break;
            case 47:
                emphasizeCircle.gameObject.SetActive(false);
                tutorialButton.gameObject.SetActive(false);
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[34];
                isQuestComplete = true;
                break;
            case 48:
                Time.timeScale = 0.05f;
                explain.text = tutorialExplain.Explain[35];
                if (selectManager.SelectedList.Count == 1)
                    idx++;
                break;
            case 49:
                stateManager.SetCurrentState(stateManager.GetComponentInChildren<TutorialState>());
                explain.text = tutorialExplain.Explain[36];
                isQuestComplete = true;
                break;
            case 50:
                explain.text = tutorialExplain.Explain[37];
                isQuestComplete = true;
                break;
            case 51:
                explain.text = tutorialExplain.Explain[38];
                ButtonSetting(new Vector2(-680, -160), new Vector2(200, 200), DescendGod);
                break;
            case 52:
                tutorialButton.gameObject.SetActive(false);
                InitEmphasizeCircle();
                ActiveExplainPanel(true);
                explain.text = tutorialExplain.Explain[39];
                isQuestComplete = true;
                break;
            case 53:
                explain.text = tutorialExplain.Explain[40];
                isQuestComplete = true;
                break;
            case 54:
                explain.text = tutorialExplain.Explain[41];
                isQuestComplete = true;
                break;
            case 55:
                explain.text = tutorialExplain.Explain[42];
                isQuestComplete = true;
                break;
            case 56:
                explain.text = tutorialExplain.Explain[43];
                isQuestComplete = true;
                break;
            case 57:
                explain.text = tutorialExplain.Explain[44];
                isQuestComplete = true;
                break;
            case 58:
                explain.text = tutorialExplain.Explain[45];
                isQuestComplete = true;
                break;
            case 59:
                explain.text = tutorialExplain.Explain[46];
                isQuestComplete = true;
                break;
            case 60:
                explain.text = tutorialExplain.Explain[47];
                ButtonSetting(new Vector2(850, -430), new Vector2(100, 100), AscendGod);
                emphasizeCircle.gameObject.SetActive(true);
                EmphasizeCircle(new Vector2(850, -430), new Vector2(100, 100));
                break;
            case 61:
                break;
            case 62:
                ActiveExplainPanel(true);
                emphasizeCircle.gameObject.SetActive(false);
                explain.text = tutorialExplain.Explain[48];
                isQuestComplete = true;
                break;
            case 63:
                explain.text = tutorialExplain.Explain[49];
                isQuestComplete = true;
                break;
            case 64:
                explain.text = tutorialExplain.Explain[50];
                isQuestComplete = true;
                break;
            case 65:
                explain.text = tutorialExplain.Explain[51];
                isQuestComplete = true;
                break;
            case 66:
                emphasizeCircle.gameObject.SetActive(true);
                EmphasizeCircle(new Vector2(0, 480), new Vector2(100, 100));
                isQuestComplete = true;
                break;
            case 67:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                emphasizeCircle.gameObject.SetActive(false);
                explain.text = tutorialExplain.Explain[52];
                isQuestComplete = true;
                break;
            case 68:
                explain.text = tutorialExplain.Explain[53];
                isQuestComplete = true;
                break;
            case 69:
                Time.timeScale = 1f;
                ButtonInactive(true);
                ActiveExplainPanel(false);
                break;
            case 70:
                ActiveExplainPanel(true);
                explain.text = tutorialExplain.Explain[54];
                isQuestComplete = true;
                break;
            case 71:
                explain.text = tutorialExplain.Explain[55];
                isQuestComplete = true;
                break;
            case 72:
                explain.text = tutorialExplain.Explain[56];
                isQuestComplete = true;
                break;
            case 73:
                emphasizeCircle.gameObject.SetActive(true);
                EmphasizeCircle(new Vector2(-430, 0), new Vector2(500, 500));
                isQuestComplete = true;
                break;
            case 74:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[57];
                isQuestComplete = true;
                break;
            case 75:
                explain.text = tutorialExplain.Explain[58];
                ButtonSetting(new Vector2(-430, -320), new Vector2(100, 100), MoveToTutorialWorld);
                isQuestComplete = true;
                break;
            case 76: 
                EmphasizeCircle(new Vector2(-430, -320), new Vector2(100, 100));
                 
                 break;
            case 77:
                InitEmphasizeCircle();
                ActiveExplainPanel(true);
                tutorialButton.gameObject.SetActive(false);
                explain.text = tutorialExplain.Explain[59];
                isQuestComplete = true;
                break;
            case 78:
                explain.text = tutorialExplain.Explain[60];
                ButtonSetting(new Vector2(-940, 400), new Vector2(50, 50), WorldSceneListOnButton);
                isQuestComplete = true;
                break;
            case 79:
                EmphasizeCircle(new Vector2(-940, 400), new Vector2(50, 50));
                break;
            case 80:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                tutorialButton.gameObject.SetActive(false);
                explain.text = tutorialExplain.Explain[61];
                isQuestComplete = true;
                break;
            case 81:
                EmphasizeCircle(new Vector2(-700, 40), new Vector2(400, 400)); 
                isQuestComplete = true;
                break;
            case 82:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[62];
                isQuestComplete = true;
                break;
            case 83:
                EmphasizeCircle(new Vector2(-590, 325), new Vector2(50, 50));
                isQuestComplete = true;
                break;
            case 84:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[63];
                ButtonSetting(new Vector2(-690, 175), new Vector2(50, 50), TurnOnKnightStatus);
                isQuestComplete = true;
                break;
            case 85:
                EmphasizeCircle(new Vector2(-690, 175), new Vector2(50, 50));
                break;
            case 86:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                tutorialButton.gameObject.SetActive(false);
                explain.text = tutorialExplain.Explain[64];
                isQuestComplete = true;
                break;
            case 87:
                EmphasizeCircle(new Vector2(630, -165), new Vector2(100, 100));
                isQuestComplete = true;
                break;
            case 88:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[65];
                isQuestComplete = true;
                break;
            case 89:
                EmphasizeCircle(new Vector2(790, -155), new Vector2(100, 100));
                isQuestComplete = true;
                break;
            case 90:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[66];
                isQuestComplete = true;
                break;
            case 91:
                EmphasizeCircle(new Vector2(-695, -440), new Vector2(100, 100));
                isQuestComplete = true;
                break;
            case 92:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[67];
                isQuestComplete = true;
                break;
            case 93:
                EmphasizeCircle(new Vector2(450,482), new Vector2(100, 100));
                isQuestComplete = true;
                break;
            case 94:
                ActiveExplainPanel(true);
                InitEmphasizeCircle();
                explain.text = tutorialExplain.Explain[68];
                isQuestComplete = true;
                break;
            case 95:
                explain.text = tutorialExplain.Explain[69];
                isQuestComplete = true;
                break;
            case 96:
                explain.text = tutorialExplain.Explain[70];
                isQuestComplete = true;
                break;
            case 97:
                explain.text = tutorialExplain.Explain[71];
                isQuestComplete = true;
                break;
            case 98:
                EndTutorial();
                idx++;
                break;
            case 99:
                Destroy(gameObject);
                break;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    private void ClickCursor(Vector2 _cursorPos,Vector2 _cursorScale)
    {
        // 클릭
        Vector2 startCursorScale = _cursorScale + new Vector2(20f, 20f);
        
        cursorImage.rectTransform.anchoredPosition = _cursorPos;

        elapsed2 += Time.unscaledDeltaTime;

        if (elapsed2 >= 1)
        {
            elapsed2 = 0f;
        }
        Vector2 cursorScale = Vector2.Lerp(startCursorScale, _cursorScale, elapsed2 );
        cursorImage.rectTransform.sizeDelta = cursorScale;
        
    }

    private void EmphasizeCircle(Vector2 _circlePos,Vector2 _circleScale)
    {
        // 강조 원 생성
        Vector2 sizeUp = new Vector2(100f, 100f);
        Vector2 startCircleScale = _circleScale + sizeUp;
        ActiveExplainPanel(false);
        emphasizeCircle.rectTransform.anchoredPosition = _circlePos;
        elapsed += Time.unscaledDeltaTime;
        if (elapsed >= 1)
        {
            elapsed = 0f;
        }
        Vector2 emphasizeScale =  Vector2.Lerp(startCircleScale, _circleScale, elapsed2);

        emphasizeCircle.rectTransform.sizeDelta = emphasizeScale;
        
    }
    private void ActiveTutorialBattlePanel()
    {
        // 튜토리얼 패널 활성화
        isQuestComplete = true;
        tutorialBattleButtonImg.gameObject.SetActive(false);
        tutorialBattlePanel.gameObject.SetActive(true);
    }
    private void ButtonSetting(Vector2 _circlePos, Vector2 _circleScale, Action _callBack)
    {
        // 버튼 세팅
        tutorialButton.GetComponent<Image>().rectTransform.anchoredPosition = _circlePos;
        tutorialButton.GetComponent<Image>().rectTransform.sizeDelta = _circleScale;
        tutorialButton.gameObject.SetActive(true);
        tutorialButton.onClick.RemoveAllListeners();
        tutorialButton.onClick.AddListener(() => _callBack());  
    }
    private void BattleSceneListOnButton()
    {
        // 배틀씬 리스트 버튼 
        deploymentSceneUIManager.ListOnBtn();
        isQuestComplete = true;
    }
    private void WorldSceneListOnButton()
    {
        // 월드씬 리스트 버튼
        dollManagePanel.ToggleDrawer();
        isQuestComplete = true;
    }

    private void StartBattle()
    {
        // 배틀 스타트
        isQuestComplete = true;
        stateManager.SetCurrentState(stateManager.GetComponentInChildren<BattleStartState>());
    }
    private void BattleAscend()
    {
        // 배틀 올라가기
        stateManager.SetCurrentState(stateManager.GetComponentInChildren<BattleAscendState>());
    }

    public void DescendGod()
    {
        // 강림
        ActiveExplainPanel(false);
        Knight knight = selectManager.SelectedList[0].GetComponentInChildren<Knight>();
        descendKnight = knight;
        knight.ToggleControll();
        battleSceneCamera.CameraViewToggle(knight.GetCamHolder());
        stateManager.SetCurrentState(stateManager.GetComponentInChildren<BattleDescendState>());
    }
    public void AscendGod()
    {
        // 올라가기
        idx++;
        ActiveEmphasizeCircle(false);
        descendKnight.ToggleControll();
        battleSceneCamera.CameraViewToggle(descendKnight.GetCamHolder(), BattleAscend);
    }

    private void MoveToTutorialBattle()
    {
        // 튜토리얼 배틀로 이동
        isQuestComplete = true;
        emphasizeCircle.gameObject.SetActive(false);
        tutorialBattlePanel.gameObject.SetActive(false);
        ActiveExplainPanel(false);
        GameManager.Instance.LoadSceneWithName("Battle_Tutorial");

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            if (scene.name == "Battle_Tutorial")
            {
                worldTrigger = null;
                dollManagePanel = null;
                inactiveButtons.Clear();
                stateManager = GameObject.FindObjectOfType<StateManager>();
                selectManager = GameObject.FindObjectOfType<SelectManager>();
                deploymentSceneUIManager = GameObject.FindObjectOfType<DeploymentSceneUIManager>();
                battleSceneUIManager = GameObject.FindObjectOfType<BattleSceneUIManager>();
                battleSceneCamera = GameObject.FindObjectOfType<CameraController>();

                stateManager.SetCurrentState(stateManager.GetComponentInChildren<TutorialState>());
                inactiveButtons.AddRange(deploymentSceneUIManager.GetComponentsInChildren<Button>());
                inactiveButtons.AddRange(battleSceneUIManager.GetComponentsInChildren<Button>());

                ActiveExplainPanel(true);
                ButtonInactive(false);

            }
        };
    }
    private void MoveToTutorialWorld()
    {
        // 튜토리얼 월드로 이동
        ActiveExplainPanel(false);
        GameManager.Instance.LoadSceneWithName("WorldMap");
        allyKnightsManager.OnBattleEnd();
        idx++;
        stateManager = null;
        selectManager = null;
        deploymentSceneUIManager = null;
        battleSceneUIManager = null;
        battleSceneCamera = null;
        inactiveButtons.Clear();
        SceneManager.sceneLoaded += InactiveButtons;
    }

    private void InactiveButtons(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "WorldMap")
        {
            worldTrigger = GameObject.FindObjectOfType<WorldTrigger>();
            dollManagePanel = GameObject.FindObjectOfType<DollManagePanel>();

            inactiveButtons.AddRange(worldTrigger.GetComponentsInChildren<Button>());
            inactiveButtons.AddRange(dollManagePanel.GetComponentsInChildren<Button>());

            ButtonInactive(false);
        }
    }
    private void EndTutorial()
    {
        // 튜토리얼 끝내기
        GameManager.Instance.ClearMap(EMap.Tutorial);
        inactiveButtons.AddRange(worldTrigger.GetComponentsInChildren<Button>());
        inactiveButtons.AddRange(dollManagePanel.GetComponentsInChildren<Button>());
        
        ActiveExplainPanel(false);
        ButtonInactive(true);

        SceneManager.sceneLoaded -= InactiveButtons;
    }
    public void NextTutorial()
    {
        idx++;
    }
    private void TurnOnKnightStatus()
    {
        // 병사 정보 켜기
        List<KnightInformation> aliveKnightsInfo = allyKnightsManager.GetAliveKnightInformationList();
        dollManagePanel.TurnOnKnightStatus(aliveKnightsInfo[0]);
        isQuestComplete = true;
    }
    private void InitEmphasizeCircle()
    {
        // 강조 원 초기화
        emphasizeCircle.rectTransform.sizeDelta = Vector2.zero;
    }
    public void ActiveEmphasizeCircle(bool _bool)
    {
        // 강조 원 활성화 여부
        emphasizeCircle.gameObject.SetActive(_bool);
    }
    private void ActiveExplainPanel(bool _bool)
    {
        // 설명패널 활성화 여부
        explainPanel.gameObject.SetActive(_bool);   
    }
}
