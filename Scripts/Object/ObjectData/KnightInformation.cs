using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : JSJ
 * 파일명 : KnightInformation.cs
==============================
*/
public class KnightInformation : MonoBehaviour
{
    private Rank knightRank = Rank.SS;
    public Rank KnightRank
    {
        get { return knightRank; }
        set { knightRank = value; }
    }

    private EKnightType knightType = EKnightType.Default;
    public EKnightType KnightType
    {
        get { return knightType; }
        set { knightType = value; }
    }
    
    public string KnightName
    {
        get { return knightName; }
        set { knightName = value; }
    }
    public int exp = 0;
    public int nextExp = 20;

    [SerializeField]
    private string knightName = null;

    public bool Upgrade()
    {
        exp += 10;

        if(exp >= nextExp)
        {
            RankUp();
            exp = 0;
            nextExp *= 2;
            return true;
        }

        return false;
    }

    public void RankUp()
    {
        if (knightRank == Rank.SS) return;
        knightRank--;
    }

    public float GetExpRatio()
    {
        if (knightRank == Rank.SS) return 1f;

        return exp / (float)nextExp;
    }
    
}
