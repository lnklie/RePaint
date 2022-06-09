using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : JSJ
 * 파일명 : GameManager.cs
==============================
*/
public class GameManager : SingleTonManager<GameManager>
{
    private byte clearMap = 64;
    private bool completeTutorial = false;
    private GameObject loadingUI = null;
    private Text loadingText = null;
    private SoundManager soundManager = new SoundManager();
    private AllyKnightsManager allyKnightsManager = null;

    public AsyncOperation asyncScene = null;
    public bool loadGame = false;
    public static int soul = 0;
    public static SoundManager SoundManager { get { return Instance.soundManager; } }
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        loadingUI = Resources.Load<GameObject>("Prefabs\\Loading");
        loadingText = loadingUI.GetComponentInChildren<Text>();
        loadingUI = Instantiate(loadingUI, transform);
        loadingUI.SetActive(false);

        SoundManager.GenerateSound();
    }

    private void Start()
    {
        SoundManager.SoundPlay("Intro_SceneBGM",Sound.BACKGROUND);
        allyKnightsManager = GetComponentInChildren<AllyKnightsManager>();
        SceneManager.sceneLoaded += SetActiveAllyKnights;
    }

    public bool GetCompleteTutorial()
    {
        return completeTutorial;
    }
    public void SetCompleteTutorial(bool _bool)
    {
        completeTutorial = _bool;
    }

    public static void SoundClear()
    {
        SoundManager.Clear();
    }

    public static void SoundStop(Sound _sound)
    {
        SoundManager.SoundStop(_sound);
    }

    public void LoadSceneWithName(string _sceneName)
    {
        SoundClear();
        loadingUI.SetActive(true);
        StartCoroutine("LoadSceneCoroutine", _sceneName);
    }

    private IEnumerator LoadSceneCoroutine(string _sceneName)
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        yield return wait;
        asyncScene = SceneManager.LoadSceneAsync(_sceneName);
        yield return asyncScene;

        string bgm = _sceneName + "BGM";
        if (_sceneName == "WorldMap")
        {
            SoundManager.SoundPlay(Sound.WORLDMAP);
        }
        else
        {
            SoundManager.SoundPlay(bgm, Sound.BACKGROUND);
        }
        loadingUI.SetActive(false);
        yield return null;

    }

    public byte GetClearMapFlag()
    {
        return clearMap;
    }

    public void ClearMap(EMap _map)
    {
        clearMap |= (byte)_map; 
    }
    public bool IsClear(EMap _map)
    {
        return ((clearMap & (byte)_map) > 0);
    }
 
    public List<KnightInformation> GetAliveKnightInformationList()
    {
        if(!allyKnightsManager)
            allyKnightsManager = GetComponentInChildren<AllyKnightsManager>();

        List<KnightInformation> infos = allyKnightsManager.GetAliveKnightInformationList();
        return infos;
    }

    public List<KnightInformation> GetDeadKnightInformationList()
    {
        if (!allyKnightsManager)
            allyKnightsManager = GetComponentInChildren<AllyKnightsManager>();

        List<KnightInformation> infos = allyKnightsManager.GetDeadKnightInformationList();
        return infos;
    }

    private void SetActiveAllyKnights(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.StartsWith("Battle")) return;

        if (!allyKnightsManager)
            allyKnightsManager = GetComponentInChildren<AllyKnightsManager>();

        allyKnightsManager.OnBattleScene();
        SoundStop(Sound.WORLDMAP);
    }
}
