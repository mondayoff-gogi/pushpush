using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;
        }        
    }

    public PlayerData[] PlayerDatas;

    [System.Serializable]
    public struct PlayerData//
    {
        public float MaxSpeed;
        public float Accelerate;
        public float Friction;
        public float CannotMoveTimer;
        public float MaxHP;
        public float Damage;
        public int BulletIndex;
    }
    
    public const float MaxSpeed = 50;
    public const float MaxAccelerate = 10;
    public const float MaxFriction = 1;
    public const float MaxCannotMoveTimer = 2;
    public const float MaxMaxHP = 30;
    public const float MaxDamage = 30;
    



}
