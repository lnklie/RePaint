using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : JSJ
 * 파일명 : BattleSceneManager.cs
==============================
*/
public class BattleSceneManager : MonoBehaviour
{
    private bool DeployPhase = true;
    private int totalAlly = 0;
    private int totalEnemy = 0;
    private float battleTime = 0f;
    private float deadProbability = 30f;

    [SerializeField] 
    private EMap map = EMap.Valley;
    [SerializeField] 
    private int reward = 15;

    [SerializeField] 
    private AllyKnightsManager allyKnightManager = null;
    [SerializeField]
    private EnemyKnightsManager enemyKnightManager = null;
    [SerializeField] 
    private DeploymentSceneUIManager deployUIManager = null;
    [SerializeField] 
    private BattleSceneUIManager battleUIManager = null;
    [SerializeField] 
    private CameraController mainCam = null;
    [SerializeField] 
    private GameResultPanelUI gameResultPanel = null;

    [SerializeField] 
    private TutorialSceneManager tutorialManager = null;

    [Space]
    [Header("Serialize for Debug")]
    [SerializeField]
    private List<Knight> aliveAllyKnights = null;
    [SerializeField]
    private List<Knight> aliveEnemyKnights = null;
    [SerializeField]
    private List<Knight> deadAllyKnights = null;
    [SerializeField]
    private List<Knight> injuredAllyKnights = null;
    [SerializeField]
    private Knight mvp = null;


    private void Awake()
    {
        aliveAllyKnights = new List<Knight>();
        aliveEnemyKnights = new List<Knight>();
        deadAllyKnights = new List<Knight>();
        injuredAllyKnights = new List<Knight>();
        if(!enemyKnightManager)
            enemyKnightManager = GameObject.Find("Enemy").GetComponent<EnemyKnightsManager>();

        if(!GameManager.Instance.GetCompleteTutorial())
            tutorialManager= GameObject.FindObjectOfType<TutorialSceneManager>();
    }

    private void Start()
    {
        allyKnightManager = GameObject.FindObjectOfType<AllyKnightsManager>();
        allyKnightManager.ReDeploymentKnights();

        mainCam = Camera.main.GetComponent<CameraController>();
    }


    private void Update()
    {
        if (!DeployPhase)
            battleTime += Time.deltaTime;
    }

    public void StartBattle()
    {
        StartCoroutine(SetKnightDieCallBack());
        
        GameManager.SoundClear();
        GameManager.SoundManager.SoundPlay("Battle_Start" + Random.Range(1, 6), Sound.BACKGROUND);
        StartCoroutine(CoroutineEfectGround());
        enemyKnightManager.gameObject.SetActive(true);

        allyKnightManager.StartBattle();
        enemyKnightManager.StartBattle();

        deployUIManager.gameObject.SetActive(false);
        battleUIManager.ActiveInfluenceHolder();

        DeployPhase = false;
    }

    public void EndBattle()
    {
        allyKnightManager.OnBattleEnd();
        GameManager.Instance.LoadSceneWithName("WorldMap");
    }

    private void Victory()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (!GameManager.Instance.GetCompleteTutorial())
            tutorialManager?.NextTutorial();

        GameManager.SoundStop(Sound.EFFECTGROUND);

        mainCam.transform.SetParent(transform);
        DeployPhase = true;
        mvp = FindMVPKnight();
        DisableAllWithoutMVP(mvp);

        gameResultPanel.SetVictoryResult(injuredAllyKnights.Count, deadAllyKnights.Count, (int)battleTime, reward, mvp);

        mainCam.CloseUpToMVP(mvp.transform.parent,
            gameResultPanel.ShowVictoryPanel
        );

        GameManager.Instance.ClearMap(map);
        GameManager.soul += reward;
        allyKnightManager.DeadKnights(deadAllyKnights);
    }

    private void Defeat()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DeployPhase = true;
        enemyKnightManager.gameObject.SetActive(false);
        GameManager.SoundStop(Sound.EFFECTGROUND);
        gameResultPanel.SetDefeatResult(injuredAllyKnights.Count, deadAllyKnights.Count, (int)battleTime, reward, mvp);

        mainCam.CloseUpToMVP(mvp.transform.parent,
            gameResultPanel.ShowDefeatPanel, false
        );
        allyKnightManager.DeadKnights(deadAllyKnights);
    }

    private Knight FindMVPKnight()
    {
        if (aliveAllyKnights.Count <= 0) return null;

        Knight mvp = aliveAllyKnights[0];
        int killCount = mvp.KillCount;
        for (int i = 1; i < aliveAllyKnights.Count; i++)
        {
            if (aliveAllyKnights[i].KillCount > killCount)
            {
                mvp = aliveAllyKnights[i];
                killCount = mvp.KillCount;
            }
        }

        return mvp;
    }

    private void DisableAllWithoutMVP(Knight _mvp)
    {
        foreach (Knight knight in aliveAllyKnights)
        {
            if (knight == _mvp) continue;
            knight.transform.parent.gameObject.SetActive(false);
        }
    }
    private IEnumerator CoroutineEfectGround()
    {
        GameManager.SoundManager.SoundPlay("WarHorn", Sound.EFFECT);
        yield return new WaitForSecondsRealtime(12f);
        GameManager.SoundManager.SoundPlay("BattleSoundEFG", Sound.EFFECTGROUND);

    }
    private IEnumerator SetKnightDieCallBack()
    {
        Knight[] allyKnightList = allyKnightManager.GetKnightAll();
        totalAlly = allyKnightList.Length;
        foreach (Knight knight in allyKnightList)
        {
            aliveAllyKnights.Add(knight);

            knight.onDie += () =>
            {
                float rand = Random.Range(0f, 100f);
                aliveAllyKnights.Remove(knight);
                if (rand <= deadProbability)
                    deadAllyKnights.Add(knight);
                else
                    injuredAllyKnights.Add(knight);

                if (aliveAllyKnights.Count == 0)
                {
                    mvp = knight;
                    Defeat();
                }
                battleUIManager.UpdateAllyCountUI(aliveAllyKnights.Count, totalAlly);
            };
            yield return null;
        }

        Knight[] enemyKnightList = enemyKnightManager.GetKnightAll();
        totalEnemy = enemyKnightList.Length;
        foreach (Knight knight in enemyKnightManager.GetKnightAll())
        {
            aliveEnemyKnights.Add(knight);

            knight.onDie += () =>
            {
                aliveEnemyKnights.Remove(knight);
                battleUIManager.UpdateEnemyCountUI(aliveEnemyKnights.Count, totalEnemy);
                if (aliveEnemyKnights.Count == 0) Victory();
            };
        }
        yield return null;
    }
}
    
