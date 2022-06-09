using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : Inklie
 * 파일명 : JSJ.cs
==============================
*/
public class DollUpgradePanel : MonoBehaviour
{
    private KnightInformation currentKnight = null;
    private AllyKnightsManager allyKnightsManager = null;
    private DollManagePanel dollManagePanel = null;

    [SerializeField]
    private Text nameText = null;
    [SerializeField] 
    private Text rankText = null;
    [SerializeField] 
    private Button upgradeBtn = null;
    [SerializeField] 
    private Button revivalBtn = null;
    [SerializeField] 
    private Text upgradeText = null;
    [SerializeField] 
    private Text revivalText = null;
    [SerializeField] 
    private Slider expSlider = null;

    
    private void Start()
    {
        ClosePanel();

        allyKnightsManager = GameObject.FindObjectOfType<AllyKnightsManager>();
        dollManagePanel = GetComponentInParent<DollManagePanel>();
    }

    public void SetDollInformation(KnightInformation _info)
    {
        currentKnight = _info;
        SetDollName(_info.KnightName);
        SetDollRank(_info.KnightRank);
        SetDollExp(_info.GetExpRatio());
    }

    public bool IsEqaul(KnightInformation _info)
    {
        return currentKnight == _info;
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void OpenPanel(bool _isDead = false)
    {
        upgradeBtn.interactable = !_isDead;
        revivalBtn.interactable = _isDead;

        upgradeText.color = _isDead ? Color.gray : Color.white;
        revivalText.color = _isDead ? Color.white : Color.gray;
        gameObject.SetActive(true);
    }

    public void RevivalDoll()
    {
        if (GameManager.soul < 1) return;

        allyKnightsManager.RevivalKnight(currentKnight);
        dollManagePanel.DeactivateAllNameTag();
        dollManagePanel.ActivateDeadNameTags();

        OpenPanel();

        GameManager.soul--;
        dollManagePanel.UpdateSoulText();
    }

    public void RankUp()
    {
        if (currentKnight.KnightRank == Rank.SS) return;

        if (GameManager.soul < 1) return;

        bool isRankUp = currentKnight.Upgrade();

        if (isRankUp)
            SetDollRank(currentKnight.KnightRank);

        SetDollExp(currentKnight.GetExpRatio());

        GameManager.soul--;
        dollManagePanel.UpdateSoulText();
    }


    private void SetDollName(string _name)
    {
        nameText.text = _name;
    }

    private void SetDollRank(Rank _rank)
    {
        rankText.text = _rank.ToString();
    }

    private void SetDollExp(float _exp)
    {
        expSlider.value = _exp;
    }
}
