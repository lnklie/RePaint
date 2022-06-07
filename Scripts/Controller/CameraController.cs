using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : KJY
 * 파일명 : CameraController.cs
==============================
*/
public class CameraController : MonoBehaviour
{
    [SerializeField] 
    private Transform[] views = null;
    [SerializeField]
    private Transform cameraHolder = null;
    [SerializeField] 
    private Transform quarterHolder = null;
    [SerializeField] 
    private VolumeProfile volumeProfile = null;
    private TutorialSceneManager tutorialManager = null;
    private ChromaticAberration chromaticAberration = null;
    private RaycastHit hit = default;

    // 카메라 초기 거리
    private float camZoomInitialDistance =0;
    // 카메라 휠 입력 값
    private float cameraWheelInput = 0;
    // 현재 휠 입력 값
    private float currentWheel = 0;
 
    private bool firstPersonMode = false;

    [Serializable]
    public class CameraOption
    {
        [Range(1f, 10f), Tooltip("카메라 상하좌우 회전 속도")]
        public float rotationSpeed = 2f;
        [Range(-90f, 0f), Tooltip("올려다보기 제한 각도")]
        public float lookUpDegree = -60f;
        [Range(0f, 75f), Tooltip("내려다보기 제한 각도")]
        public float lookDownDegree = 75f;
        [Range(0f, 20f), Space, Tooltip("줌 확대 최대 거리")]
        public float zoomInDistance = 3f;
        [Range(0f, 20f), Tooltip("줌 축소 최대 거리")]
        public float zoomOutDistance = 3f;
        [Range(1f, 50f), Tooltip("줌 속도")]
        public float zoomSpeed = 10f;
        [Range(0.01f, 2f), Tooltip("줌 가속")]
        public float zoomAccel = 0.1f;
    }

    public CameraOption CamOption => _cameraOption;

    [Space]
    [SerializeField] private CameraOption _cameraOption = new CameraOption();

    private Vector3 target = default;
    private Vector3 camPos = default;
    private Vector3 originLocalPos = default;
    private Quaternion originLocalRot = default;

    private float speedMove = 30f;

    private void Start()
    {
        tutorialManager = FindObjectOfType<TutorialSceneManager>();
    }

    void Update()
    {
         if (firstPersonMode == true) return;

        SetValuesByKeyInput();
        CameraMove();
        CameraZoom();
        CameraRotate();
    }

    private void SetValuesByKeyInput()
    {
        camZoomInitialDistance = Vector3.Distance(hit.point, transform.position);
        cameraWheelInput = Input.GetAxisRaw("Mouse ScrollWheel");
        currentWheel = Mathf.Lerp(currentWheel, cameraWheelInput, CamOption.zoomAccel);
    }

    private void CameraMove()
    {
        float keyH = Input.GetAxis("Horizontal");
        float keyV = Input.GetAxis("Vertical");
        keyH = keyH * speedMove * Time.deltaTime;
        keyV = keyV * speedMove * Time.deltaTime;


        Vector3 vDir = transform.forward;
        vDir.y = 0f;
        vDir.Normalize();
        transform.parent.position += vDir * keyV;

        Vector3 hDir = transform.right;
        hDir.y = 0f;
        hDir.Normalize();
        transform.parent.position += hDir * keyH;
    }

    private void CameraZoom()
    {
        if (Mathf.Abs(currentWheel) < 0.01f) return; // 휠 입력 있어야 가능

        Transform CamTr = transform;

        float zoom = Time.deltaTime * CamOption.zoomSpeed;
        float currentCamToHolderDist = Vector3.Distance(CamTr.position, hit.point);
        Vector3 move = Vector3.forward * zoom * currentWheel * 10f;

        // Zoom In
        if (currentWheel > 0.01f)
        {
            if (camZoomInitialDistance - currentCamToHolderDist < CamOption.zoomInDistance)
            {
                CamTr.Translate(move, Space.Self);
            }
        }
        // Zoom Out
        else if (currentWheel < -0.01f)
        {

            if (currentCamToHolderDist - camZoomInitialDistance < CamOption.zoomOutDistance)
            {
                CamTr.Translate(move, Space.Self);
            }
        }
    }

    private void CameraRotate()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Vector3 screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            Physics.Raycast(ray, out hit, 1000f, (1 << 8));
        }

        if (Input.GetMouseButton(2))
        {
            float axis = Input.GetAxis("Mouse X");
            float deltaCoef = Time.deltaTime * 100f;
            if (hit.collider != null)
            {
                if (axis > 0f)
                {
                    transform.RotateAround(hit.point, Vector3.up, deltaCoef);
                }
                else if (axis < 0f)
                {
                    transform.RotateAround(hit.point, -Vector3.up, deltaCoef);
                }
            }
        }
    }

    public void CameraViewToggle(Transform _camHolder, Action _callback = null)
    {
        cameraHolder = _camHolder;
        StopAllCoroutines();
        StartCoroutine(DescentGod(_callback));
    }

    private IEnumerator DescentGod(Action _callback)
    {
        float elapsed = 0.01f;
        volumeProfile.TryGet(out chromaticAberration);

        firstPersonMode = !firstPersonMode;

        if (firstPersonMode == true)
        {
            //강림
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            originLocalPos = transform.localPosition;
            originLocalRot = transform.localRotation;

            target = cameraHolder.position;
            camPos = transform.position;
            chromaticAberration.intensity.value = 1f;
            while (Vector3.Distance(transform.position, target)> 1f)
            {
                transform.position = Vector3.Lerp(camPos, target, elapsed);
                elapsed *= 1.02f;
                yield return null;
            }
            tutorialManager?.NextTutorial();
            chromaticAberration.intensity.value = 0f;
            transform.SetParent(cameraHolder);
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localPosition = Vector3.zero;
            yield break;
        }

        if (firstPersonMode == false)
        {
            elapsed = 0.01f;
            camPos = transform.position;
            Vector3 rigPos = quarterHolder.transform.position;
            chromaticAberration.intensity.value = 1f;
            transform.localRotation = Quaternion.Euler(30f, 45f, 0f);
            while (Vector3.Distance(rigPos, transform.position) > 1f)
            {
                transform.position = Vector3.Lerp(camPos, rigPos, elapsed);
                elapsed *= 1.02f;
                yield return null;
            }

            tutorialManager?.NextTutorial();
            _callback?.Invoke();
            chromaticAberration.intensity.value = 0f;
            transform.SetParent(quarterHolder);
            transform.localPosition = originLocalPos;
            transform.localRotation = originLocalRot;
            yield break;
        }
    }


    public void CloseUpToMVP(Transform _target, Action _onCloseUp, bool _isWin = true)
    {
        StartCoroutine(CloseUp(_target, _onCloseUp, _isWin));
    }

    private IEnumerator CloseUp(Transform _target, Action _onCloseUp, bool _isWin)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float duration = 3.0f;
        float elapsed = 0f;

        Vector3 targetPos;
        Quaternion targetRot;

        if (_isWin)
        {
            targetPos = _target.position + (_target.forward * 2.5f + _target.right) + (Vector3.up);
            targetRot = Quaternion.LookRotation(-_target.forward);
        }
        else
        {
            targetPos = _target.position + (Vector3.up * 2f + _target.right);
            targetRot = Quaternion.LookRotation(-_target.up);
        }

        while (elapsed <= duration)
        {
            elapsed += Time.deltaTime;

            transform.position = Vector3.Slerp(startPos, targetPos, elapsed / duration);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
            yield return null;
        }
        transform.position = targetPos;
        transform.rotation = targetRot;
        _onCloseUp?.Invoke();
    }

    //public void ChangeViewButton(int _cameraNum)
    //{
    //    StartCoroutine(ChangeView(_cameraNum));

    //}
    public void StartBattleDirection()
    {
        StartCoroutine(StartBattleDirectionCoroutine());
    }

    //private IEnumerator ChangeView(int _cameraNum)
    //{
    //    Vector3 startPos = transform.position;
    //    Quaternion startRot = transform.rotation;
    //    float duration = 1.0f;
    //    float elapsed = 0f;
    //    Vector3 targetPos = views[_cameraNum].position;
    //    Quaternion targetRot = views[_cameraNum].rotation;

    //    while (elapsed <= duration)
    //    {
    //        elapsed += Time.deltaTime;

    //        transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
    //        transform.rotation = Quaternion.Lerp(startRot, targetRot, elapsed / duration);
    //        yield return null;
    //    }
    //    transform.position = targetPos;
    //    transform.rotation = targetRot;
    //}
    private IEnumerator StartBattleDirectionCoroutine()
    {

        float duration = 3.0f;
        float elapsed = 0f;

        this.transform.localPosition = new Vector3(200f, 0f, 180f);
        this.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
        Vector3 startPos = this.transform.localPosition;
        Vector3 targetPos = transform.localPosition + new Vector3(-90f, 0f, 0f);


        while (elapsed <= duration)
        {
            elapsed += Time.deltaTime;

            transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        elapsed = 0f;
        this.transform.localPosition = new Vector3(200f, 0f, 260f);
        this.transform.rotation = Quaternion.Euler(60f, 180f, 0f);
        startPos = this.transform.localPosition;

        Vector3 targetPos2 = transform.localPosition + new Vector3(-90f, 0f, 0f);

        while (elapsed <= duration)
        {
            elapsed += Time.deltaTime;

            transform.localPosition = Vector3.Lerp(startPos, targetPos2, elapsed / duration);

            yield return null;
        }
        this.transform.localPosition = new Vector3(200f, 0f, 260f);
        this.transform.rotation = Quaternion.Euler(60f, 180f, 0f);
        //ChangeViewButton(4);
    }
}

