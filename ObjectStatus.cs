using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStatus : MonoBehaviour
{
    [Header("DataManager")]
    [HideInInspector]
    public float MaxSpeed;
    [HideInInspector]
    public Vector3 Speed;
    [HideInInspector]
    public float Accelerate=1;
    [HideInInspector]
    public float Friction;
    [HideInInspector]
    public float CannotMoveTimer;
    [HideInInspector]
    public float MaxHP;
    [HideInInspector]
    public float Damage;
    [HideInInspector]
    public int BulletIndex;


    [Header("Editor")]
    public float CurHP;
    public bool IsGetHit;

    

    

    private void OnEnable()
    {
        IsGetHit = false;
        Speed = Vector3.zero;

        CurHP = MaxHP;
    }    
}
