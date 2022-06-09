using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
==============================
 * ���������� : 2022-06-06
 * �ۼ��� : Inklie
 * ���ϸ� : JSJ.cs
==============================
*/
public class ResultPanelUI : MonoBehaviour
{
    private float deadProbability = 30f;

    [SerializeField]
    private Text injureKightsText = null;
    [SerializeField]
    private Text deadKightsText = null;
    [SerializeField] 
    private Text timeText = null;
    [SerializeField]
    private Text rewardText = null;

    public void SetDeadKinghtsText(int _injured, int _dead)
    {
        injureKightsText.text = _injured.ToString();
        deadKightsText.text = _dead.ToString();
    }

    public void SetTimeText(int _sec)
    {
        int min = _sec / 60;
        int sec = _sec % 60;

        string value = min + "m " + sec + "s";
        timeText.text = value;
    }

    public void SetRewardText(int _value)
    {
        rewardText.text = "x " + _value;
    }

}
