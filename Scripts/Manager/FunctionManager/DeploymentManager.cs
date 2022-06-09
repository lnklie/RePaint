using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : DefaultUIManager.cs
==============================
*/
public class DeploymentManager : MonoBehaviour
{                             
    [Header("Hologram Manage")]
    [SerializeField] 
    private List<GameObject> hologramObject = new List<GameObject>();

    private int w = 0;
    private int h = 0;
    private int[] indices = { 0 };
    private SelectManager selectManager = null;
    private GameObject deploymentBox = null;
    private Mesh deploymentMesh = null;
    private Material mat = null;
    private Material holoMat = null; // 임시 홀로그램 메테리얼
    private Vector3[] startendpos = default;
    private Vector3[] deployPos= default;
    private RaycastHit hit = default;

    [SerializeField] 
    private Mesh holoMesh = null;
    private void Awake()
    {
        selectManager = this.GetComponent<SelectManager>();
    }
    void Start()
    {
        GenerateHologram();
        GenerateDeploymentBox();
    }

    private void GenerateHologram()
    {
        // 홀로그램 생성
        GameObject P_hologram = new GameObject("HoloGram");
        P_hologram.SetActive(false);
        holoMat = Resources.Load<Material>("Materials\\Hologram");
        for (int i = 0; i < 100; i++)
        {
            hologramObject[i] = Instantiate(P_hologram, this.transform);
            hologramObject[i].AddComponent<MeshFilter>().mesh = holoMesh;
            hologramObject[i].AddComponent<MeshRenderer>().material = holoMat;
            hologramObject[i].transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }
    private void GenerateDeploymentBox()
    {
        // 배치박스 생성
        mat = Resources.Load<Material>("Materials\\DeployBox_Mat");

        deploymentBox = new GameObject("DragBox");
        deploymentBox.AddComponent<MeshFilter>();
        deploymentBox.AddComponent<MeshRenderer>();
        deploymentBox.GetComponent<MeshRenderer>().material = mat;
    }
    public void HologramActive(int _num, bool _bool)
    {
        // 홀로그램 활성화/비활성화
        hologramObject[_num].SetActive(_bool);
    }
    public void PickDeploy()
    {
        // 집기 배치
        Debug.DrawRay(selectManager.SelectedList[0].transform.position, Vector3.up * 3f, Color.red, 60f);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 8 | 1 << 12)))
        {
            Debug.DrawRay(hit.point, Vector3.up * 5, Color.cyan, 0.1f);
            Vector3 hitPos = hit.point;
            selectManager.SelectedList[0].GetComponent<NavMeshAgent>().Warp(hitPos);
        }
       
        selectManager.SingleInitialization(selectManager.SelectedList[0]);
        selectManager.SetSelected(false);

    }
    public void DeployUnits()
    {
        // 유닛 배치
        int selectedCount = selectManager.SelectedList.Count;

        for (int i = 0; i < selectedCount; ++i)
        {
            selectManager.SelectedList[i].transform.position =
                hologramObject[i].transform.position;
        }
        CancelDeploy();
    }
    public void CancelDeploy() 
    {
        // 배치 취소
        deploymentBox.gameObject.SetActive(false);

        for (int x = 0; x < selectManager.SelectedList.Count; x++)
        {
            selectManager.SelectedList[x].GetComponent<OutlinesController>().enabled = false;
            hologramObject[x].SetActive(false);
        }
        selectManager.SelectedList.Clear();
    }

    public void OnDeployDrag(Vector3 _p1, Vector3 _dragP2) 
    {
        // 배치 드래그 중
        deploymentBox.gameObject.SetActive(true);
        // 마우스지점
        startendpos = new Vector3[2] { _p1, _dragP2 };

        // 마우스에서 레이를쏴서 그라운드랑 닿는지점
        Vector3[] hitPos = new Vector3[2];

        for (int j = 0; j < startendpos.Length; j++)
        {
            Ray ray = Camera.main.ScreenPointToRay(startendpos[j]);
            // 레이를 사각형의 꼭짓점으로 쏨
            if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 8 )))
            {
                hitPos[j] = hit.point;
            }
        }

        #region 배치박스 그리기 위해 점찍기
        int i = 0;
        w = (Mathf.CeilToInt(Mathf.Abs(hitPos[1].x - hitPos[0].x)) + 1);
        h = (Mathf.CeilToInt(Mathf.Abs(hitPos[0].z - hitPos[1].z)) + 1);

        int size = (Mathf.Abs(w * h));

        deployPos = new Vector3[size];

        float smallX = (hitPos[0].x < hitPos[1].x) ? hitPos[0].x : hitPos[1].x;
        float smallZ = (hitPos[1].z < hitPos[0].z) ? hitPos[1].z : hitPos[0].z;


        for (int z = 0; z < h; z++)
        {
            for (int x = 0; x < w; x++)
            {

                deployPos[i] = new Vector3(smallX + x ,
                hit.transform.GetComponent<Terrain>().SampleHeight(
                    new Vector3(smallX + x, 0f, smallZ + z)) + 0.5f,
                smallZ + z );
                i++;
            }
        }
        GenerateHologram(w, h);
        deploymentMesh = GenerateDeploymentMesh(deployPos, w, h);
        deploymentBox.GetComponent<MeshFilter>().mesh = deploymentMesh;
        #endregion
    }

    public void OnUIDeployCancel()
    {
        // UI에서 배치 취소
        deploymentBox.gameObject.SetActive(false);
        for (int x = 0; x < selectManager.SelectedList.Count; x++)
        {
            hologramObject[x].SetActive(false);
        }
    }

    private Mesh GenerateDeploymentMesh(Vector3[] _vertexs, int _width, int _height)
    {
        // 배치 박스 그리기
        int idxBufSize = (_width - 1) * (_height - 1) * 6;
        indices = new int[idxBufSize];
        
        int idx = 0;
        for (int z = 0; z < _height - 1; ++z) 
            for (int x = 0; x < _width - 1; ++x) 
            {
                int v1 = (z * _width) + x;  
                int v2 = v1 + 1;            
                int v3 = v1 + _width;       
                int v4 = v3 + 1;            


                indices[idx++] = v3;        
                indices[idx++] = v4;        
                indices[idx++] = v1;        
                indices[idx++] = v1;        
                indices[idx++] = v4;        
                indices[idx++] = v2;        
            }
 
        Mesh deploymentMesh = new Mesh();
        deploymentMesh.vertices = _vertexs;
        deploymentMesh.triangles = indices;
        return deploymentMesh;
    }

    private void GenerateHologram(int _width, int _height)
    {
        // 홀로그램 만들기
        int selectedCount = GetComponent<SelectManager>().SelectedList.Count;

        if (5 * selectedCount > (_width * _height))
        {
            deploymentBox.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0f, 0f, 0.5f);
            for(int i = 0; i < selectedCount; i++)
            {
                hologramObject[i].gameObject.SetActive(false);
                hologramObject[i].transform.position = selectManager.SelectedList[i].transform.position;
            }
            return;
        }
        else
        {
            deploymentBox.GetComponent<MeshRenderer>().material.color = new Color(0f, 1.0f, 0f, 0.5f);
        }
        int widthCnt = Mathf.CeilToInt(Mathf.Sqrt(selectedCount * (_width / (float)_height)));
        if (widthCnt > selectedCount) widthCnt = selectedCount;
        int heightCnt = Mathf.CeilToInt(selectedCount / (float)widthCnt);
        if (heightCnt > selectedCount) heightCnt = selectedCount;

        int offsetX = _width / widthCnt;
        int offsetZ = _height / heightCnt;

        int paddingX = (_width - (offsetX * (widthCnt - 1))) / 2;
        int paddingY = (_height - (offsetZ * (heightCnt - 1))) / 2;

        
        for (int i = 0; i < selectedCount ; i++)
        {
            int x = i % widthCnt; 
  
            int y = i / widthCnt; 

            hologramObject[i].gameObject.SetActive(true);
            hologramObject[i].transform.position
                            = deployPos[(paddingY + (y * offsetZ)) * _width + (paddingX + (x * offsetX))];

            hologramObject[i].transform.position += Vector3.up * 0.2f;
        }
    }
}