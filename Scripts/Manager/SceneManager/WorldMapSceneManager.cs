using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : JSJ
 * 파일명 : WorldMapSceneManager.cs
==============================
*/
public class WorldMapSceneManager : MonoBehaviour
{
    private GameManager gm = null;
    private GameObject tutorialPrefab = null;

    [SerializeField] 
    private WorldTrigger worldTrigger = null;
    [SerializeField] 
    private GameObject tutorialManager = null;

    private void Awake()
    {
        gm = GameManager.Instance;
    }
    private void Start()
    {
        if (!GameManager.Instance.loadGame)
            tutorialPrefab = Instantiate(tutorialManager);
    }
    public void LoadBattleScene()
    {
        string sceneName = "Battle_" + worldTrigger.BattleField;
        gm.LoadSceneWithName(sceneName);
    }
}
