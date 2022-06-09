using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : JSJ
 * 파일명 : AllyKnightsManager.cs
==============================
*/
public class AllyKnightsManager : KnightManager
{

    [SerializeField] 
    private List<Knight> deadKnights = new List<Knight>();
    [SerializeField]
    private GameObject knightHolder = null;

    public static AllyKnightsManager instance = null;
    protected override void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        base.Awake();
    }

    protected override void Start()
    {
        if (GameManager.Instance.loadGame == false)
        {
            base.Start();
        }
       
        transform.SetParent(GameManager.Instance.transform);
        knightHolder.SetActive(false);
    }

    public void LoadKnights()
    {
        knightsSpawner.LoadAliveSpawnKnights(knights);
        knightsSpawner.LoadDeadSpawnKnights(deadKnights);
        SetDefalutTypeAll();
    }

    public void OnBattleScene()
    {
        selectManager = GameObject.FindObjectOfType<SelectManager>();

        knightHolder.SetActive(true);
        foreach (Knight knight in knights)
        {
            knight.transform.parent.gameObject.SetActive(true);
            knightCustomizer.ReturnToDoll(knight);
        }

        knightCustomizer.OnBattleScene();
    }

    public void StopAllKnights()
    {
        foreach (Knight knight in knights)
            knight.EndBattle();
    }

    public void OnBattleEnd()
    {
        StopAllKnights();
        knightHolder.SetActive(false);
        SetDefalutTypeAll();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReDeploymentKnights()
    {
        int width = Mathf.CeilToInt(Mathf.Sqrt(knights.Count));

        RaycastHit hit;
        Terrain terrain = null;

        Vector3 screenCenter = new Vector3(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Terrain")))
        {
            terrain = hit.collider.GetComponent<Terrain>();
            transform.position = hit.point;
        }

        foreach (Knight knight in deadKnights)
            knight.transform.parent.gameObject.SetActive(false);

        int idx = 0;
        int knightCount = knights.Count;
        for (int z = 0; z < width; z++)
            for (int x = 0; x < width; x++)
            {
                knights[idx].transform.parent.localPosition = new Vector3(x, 0f, z);

                float y = terrain.SampleHeight(knights[idx].transform.position);
                knights[idx].transform.parent.position = new Vector3(knights[idx].transform.position.x, y, knights[idx].transform.position.z);
                knights[idx].transform.parent.rotation = Quaternion.identity;
                idx++;
                if (idx >= knightCount) return;
            }

    }

    public void DeadKnights(List<Knight> _deadKnights)
    {
        for (int i = 0; i < _deadKnights.Count; i++)
        {
            if (knights.Contains(_deadKnights[i]))
            {
                knights.Remove(_deadKnights[i]);
                deadKnights.Add(_deadKnights[i]);
            }
        }
    }

    public List<KnightInformation> GetAliveKnightInformationList()
    {
        List<KnightInformation> infos = new List<KnightInformation>();
        foreach (Knight knight in knights)
            infos.Add(knight.GetComponent<KnightInformation>());

        return infos;
    }

    public List<KnightInformation> GetDeadKnightInformationList()
    {
        List<KnightInformation> infos = new List<KnightInformation>();
        foreach (Knight knight in deadKnights)
            infos.Add(knight.GetComponent<KnightInformation>());

        return infos;
    }

    public void AddDoll()
    {
        knightsSpawner.SpawnKnights(knights, 1);
        defaultKnight.Add(knights[knights.Count - 1].GetComponent<KnightInformation>());
    }

    public bool IsDead(Knight _knight)
    {
        return deadKnights.Contains(_knight);
    }

    public void RevivalKnight(KnightInformation _knight)
    {
        Knight knight = _knight.GetComponentInChildren<Knight>();

        if (!(deadKnights.Contains(knight))) return;

        deadKnights.Remove(knight);
        knights.Add(knight);
        defaultKnight.Add(_knight);
    }
}
