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
public class DollManagePanel : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject nameTagPrefab = null;

    [Header("Reference Components")]
    [SerializeField] private Button onOffButton = null;
    [SerializeField] private Button aliveKnightListButton = null;
    [SerializeField] private Button deadKnightListButton = null;
    [SerializeField] private List<GameObject> nameTags = null;
    [SerializeField] private Text soulCount = null;
    [SerializeField] private DollUpgradePanel upgradePanel = null;

    [Header("Button Sprite")]
    [SerializeField] private Sprite activeAliveKnight = null;
    [SerializeField] private Sprite deactiveAliveKnight = null;
    [SerializeField] private Sprite activeDeadKnight = null;
    [SerializeField] private Sprite deactiveDeadKnight = null;


    private bool isOpen = false;
    private int index = 0;
    private Dictionary<string, GameObject> nameTagDic = new Dictionary<string, GameObject>();
    private AllyKnightsManager allyKnightsManager = null;
    private Animator anim = null;

    private Image aliveBtnImage;
    private Image deadBtnImage;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        onOffButton.onClick.AddListener(ToggleDrawer);
        aliveKnightListButton.onClick.AddListener(DeactivateAllNameTag);
        aliveKnightListButton.onClick.AddListener(ActivateAliveNameTags);
        deadKnightListButton.onClick.AddListener(DeactivateAllNameTag);
        deadKnightListButton.onClick.AddListener(ActivateDeadNameTags);

        aliveBtnImage = aliveKnightListButton.GetComponentsInChildren<Image>()[1];
        deadBtnImage = deadKnightListButton.GetComponentsInChildren<Image>()[1];
    }

    private void Start()
    {
        if (GameManager.Instance.loadGame == false)
            Initailize();
    }


    public void Initailize()
    {
        allyKnightsManager = GameObject.FindObjectOfType<AllyKnightsManager>();

        PairringNameTag();
        DeactivateAllNameTag();
        ActivateAliveNameTags();
        UpdateSoulText();
    }




    public void ToggleDrawer()
    {
        isOpen = !isOpen;
        anim.SetBool("ListOpen", isOpen);
        onOffButton.transform.localScale = new Vector3(onOffButton.transform.localScale.x * -1f, 1f, 1f);

        upgradePanel.ClosePanel();
    }

    public void AddDoll()
    {
        if (GameManager.soul < 1) return;

        if(!allyKnightsManager)
            allyKnightsManager = GameObject.FindObjectOfType<AllyKnightsManager>();

        allyKnightsManager.AddDoll();
        KnightInformation info = allyKnightsManager.GetKnight(allyKnightsManager.KnightCount() - 1);


        if(index >= nameTags.Count)
        {
            nameTags.Add(Instantiate(nameTagPrefab, GetComponentInChildren<VerticalLayoutGroup>().transform));
        }
        SetNameTag(nameTags[index], info);
        index++;
        ActivateAliveNameTags();

        GameManager.soul--;
        UpdateSoulText();
    }

    public void TurnOnKnightStatus(KnightInformation _infomation)
    {
        if (upgradePanel.IsEqaul(_infomation) && upgradePanel.gameObject.activeSelf)
        {
            upgradePanel.ClosePanel();
            return;
        }

        upgradePanel.SetDollInformation(_infomation);

        if (!allyKnightsManager)
            allyKnightsManager = GameObject.FindObjectOfType<AllyKnightsManager>();

        bool isDead = allyKnightsManager.IsDead(_infomation.GetComponentInChildren<Knight>());
        upgradePanel.OpenPanel(isDead);
    }

    public void DeactiveNameTag(string _knigthName)
    {
        nameTagDic[_knigthName].SetActive(false);
    }

    private void PairringNameTag()
    {
        if (!allyKnightsManager) {
            Debug.Log("AllyKnightsManager is Null");
            return;
        }

        List<KnightInformation> aliveKnightsInfo = allyKnightsManager.GetAliveKnightInformationList();

        for(int i = 0; i < aliveKnightsInfo.Count; i++)
        {
            SetNameTag(nameTags[i], aliveKnightsInfo[i]);
            index++;
        }

        List<KnightInformation> deadKnightsInfo = allyKnightsManager.GetDeadKnightInformationList();
        for (int i = index; i < deadKnightsInfo.Count + index; i++)
        {
            SetNameTag(nameTags[i], deadKnightsInfo[i - index]);
        }
        index += deadKnightsInfo.Count;
    }

    public void ActivateAliveNameTags()
    {
        if (!allyKnightsManager)
            allyKnightsManager = GameObject.FindObjectOfType<AllyKnightsManager>();

        List<KnightInformation> aliveKnightsInfo = allyKnightsManager.GetAliveKnightInformationList();
        foreach (KnightInformation info in aliveKnightsInfo)
            nameTagDic[info.KnightName].SetActive(true);

        upgradePanel.ClosePanel();
        aliveBtnImage.sprite = activeAliveKnight;
        deadBtnImage.sprite = deactiveDeadKnight;
    }

    public void ActivateDeadNameTags()
    {
        if (!allyKnightsManager)
            allyKnightsManager = GameObject.FindObjectOfType<AllyKnightsManager>();

        List<KnightInformation> deadKnightsInfo = allyKnightsManager.GetDeadKnightInformationList();
        foreach (KnightInformation info in deadKnightsInfo)
            nameTagDic[info.KnightName].SetActive(true);

        upgradePanel.ClosePanel();
        aliveBtnImage.sprite = deactiveAliveKnight;
        deadBtnImage.sprite = activeDeadKnight;
    }

    public void UpdateSoulText() {
        soulCount.text = GameManager.soul.ToString();
    }

    public void DeactivateAllNameTag()
    {
        foreach (GameObject nameTag in nameTags)
            nameTag.SetActive(false);
    }


    private void SetNameTag(GameObject _nameTag, KnightInformation _info)
    {
        nameTagDic.Add(_info.KnightName, _nameTag); 
        Text[] texts = _nameTag.GetComponentsInChildren<Text>();
        texts[0].text = _info.KnightName;
        texts[1].text = _info.KnightRank.ToString();

        Button btn = _nameTag.GetComponentInChildren<Button>();
        btn.onClick.AddListener(() => TurnOnKnightStatus(_info));    
    }
}
