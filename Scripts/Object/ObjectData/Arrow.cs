using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
==============================
 * 최종수정일 : 2022-06-06
 * 작성자 : JSJ
 * 파일명 : Arrow.cs
==============================
*/
public class Arrow : MonoBehaviour
{
    private Rigidbody rb = null;
    private Collider hitCollider = null;
    private Knight shooter = null;
    private Knight target = null;
    private RaycastHit hit;

    private bool isClose = false;
    private bool isHit = false;
    private int layerMask;

    public event System.Action onHit = null;
    public int team;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hitCollider = GetComponent<Collider>();
        layerMask = 1 << LayerMask.NameToLayer("Targetable");
    }

    private void OnEnable()
    {
        rb.useGravity = false;
        rb.isKinematic = false;
        isHit = false;
    }


    private void FixedUpdate()
    {
        if (!isHit) {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
        
        if(Physics.Raycast(transform.position, transform.forward, out hit, 10f, layerMask))
        {
            if (!isClose)
            {
                hit.collider.GetComponent<Knight>().Defend(transform, transform);
                isClose = true;
            }
        }
    }


    public void Shoot(Vector3 _dir, float _dist, Knight _shooter)
    {
        shooter = _shooter;
        team = _shooter.Team;
        _dir += Vector3.up * 0.3f;
        float power = _dist + Random.Range(15f, 25f);
        rb.useGravity = true;
        rb.AddForce(_dir * power, ForceMode.Impulse);
        
        hitCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Knight") && _other.GetComponent<Knight>().Team == team) return;
        
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        hitCollider.enabled = false;
        transform.SetParent(_other.transform);
        isHit = true;
        onHit?.Invoke();
    }

    public Knight GetShooter()
    {
        return shooter;
    }
}
