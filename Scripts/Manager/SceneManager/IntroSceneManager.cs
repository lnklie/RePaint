using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
==============================
 * ���������� : 2022-06-06
 * �ۼ��� : JSJ
 * ���ϸ� : IntroSceneManager.cs
==============================
*/
public class IntroSceneManager : MonoBehaviour
{
    private GameManager gm = null;
    [SerializeField] 
    private Button newGameButton = null;

    private void Awake()
    {
        if (newGameButton) newGameButton.onClick.AddListener(LoadWorldMapScene);

        gm = GameManager.Instance;
    }

    private void LoadWorldMapScene()
    {
        gm.LoadSceneWithName("WorldMap");
    }
}
