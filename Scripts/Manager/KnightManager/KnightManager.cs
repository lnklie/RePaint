using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
==============================
 * ���������� : 2022-06-06
 * �ۼ��� : JSJ
 * ���ϸ� : KnightManager.cs
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
        // ������ ���õ��� �ʾҴ��� Ȯ��
        return (defaultKnight.Count <= 0);
    }

    public int KnightCount()
    {
        // ���� ��
        return knights.Count;
    }

    public void StartBattle()
    {
        // ��Ʋ ����
        foreach (Knight knight in knights)
        {
            knight.StartBattle();
        }
    }

    public Knight[] GetKnightAll()
    {
        // ��� ��� �迭 ��ȯ
        return knights.ToArray();
    }


    public EKnightType GetKnightType(int _index)
    {
        // ��� Ÿ�� ��ȯ
        return knights[_index].GetComponent<KnightInformation>().KnightType;
    }

    public KnightInformation GetKnight(int _index, int _type)
    {
        // ��� ��ȯ
        return GetKnightInformationListWithType(_type)[_index];
    }

    public KnightInformation GetKnight(int _index)
    {
        // ���õ��� ���� ��� ��ȯ
        return defaultKnight[_index];
    }

    public List<KnightInformation> GetKnightInformationListWithType(int _type)
    {
        // ������ ���� ����Ʈ ��ȯ
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
        // ���� ����
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
        // ��� ���� �ʱ�ȭ
        defaultKnight.Clear();
        swordKnight.Clear();
        spearKnight.Clear();
        bowKnight.Clear();

        for (int i = 0; i < knights.Count; i++)
            defaultKnight.Add(knights[i].GetComponent<KnightInformation>());
    }
}
