using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : KJY
 * 파일명 : SingleTonManager.cs
==============================
*/
public class WorldTrigger : MonoBehaviour
{
    [Header("AnemyText")]
    [SerializeField]
    private Text swordText = null;

    private string battleField = string.Empty;
    public string BattleField { get { return battleField; } }

    [SerializeField] 
    private Text spearText = null;
    [SerializeField]
    private Text bowText = null;
    [SerializeField]
    private Button[] battleBtns = null;
    [SerializeField]
    private GameObject battlePanel = null;

    private void Start()
    {
        battlePanel.SetActive(false);

        byte clearMapFlag = GameManager.Instance.GetClearMapFlag();

        for(int i =0; i < battleBtns.Length; i++)
        {
            if((clearMapFlag & (byte)Mathf.Pow(2, i)) > 0)
                battleBtns[i].gameObject.SetActive(false);
            else
                battleBtns[i].gameObject.SetActive(true);
        }
    }

    public void BattleBtn(string _battlefiel­d)
    {
        battleField = _battlefield;

        if(_battlefiel­d == "Valley")
        {
            swordText.text = "12";
            spearText.text = "8";
            bowText.text = "17";
        }
        else if (_battlefiel­d == "Desert")
        {
            swordText.text = "48";
            spearText.text = "12";
            bowText.text = "12";
        }
        else if (_battlefiel­d == "Castle")
        {
            swordText.text = "35";
            spearText.text = "31";
            bowText.text = "34";
        }
        else if (_battlefiel­d == "Graveyard")
        {
            swordText.text = "42";
            spearText.text = "24";
            bowText.text = "14";
        }
        else if (_battlefiel­d == "Ridge")
        {
            swordText.text = "16";
            spearText.text = "40";
            bowText.text = "24";
        }
        else if (_battlefiel­d == "Forest")
        {
            swordText.text = "26";
            spearText.text = "25";
            bowText.text = "21";
        }

        battlePanel.SetActive(true);
    }

    public void BattlePanelOffBtn()
    {
        battlePanel.SetActive(false);
    }
}
