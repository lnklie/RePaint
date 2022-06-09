using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : Inklie
 * 파일명 : JSJ.cs
==============================
*/
public class BattleEnvironmentSpawner : MonoBehaviour
{
    public static BattleEnvironmentSpawner instance = null;

    [Header("Ally Knight")]
    [SerializeField] 
    private AllyKnightsManager allyKnightsManager = null;

    [Header("Map Environment Prefabs")]
    [SerializeField]
    private GameObject globalVolumePrefab = null;
    [SerializeField]
    private GameObject reflectionProbPrefab = null;
    [SerializeField]
    private GameObject dayAndNightPrefab = null;
    [SerializeField] 
    private GameObject cloudPrefab = null;
    [SerializeField] 
    private GameObject moonPrefab = null;
    [SerializeField]
    private GameObject directionalLightPrefab = null;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += InstantiateEnviroment;
        SceneManager.sceneLoaded += SetActiveAllyKnights;
        transform.SetParent(GameManager.Instance.transform);
    }

    private void InstantiateEnviroment(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.StartsWith("Battle")) return;

        Instantiate(globalVolumePrefab);
        Instantiate(reflectionProbPrefab);
        Instantiate(dayAndNightPrefab);
        Instantiate(cloudPrefab);
        Instantiate(moonPrefab);
        Instantiate(directionalLightPrefab);
    }

    private void SetActiveAllyKnights(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.StartsWith("Battle")) return;

        allyKnightsManager.OnBattleScene();
    }

    //private void InstantiateBattleSystem(Scene scene, LoadSceneMode mode)
    //{
    //    if (!scene.name.StartsWith("Battle")) return;

    //    Instantiate(cameraRig);
    //    Instantiate(deploymentCanvas);
    //    Instantiate(defaultCanvas);
    //    Instantiate(battleCanvas);
    //    Instantiate(battleResultCanvas);
    //    Instantiate(deploymentSystem);
    //    Instantiate(userControl);
    //}
}
