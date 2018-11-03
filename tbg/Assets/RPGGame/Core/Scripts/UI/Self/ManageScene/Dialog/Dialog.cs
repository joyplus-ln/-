using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{

    public Action close;
    // Use this for initialization
    protected virtual void Start()
    {

    }

    public virtual void Close()
    {
        Destroy(gameObject);
        if (close != null)
        {
            close.Invoke();
        }
    }

}
