using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class timerevent
{
    [SerializeField]
    protected float rate;

    [SerializeField]
    protected float LerpTime;

    protected float curenttime;

    [SerializeField]
    protected Color col;

    // Use this for initialization

    private void Awake()
    {

    }

    void Start() {

    }

    private void Update()
    {
        
    }
    
    public float GetRate()
    {
        return rate;
    }

    public Color GetColor()
    {
        return col;
    }

    public float GetLerpTime ()
    {
        return LerpTime;
    }

    public void SetLerpTime(float var)
    {
        LerpTime = var;
    }
}
