using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

//[RequireComponent(typeof(Rigidbody))]
public class TargetingRigidbody : MonoBehaviour
{
    public float speed = 2.0f;
    public Vector3 target;
    public bool IsMoving { get; private set; }
    private Vector3 desiredVelocity;
    //private Quaternion desiredRotation;
    private float lastSqrMag;

    private Transform tempTransform;
    public Transform TempTransform
    {
        get
        {
            if (tempTransform == null)
                tempTransform = GetComponent<Transform>();
            return tempTransform;
        }
    }
    //private Rigidbody tempRigidbody;
    //public Rigidbody TempRigidbody
    //{
    //    get
    //    {
    //        if (tempRigidbody == null)
    //            tempRigidbody = GetComponent<Rigidbody>();
    //        return tempRigidbody;
    //    }
    //}

    public void StartLocalMove(Vector3 target, float speed, Action<bool> finish)
    {
        IsMoving = true;
        TempTransform.DOLocalMove(target, 1.6f).OnComplete(() =>
        {
            IsMoving = false;
            if (finish != null) finish.Invoke(true);
        });
        return;
        this.speed = speed;
        this.target = target;
        // calculate directional vector to target
        var heading = target - TempTransform.position;
        var directionalVector = heading.normalized * speed;

        // reset lastSqrMag
        lastSqrMag = Mathf.Infinity;

        if (directionalVector.magnitude > 0)
        {
            // apply to rigidbody velocity
            desiredVelocity = directionalVector;
            //desiredRotation = Quaternion.LookRotation(directionalVector);

            IsMoving = true;
        }
    }

    public void StartPositionMove(Vector3 target, float speed, Action<bool> finish)
    {
        IsMoving = true;
        TempTransform.DOMove(target, 1.0f).OnComplete(() =>
        {
            IsMoving = false;
            if (finish != null) finish.Invoke(true);
        });
        return;
    }

    public void PointDown()
    {
        GamePlayManager.Singleton.CurrentSelectedEntity = gameObject;
    }

    public void PointUp()
    {
        GamePlayManager.Singleton.CurrentSelectedEntity = null;
    }

}
