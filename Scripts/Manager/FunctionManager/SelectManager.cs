using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
==============================
 * 최종수정일 : 2022-06-05
 * 작성자 : Inklie
 * 파일명 : SelectManager.cs
==============================
*/
public class SelectManager : MonoBehaviour
{
    [Header("Selected Knights")]
    [SerializeField] 
    List<GameObject> selectedList = new List<GameObject>(); // 선택한 오브젝트를 담는 리스트
    public List<GameObject> SelectedList
    {
        get { return selectedList; }
    }

    private bool isPickMode = false;
    private RaycastHit hit = default;
    private DeploymentManager deploymentManager = null;
    private MeshCollider selectionBox = null;
    private Mesh selectionMesh = null;
    private Vector2[] corners = default;  // 꼭짓점
    private Vector3[] sverts = default; // 꼭짓점의 위치
    private Vector3[] svecs = default; // 육면체 변의 길이

    [SerializeField] 
    private bool selected = false;
    private void Start()
    {
        deploymentManager = this.GetComponent<DeploymentManager>();
    }

    public void OnSingleSelect(Vector3 _p1)
    {
        // 드래그가 아닌 단일 선택
        Ray ray = Camera.main.ScreenPointToRay(_p1);

        // 마우스 다운일 때 마우스 포지션을 레이로 가져옴
        if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 9)))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // 오브젝트 추가 선택
                if (!hit.transform.gameObject.GetComponent<OutlinesController>().enabled)
                {
                    Select(hit.transform.gameObject);
                }
                else
                    SingleInitialization(hit.transform.gameObject);
            }
            else
            {
                if (!EventSystem.current.IsPointerOverGameObject(-1))
                {
                    if (!hit.transform.gameObject.GetComponent<OutlinesController>().enabled)
                    {
                        ListInitialization();
                        Select(hit.transform.gameObject);
                    }
                    else
                    {
                        if(selectedList.Count > 1)
                        {
                            ListInitialization();
                            Select(hit.transform.gameObject);
                        }
                        else
                        {
                            ListInitialization();
                        } 
                    }
                }
            }
        }
        else 
        {
            // 아무것도 히트시킨 것이 없을 때
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return;
            }
            else
            {
                if (!EventSystem.current.IsPointerOverGameObject(-1))
                    ListInitialization();

            }
        }
        selected = selectedList.Count > 0 ? true : false;
    }
    public void OnBattleSingleSelect(Vector3 _p1)
    {
        // 드래그가 아닌 단일 선택
        Ray ray = Camera.main.ScreenPointToRay(_p1);

        // 마우스 다운일 때 마우스 포지션을 레이로 가져옴
        if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 9)))
        {

            // 다른 오브젝트를 누를 때
            if (!EventSystem.current.IsPointerOverGameObject(-1))
            {

                ListInitialization();
                Select(hit.transform.gameObject);
            }

        }
        else
        {
             if (!EventSystem.current.IsPointerOverGameObject(-1))
                 ListInitialization();  
        }
        selected = selectedList.Count > 0 ? true : false;
    }

    private void Select(GameObject _hit)
    {
        // 선택 시 적용사항
        selectedList.Add(_hit);
        _hit.GetComponent<OutlinesController>().enabled = true;
    }

    public bool IsSelected()
    {
        return selected;
    }
    public void SetSelected(bool _isSelected)
    {
        selected = _isSelected;
    }
    public void OnListSelect(KnightInformation _selectListKnights)
    {
        // UI 리스트에서 선택했을 때
        if (Input.GetKey(KeyCode.LeftShift))
        {
            SelectList(_selectListKnights);
        }
        else
        {
            ListInitialization();
            SelectList(_selectListKnights);
        }
        selected = selectedList.Count > 0 ? true : false;
    }

    public void SelectList(KnightInformation _selectListKnights)
    {
        // 리스트 선택
        selectedList.Add(_selectListKnights.transform.parent.gameObject);
        _selectListKnights.transform.parent.gameObject.GetComponent<OutlinesController>().enabled = true;
    }

    public void ListInitialization()
    {
        // 리스트 초기화 
        for (int i = 0; i < selectedList.Count; i++)
        {
            selectedList[i].GetComponent<OutlinesController>().enabled = false;
            deploymentManager.HologramActive(i,false);
        }
        selectedList.Clear();
    } 
    public void SingleInitialization(GameObject _go)
    {
        // 단일 선택 초기화
        for(int i = 0; i < selectedList.Count; i++)
        {
            if(selectedList[i] == _go)
            {
                // 삭제
                selectedList[i].GetComponent<OutlinesController>().enabled = false;
                selectedList.RemoveAt(i);
            }
        }
    }
    public bool IsListSelected(KnightInformation _go)
    {
        // 리스트 선택했는지 여부
        for (int i = 0; i < selectedList.Count; i++)
        {
            if (selectedList[i] == _go.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsPickMode()
    {
        // 픽 모드인지 여부
        return isPickMode;
    }
    public void SetPickMode(bool _bool)
    {
        // 픽 모드 셋팅
        isPickMode = _bool;
    }
    public void OnDragSelect(Vector3 _p1, Vector3 _p2) 
    {
        //드래그로 선택할 때
        #region 선택드래그박스 그리기위한 준비
        // 드래그 일때
        sverts = new Vector3[4];
        // 점 4개
        svecs = new Vector3[4];
        //그라운드와 닿는 점 4개

        int i = 0;

        // 점 p2는 마우스 포지션
        corners = GetBoundingBox(_p1, _p2);

        foreach(Vector2 corner in corners)
        {
            Ray ray = Camera.main.ScreenPointToRay(corner);

            // 레이를 사각형의 꼭짓점으로 쏨
            if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 10)))
            {
                // 레이어가 순서가 10인 부분만 hit
                // 꼭짓점의 위치
                sverts[i] = hit.point;

                // 육면체 변의 길이
                svecs[i] = ray.origin - hit.point;

                Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), sverts[i], Color.red, 1.0f);
            }
            i++;
        }
        selectionMesh = GenerateSelectionMesh(sverts, svecs);
        selectionBox = gameObject.AddComponent<MeshCollider>();


        selectionBox.sharedMesh = selectionMesh;
        selectionBox.convex = true;
        selectionBox.isTrigger = true;

        Destroy(selectionBox, 0.02f);
        #endregion
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            // 왼쪽쉬프트를 누르지 않을 때 
            for (int k = 0; k < selectedList.Count; k++)
            {
                selectedList[k].GetComponent<OutlinesController>().enabled = false;
                deploymentManager.HologramActive(k, false);
            }
            selectedList.Clear();
        }
    } 

    public void Pick()
    {
        // 찝기
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 50000.0f, (1 << 8)))
        {
            Debug.DrawRay(hit.point, Vector3.up*5,Color.cyan,0.1f);
            Vector3 hitPos = hit.point;
            selectedList[0].transform.position = hitPos + Vector3.up;
        }
    }


    private Vector2[] GetBoundingBox(Vector2 p1, Vector2 p2)
    {

        // 위치에 맞는 꼭짓점 만들기 
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x)
        {
            if (p1.y > p2.y)
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else 
        {
            if (p1.y > p2.y)
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }

        }
        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;
    }


    
    private Mesh GenerateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        // 충돌할 선택 박스 만들기
        Vector3[] verts = new Vector3[8];
        int[] tris = 
            { 0, 1, 2,
            2, 1, 3,
            4, 6, 0,
            0, 6, 2,
            6, 7, 2,
            2, 7, 3,
            7, 5, 3,
            3, 5, 1,
            5, 0, 1,
            1, 4, 0,
            4, 5, 6,
            6, 5, 7 }; //map the tris of our cube

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }
    private void OnTriggerEnter(Collider other)
    {
        // 선택 박스 충돌
        if (other.gameObject.layer == 9)
        {
            if (!other.gameObject.GetComponent<OutlinesController>().enabled)
            {
                selectedList.Add(other.gameObject);
                other.transform.gameObject.GetComponent<OutlinesController>().enabled = true;
                selected = true;
            }
            else
            {
                if (selectedList.Count == 0)
                    selected = false;
                SingleInitialization(other.gameObject);
            }
        }
    }
}