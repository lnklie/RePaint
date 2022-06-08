using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : KJY
 * 파일명 : SaveData.cs
==============================
*/
public class SaveData : MonoBehaviour
{
    private GameManager gm = null;
    private int dataNum = default;

    [Header("SaveData")]
    [Space]
    public PlayerData playerData = null;
    [SerializeField]
    private Text[] saveSlotText = null;
    [SerializeField] 
    private Text[] loadSlotText = null;
    [SerializeField] 
    private GameObject saveAccept = null;

    public List<KnightInformation> aliveKnightInformation = new List<KnightInformation>();
    public List<KnightInformation> deadKnightInformation = new List<KnightInformation>();

    private void Awake()
    {
        gm = GameManager.Instance;
    }

    private void Start()
    {
        transform.SetParent(gm.transform);
        SetSlotName();
    }

    public void SlotSelect(int _slotNum)
    {
        dataNum = _slotNum;
    }

    public void DateToSlot()
    {
        saveSlotText[dataNum].text = DateTime.Now.ToString();
    }

    public void BackToMain()
    {
        gm.LoadSceneWithName("Intro_Scene");
    }

    private void SetSlotName()
    {
        for (int i = 0; i < loadSlotText.Length; i++)
        {
            loadSlotText[i].text = PlayerPrefs.GetString("slotName" + i, "Slot" + ( i + 1));
        }

        for (int i = 0; i < saveSlotText.Length; i++)
        {
            saveSlotText[i].text = PlayerPrefs.GetString("slotName" + i, "Slot" + (i + 1));
        }
    }

    private void GetAliveKnightInformation()
    {
        aliveKnightInformation = gm.GetAliveKnightInformationList();

        for (int i = 0; i < aliveKnightInformation.Count; i++)
        {
            playerData.aliveKnightName.Add(aliveKnightInformation[i].KnightName);
            playerData.aliveKnightRank.Add(aliveKnightInformation[i].KnightRank);
            playerData.aliveKnightExp.Add(aliveKnightInformation[i].exp);
            playerData.aliveKnightNextExp.Add(aliveKnightInformation[i].nextExp);
        }
    }

    private void GetDeadKnightInformation()
    {
        deadKnightInformation = gm.GetDeadKnightInformationList();
        for (int i = 0; i < deadKnightInformation.Count; i++)
        {
            playerData.deadKnightName.Add(deadKnightInformation[i].KnightName);
            playerData.deadKnightRank.Add(deadKnightInformation[i].KnightRank);
            playerData.deadKnightExp.Add(deadKnightInformation[i].exp);
            playerData.deadKnightNextExp.Add(deadKnightInformation[i].nextExp);
        }
    }

    public void SaveAccept()
    {
        DateToSlot();

        playerData = new PlayerData();

        playerData.mainCamRigPosition = Camera.main.transform.parent.position;
        playerData.mainCamRigRotation = Camera.main.transform.parent.rotation;

        playerData.mainCamPosition = Camera.main.transform.localPosition;
        playerData.mainCamRotation = Camera.main.transform.localRotation;
        playerData.progress = gm.GetClearMapFlag();
        playerData.soul = GameManager.soul;

        for (int i = 0; saveSlotText.Length > i; i++)
        {
            PlayerPrefs.SetString("slotName" + i.ToString(), saveSlotText[i].text);
        }

        GetAliveKnightInformation();
        GetDeadKnightInformation();

        string jsonData = JsonUtility.ToJson(playerData, true);
        string path = Path.Combine(Application.dataPath, "SaveData"+ dataNum.ToString() + ".json");
        File.WriteAllText(path, jsonData);

        saveAccept.SetActive(true);
    }

    public void LoadAccept()
    {
        gm.loadGame = true;
        StartCoroutine(AfterTheScene());
    }

    private IEnumerator AfterTheScene()
    {
        gm.LoadSceneWithName("WorldMap");
        yield return new WaitForSeconds(2.1f);
        yield return new WaitUntil(() => gm.asyncScene.isDone == true);

        playerData = new PlayerData();

        string path = Path.Combine(Application.dataPath, "SaveData" + dataNum.ToString() + ".json");
        string jsonData = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);

        for(int i = 0; saveSlotText.Length > i; i++)
        {
            saveSlotText[i].text = PlayerPrefs.GetString("slotName"+i.ToString());
        }
        PlayerPrefs.Save();

        AllyKnightsManager allyKnightsManager = FindObjectOfType<AllyKnightsManager>();
        allyKnightsManager.LoadKnights();
        DollManagePanel dollManagePanel = FindObjectOfType<DollManagePanel>();
        gm.ClearMap((EMap)playerData.progress);
        GameManager.soul = playerData.soul;
        dollManagePanel.Initailize();

        Camera.main.transform.parent.position = playerData.mainCamRigPosition;
        Camera.main.transform.parent.rotation = playerData.mainCamRigRotation;
        Camera.main.transform.localPosition = playerData.mainCamPosition;
        Camera.main.transform.localRotation = playerData.mainCamRotation;
    }
}

[Serializable]
public class PlayerData 
{
    public Vector3 mainCamPosition = default;
    public Quaternion mainCamRotation = default;
    public Vector3 mainCamRigPosition = default;
    public Quaternion mainCamRigRotation = default;
    public byte progress = default;
    public int soul = 0;

    public List<string> aliveKnightName = new List<string>();
    public List<string> deadKnightName = new List<string>();
    public List<Rank> aliveKnightRank = new List<Rank>();
    public List<Rank> deadKnightRank = new List<Rank>();
    public List<int> aliveKnightExp = new List<int>();
    public List<int> deadKnightExp = new List<int>();
    public List<int> aliveKnightNextExp = new List<int>();
    public List<int> deadKnightNextExp = new List<int>();
}



