using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : KJY
 * 파일명 : IntroCameraController.cs
==============================
*/
public class IntroCameraController : MonoBehaviour
{
    [SerializeField] 
    private VolumeProfile volumeProfile = null;
    [SerializeField] 
    private GameObject title = null;
    [SerializeField] 
    private GameObject skipButton = null;
    [SerializeField] 
    private Transform landingPoint = null;
    [SerializeField]
    private GameObject smoke = null;

    private Animator titleAnimator = null;
    private Animator animator = null;
    private ChromaticAberration chromaticAberration = null;
    
    private bool isAnimation = false;
    private bool skip = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        titleAnimator = title.GetComponent<Animator>();

        animator.enabled = false;
        titleAnimator.enabled = false;
        smoke.SetActive(false);

        StartCoroutine(DropDown());
    }


    private IEnumerator IntroCameraCoroutine()
    {
        isAnimation = true;
        smoke.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        chromaticAberration.intensity.value = 0f;
        animator.enabled = true;
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length * 0.5f);
        titleAnimator.enabled = true;
    }

    private IEnumerator DropDown()
    {
        float elapsed = 0.01f;
        volumeProfile.TryGet(out chromaticAberration);

        Vector3 target = landingPoint.position;
        Vector3 camPos = transform.position;
        chromaticAberration.intensity.value = 1f;
        while (Vector3.Distance(transform.position, target) > 1f)
        {
            transform.position = Vector3.Lerp(camPos, target, elapsed);
            elapsed *= 1.02f;
            yield return null;
        }

        chromaticAberration.intensity.value = 0f;
        yield break;
    }

    public void SkipBtn()
   {
        if (skip) return;

        skipButton.SetActive(false);

        StopAllCoroutines();

        if (isAnimation)
            StartCoroutine(SkipIntro());
        else
            StartCoroutine(Skip());
    }

    private IEnumerator Skip()
    {
        skip = true;
        Vector3 target = landingPoint.position;
        Vector3 camPos = transform.position;
        while (Vector3.Distance(transform.position, target) > 1f)
        {
            transform.position = Vector3.Lerp(camPos, target, 1f);
            yield return null;
        }
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localPosition = Vector3.zero;
        StartCoroutine(SkipIntro());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!skip)
            StartCoroutine(IntroCameraCoroutine());
    }

    private IEnumerator SkipIntro()
    {
        volumeProfile.TryGet(out chromaticAberration);
        chromaticAberration.intensity.value = 0f;
        animator.enabled = true;
        animator.speed = 100f;
        yield return new WaitForSeconds(0.1f);
        titleAnimator.enabled = true;
    }
}
