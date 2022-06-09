using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : JSJ
 * 파일명 : KnightCustomizer.cs
==============================
*/
public class KnightCustomizer : MonoBehaviour
{
    private SkinnedMeshRenderer[] skmrs = null;
    private EKnightType knightType = EKnightType.Default;

    [SerializeField] 
    private SelectManager selectManager = null;
    public enum HeadObject
    {
        Pilosity, Helmet_Patina, Helmet_Blackened, Helmet_Clean,
        Chaperon, KettleHat_Backened, KettleHat_Clean, KettleHat_Patina,
    }

    public enum Weapon
    {
        Longsword, Lance, Null
    }

    public enum Shield
    {
        PunchingShield_Type01, PunchingShield_Type02, PunchingShield_Type03, PunchingShield_Type04,
        PunchingShield_Type05, PunchingShield_Type06, PunchingShield_Type07, Shield_Type01,
        Shield_Type02, Shield_Type03, Shield_Type04, Shield_Type05, Shield_Type06, Shield_Type07, Bow,
        Null
    }

    public enum CoatOfArms { 
        CoatOfArms_type01, CoatOfArms_type02, CoatOfArms_type03,
        CoatOfArms_type04, CoatOfArms_type05, CoatOfArms_type06,
        CoatOfArms_type07
    }

    public enum KnightBase { 
        Type00,
        Type01_Blackened, Type01_CleanSteel, Type01_Patina,
        Type02_Blackened, Type02_CleanSteel, Type02_Patina,
        Type03_Blackened, Type03_CleanSteel, Type03_Patina
    }
    
    [SerializeField] private HeadObject head;
    [SerializeField] private Weapon weapon;
    [SerializeField] private Shield shield;

    [SerializeField] private CoatOfArms coat;
    [SerializeField] private KnightBase body;
    
    private Material[] coatMaterials = null;
    private Material[] bodyMaterials = null;


    private CoatOfArms originCoat;

    private void Awake()
    {
        coatMaterials = Resources.LoadAll<Material>("Knights\\CoatOfArms");
        bodyMaterials = Resources.LoadAll<Material>("Knights\\KnightBase");

        originCoat = coat;
    }

    public void OnBattleScene()
    {
        selectManager = GameObject.FindObjectOfType<SelectManager>();
    }

    public IEnumerator CustomizeKnight(Knight _knight, bool _isRandom, int _type = 0)
    {
        if (_isRandom)
        {
            head = (HeadObject)Random.Range(0, 8);

            int rand = Random.Range(0, 3);

            if (rand == 1)
            {
                shield = Shield.Null;
                weapon = Weapon.Longsword;
            }
            else if (rand == 2)
            {
                weapon = Weapon.Lance;
                shield = (Shield)((int)coat + 7);
            }
            else
            {
                shield = Shield.Bow;
                weapon = Weapon.Null;
            }

            body = (KnightBase)Random.Range(0, bodyMaterials.Length);
        }

        skmrs = _knight.GetComponentsInChildren<SkinnedMeshRenderer>();



        foreach (SkinnedMeshRenderer skmr in skmrs)
        {
            if (skmr.gameObject.name.StartsWith("CoatOfArms"))
                skmr.material = coatMaterials[(int)coat];
            yield return null;
        }

        foreach (SkinnedMeshRenderer skmr in skmrs)
        {
            if (skmr.gameObject.name.StartsWith("Knight_base") ||
                skmr.gameObject.name.StartsWith("Shoulders"))
                skmr.material = bodyMaterials[(int)body];
            yield return null;
        }

        _knight.GetComponentInChildren<HeadHolder>().ActivateObject((int)head);
       
        if (weapon != Weapon.Null)
            _knight.GetComponentInChildren<WeaponHolder>()?.ActivateObject((int)weapon);
        else
            _knight.GetComponentInChildren<WeaponHolder>()?.InactiveAll();

        if (shield != Shield.Null)
            _knight.GetComponentInChildren<ShieldHolder>().ActivateObject((int)shield);
        else
            _knight.GetComponentInChildren<ShieldHolder>()?.InactiveAll();

        if (weapon == Weapon.Longsword)
            _knight.InitializeKnightInfomation(EKnightType.Sword);
        else if (weapon == Weapon.Lance)
            _knight.InitializeKnightInfomation(EKnightType.Spear);
        else if (weapon == Weapon.Null && shield == Shield.Bow)
            _knight.InitializeKnightInfomation(EKnightType.Bow);

        yield return null;
    }

    public void CustomizeSelectedKnight()
    {
        for (int i = 0; i < selectManager.SelectedList.Count; i++) 
        {
            Knight knight = selectManager.SelectedList[i].GetComponentInChildren<Knight>();
            StartCoroutine(CustomizeKnight(knight, false));
        }
    }

    public void CustomizeAllKnight(List<Knight> _knights, bool _isRandom)
    {
        for (int i = 0; i < _knights.Count; i++)
            StartCoroutine(CustomizeKnight(_knights[i], _isRandom));
    }

    public void CustomizeEnemyKnight(List<Knight> _knights)
    {
        StartCoroutine(EnemyCustomzieCorotine(_knights));
    }

    private IEnumerator EnemyCustomzieCorotine(List<Knight> _knights)
    {
        foreach (Knight knight in _knights)
        {
            if (knight.transform.parent.name == "Bow")
                SetKnightType((int)EKnightType.Bow);
            else if (knight.transform.parent.name == "Sword")
                SetKnightType((int)EKnightType.Sword);
            else if (knight.transform.parent.name == "Spear")
                SetKnightType((int)EKnightType.Spear);

            StartCoroutine(CustomizeKnight(knight, false));
            yield return null;
        }
    }

    public void SetKnightType(int _type)
    {
        if((EKnightType)_type == EKnightType.Sword)
        {
            weapon = Weapon.Longsword;
            //coat = CoatOfArms.CoatOfArms_type01;
            shield = Shield.Null;
            head = (HeadObject)Random.Range(0, 8);
            knightType = (EKnightType)_type;
        }
        else if((EKnightType)_type == EKnightType.Spear)
        {
            weapon = Weapon.Lance;
            //coat = CoatOfArms.CoatOfArms_type01;
            shield = (Shield)((int)coat * 2)-1;
            head = (HeadObject)Random.Range(0, 8);
            knightType = (EKnightType)_type;
        }
        else if ((EKnightType)_type == EKnightType.Bow)
        {
            weapon = Weapon.Null;
            shield = Shield.Bow;
            head = (HeadObject)Random.Range(0, 8);
            knightType = (EKnightType)_type;
        }
    }

    public void ReturnToDoll(Knight _knight)
    {
        skmrs = _knight.GetComponentsInChildren<SkinnedMeshRenderer>();

        _knight.GetComponentInChildren<HeadHolder>().InactiveAll();
        _knight.GetComponentInChildren<WeaponHolder>()?.InactiveAll();
        _knight.GetComponentInChildren<ShieldHolder>()?.InactiveAll();

        foreach (SkinnedMeshRenderer skmr in skmrs)
        {
            if (skmr.gameObject.name.StartsWith("CoatOfArms"))
                skmr.material = coatMaterials[0];
        }

        foreach (SkinnedMeshRenderer skmr in skmrs)
        {
            if (skmr.gameObject.name.StartsWith("Knight_base") ||
                skmr.gameObject.name.StartsWith("Shoulders"))
                skmr.material = bodyMaterials[0];
        }
    }

    public void SetHead(int _type)
    {
        head = (HeadObject)_type;
    }

    public void SetWeapon(int _type)
    {
        weapon = (Weapon)_type;
    }

    public void SetShield(int _type)
    {
        shield = (Shield)_type;
    }

    public void SetCoat(int _type)
    {
        coat = (CoatOfArms)_type;
    }

    public void SetBody(int _type)
    {
        body = (KnightBase)_type;
    }
}
