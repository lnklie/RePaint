using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : JSJ
 * 파일명 : KnightManager.cs
==============================
*/
public class KnightManager : SingleTonManager<KnightManager>
{
    protected KnightsSpawner knightsSpawner = null;
    protected KnightCustomizer knightCustomizer = null;

    [SerializeField] 
    protected SelectManager selectManager = null;
    [SerializeField] 
    protected List<Knight> knights = new List<Knight>();
    [SerializeField] 
    protected List<KnightInformation> defaultKnight = new List<KnightInformation>();
    [SerializeField] 
    protected List<KnightInformation> swordKnight = new List<KnightInformation>();
    [SerializeField] 
    protected List<KnightInformation> spearKnight = new List<KnightInformation>();
    [SerializeField] 
    protected List<KnightInformation> bowKnight = new List<KnightInformation>();


    protected virtual void Awake()
    {
        knightsSpawner = GetComponent<KnightsSpawner>();
        knightCustomizer = GetComponent<KnightCustomizer>();
    }

    protected virtual void Start()
    {
        if (GameManager.Instance.loadGame == false)
        {
            knightsSpawner.SpawnKnights(knights, 50);
            SetDefalutTypeAll();
        }
    }

    public bool IsReady()
    {
        // 병종이 선택되지 않았는지 확인
        return (defaultKnight.Count <= 0);
    }

    public int KnightCount()
    {
        // 기사들 수
        return knights.Count;
    }

    public void StartBattle()
    {
        // 배틀 시작
        foreach (Knight knight in knights)
        {
            knight.StartBattle();
        }
    }

    public Knight[] GetKnightAll()
    {
        // 모든 기사 배열 반환
        return knights.ToArray();
    }


    public EKnightType GetKnightType(int _index)
    {
        // 기사 타입 반환
        return knights[_index].GetComponent<KnightInformation>().KnightType;
    }

    public KnightInformation GetKnight(int _index, int _type)
    {
        // 기사 반환
        return GetKnightInformationListWithType(_type)[_index];
    }

    public KnightInformation GetKnight(int _index)
    {
        // 선택되지 않은 기사 반환
        return defaultKnight[_index];
    }

    public List<KnightInformation> GetKnightInformationListWithType(int _type)
    {
        // 정렬후 병사 리스트 반환
        List<KnightInformation> knightList = new List<KnightInformation>();
        if (_type == 0)
        {
            knightList = defaultKnight.OrderBy(x => x.KnightRank).ToList();
            return knightList;
        }
        else if (_type == 1)
        {
            knightList = swordKnight.OrderBy(x => x.KnightRank).ToList();
            return knightList;
        }
        else if (_type == 2)
        {
            knightList = spearKnight.OrderBy(x => x.KnightRank).ToList();
            return knightList;
        }
        else
        {
            knightList = bowKnight.OrderBy(x => x.KnightRank).ToList();
            return knightList;
        }
    }


    public void InputKnight(int _type)
    {
        // 병사 선택
        for(int i = 0; i< selectManager.SelectedList.Count; i++)
        {
            KnightInformation knightInformation = selectManager.SelectedList[i].GetComponentInChildren<KnightInformation>();
            if(_type == (int)EKnightType.Sword)
            {
                swordKnight.Remove(knightInformation);
                defaultKnight.Remove(knightInformation);
                spearKnight.Remove(knightInformation);
                bowKnight.Remove(knightInformation);
                swordKnight.Add(knightInformation);
            }
            else if(_type == (int)EKnightType.Spear)
            {
                spearKnight.Remove(knightInformation);
                defaultKnight.Remove(knightInformation);
                swordKnight.Remove(knightInformation);
                bowKnight.Remove(knightInformation);
                spearKnight.Add(knightInformation);
            }
            else if(_type == (int)EKnightType.Bow)
            {
                bowKnight.Remove(knightInformation);
                defaultKnight.Remove(knightInformation);
                swordKnight.Remove(knightInformation);
                spearKnight.Remove(knightInformation);
                bowKnight.Add(knightInformation);
            }
        }
        selectManager.ListInitialization();
    }

    protected void SetDefalutTypeAll()
    {
        // 모든 기사들 초기화
        defaultKnight.Clear();
        swordKnight.Clear();
        spearKnight.Clear();
        bowKnight.Clear();

        for (int i = 0; i < knights.Count; i++)
            defaultKnight.Add(knights[i].GetComponent<KnightInformation>());
    }
}
