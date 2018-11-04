using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{

    public Action close;

    private DialogData data;
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

    public virtual void Init(DialogData data)
    {
        this.data = data;
    }
}
