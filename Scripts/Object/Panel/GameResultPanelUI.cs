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
public class GameResultPanelUI : MonoBehaviour
{
    [SerializeField]
    private GameObject victoryPanel = null;
    [SerializeField] 
    private GameObject defeatPanel = null;
    [SerializeField] 
    private ResultPanelUI victoryResultPanel = null;
    [SerializeField] 
    private MVPPanelUI victoryMVPPanel = null;
    [SerializeField] 
    private ResultPanelUI defeatResultPanel = null;
    [SerializeField] 
    private MVPPanelUI defeatMVPPanel = null;

    public void SetVictoryResult(int _injured, int _dead, int _sec, int _reward, Knight _mvp)
    {
        victoryResultPanel.SetDeadKinghtsText(_injured, _dead);
        victoryResultPanel.SetTimeText(_sec);
        victoryResultPanel.SetRewardText(_reward);

        victoryMVPPanel.SetKillCountText(_mvp.KillCount);
        victoryMVPPanel.SetMVPNameText(_mvp.Information.KnightName);
    }

    public void SetDefeatResult(int _injured, int _dead, int _sec, int _reward, Knight _mvp)
    {
        defeatResultPanel.SetDeadKinghtsText(_injured, _dead);
        defeatResultPanel.SetTimeText(_sec);
        defeatResultPanel.SetRewardText(_reward);
        
        defeatMVPPanel.SetMVPNameText(_mvp.Information.KnightName);
    }

    public void ShowVictoryPanel()
    {
        victoryPanel.SetActive(true);
    }

    public void ShowDefeatPanel()
    {
        defeatPanel.SetActive(true);
    }
}
