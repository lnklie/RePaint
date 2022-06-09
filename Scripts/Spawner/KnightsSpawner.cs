using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : Inklie
 * 파일명 : JSJ.cs
==============================
*/
public enum Rank
{
    SS,
    S,
    A,
    B,
    C
}

public class KnightsSpawner : MonoBehaviour
{
    [Header("Customizing")]
    [SerializeField] 
    private KnightCustomizer customizer = null;
    [SerializeField] 
    private KnightCustomizer.HeadObject head = KnightCustomizer.HeadObject.Pilosity;
    [SerializeField]
    private KnightCustomizer.Weapon weapon = KnightCustomizer.Weapon.Longsword;
    [SerializeField] 
    private KnightCustomizer.Shield shield = KnightCustomizer.Shield.Shield_Type02;
    [SerializeField] 
    private KnightCustomizer.CoatOfArms coat = KnightCustomizer.CoatOfArms.CoatOfArms_type02;
    [SerializeField]
    private KnightCustomizer.KnightBase body = KnightCustomizer.KnightBase.Type00;


    private PlayerData playerData = null;
    private KnightName knighName = null;
    private string[] prefix = new string[100];
    private string[] suffix = new string[100];
    private Rank[] rank = new Rank[100];

    private List<string> usedName = new List<string>();

    [SerializeField]
    private GameObject knightPrefab = null;
    [SerializeField] 
    private Vector3 spawnRotation = Vector3.zero;
    [SerializeField] 
    private int team = 0;
    [SerializeField] 
    private bool isAlly = false;
    [SerializeField] 
    private Transform knightHolder = null;


    private void Awake()
    {
        knighName = this.GetComponent<KnightName>();
        Naming();
        Ranking();
    }
    
    private void Naming()
    {
        for (int i = 0; i < 100; i++)
        {
            prefix[i] = knighName.GetRandomPrefix();
        }
        for (int j = 0; j < 100; j++)
        {
            suffix[j] = knighName.GetRandomSuffix();
        }
    }

    private void Ranking()
    {
        for (int i = 0; i < 100; i++)
        {
            rank[i] = Rank.C;
        }
    }

    public void SpawnKnights(List<Knight> _knights, int _count)
    {
        if(_knights == null) _knights = new List<Knight>();        

        if (knightPrefab == null) return;

        for (int i = 0; i < _count; i++)
        {
            GameObject knightGo = Instantiate(knightPrefab);
            knightGo.transform.SetParent(knightHolder);
            Knight knight = knightGo.GetComponentInChildren<Knight>();
            knight.Team = team;
            _knights.Add(knight);

            KnightInformation knightInformation = knight.GetComponent<KnightInformation>();

            string name = knighName.GetRandomPrefix() + " " + knighName.GetRandomSuffix();
            while (usedName.Contains(name)) { 
                name = knighName.GetRandomPrefix() + " " + knighName.GetRandomSuffix();
            }

            usedName.Add(name);

            knightInformation.KnightName = name;
            knightInformation.KnightRank = rank[i];
            knightInformation.KnightType = EKnightType.Default;
            if (isAlly) // 귀
            {
                knightGo.tag = "Objects";
                knightGo.layer = 9;
            }
        }

        if (customizer != null)
        {
            customizer.SetHead((int)head);
            customizer.SetWeapon((int)weapon);
            customizer.SetShield((int)shield);
            customizer.SetCoat((int)coat);
            customizer.SetBody((int)body);

            customizer.CustomizeAllKnight(_knights, true);
        }
    }

    public void LoadAliveSpawnKnights(List<Knight> _knights)
    {
        if (_knights == null) _knights = new List<Knight>();

        if (knightPrefab == null) return;

        playerData = FindObjectOfType<SaveData>().playerData;
        Debug.Log(playerData.aliveKnightName.Count);

        for (int i = 0; i < playerData.aliveKnightName.Count; i++)
        {
            GameObject knightGo = Instantiate(knightPrefab);
            knightGo.transform.SetParent(knightHolder);
            Knight knight = knightGo.GetComponentInChildren<Knight>();
            knight.Team = team;
            _knights.Add(knight);

            KnightInformation knightInformation = knight.GetComponent<KnightInformation>();

            knightInformation.KnightName = playerData.aliveKnightName[i];
            usedName.Add(knightInformation.KnightName);
            knightInformation.KnightRank = playerData.aliveKnightRank[i];
            knightInformation.exp = playerData.aliveKnightExp[i];
            knightInformation.nextExp = playerData.aliveKnightNextExp[i];
            knightInformation.KnightType = EKnightType.Default;

            if (isAlly)
            {
                knightGo.tag = "Objects";
                knightGo.layer = 9;
            }
            Debug.Log(i);
        }
    }

    public void LoadDeadSpawnKnights(List<Knight> _knights)
    {
        if (_knights == null) _knights = new List<Knight>();

        if (knightPrefab == null) return;

        for (int i = 0; i < playerData.deadKnightName.Count; i++)
        {
            GameObject knightGo = Instantiate(knightPrefab);
            knightGo.transform.SetParent(knightHolder);
            Knight knight = knightGo.GetComponentInChildren<Knight>();
            knight.Team = team;
            _knights.Add(knight);

            KnightInformation knightInformation = knight.GetComponent<KnightInformation>();

            knightInformation.KnightName = playerData.deadKnightName[i];
            usedName.Add(knightInformation.KnightName);
            knightInformation.KnightRank = playerData.deadKnightRank[i];
            knightInformation.exp = playerData.deadKnightExp[i];
            knightInformation.nextExp = playerData.deadKnightNextExp[i];
            knightInformation.KnightType = EKnightType.Default;

            if (isAlly)
            {
                knightGo.tag = "Objects";
                knightGo.layer = 9;
            }
        }
    }
}
