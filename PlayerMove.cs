using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : ObjectStatus
{
    private Vector2 ClickDownPos;
    private Vector2 ClickCurPos;
    private Vector3 Dir;
    private float SizeDir;
    private float BulletTimerTemp;
    private new Camera camera;
    private float outline;
    private float BulletTimer;

    [HideInInspector]
    public bool isMine;

    [Header("PlayerIndex")]
    public int CharIndex;
    void Start()
    {
        camera = Camera.main;

        outline = 0.14f;

        CharInit();
    }

    public void CharInit()
    {
        BulletTimerTemp = 0;
        MaxSpeed = DataManager.instance.PlayerDatas[CharIndex].MaxSpeed;
        Accelerate = DataManager.instance.PlayerDatas[CharIndex].Accelerate;
        Friction = DataManager.instance.PlayerDatas[CharIndex].Friction;
        CannotMoveTimer = DataManager.instance.PlayerDatas[CharIndex].CannotMoveTimer;
        MaxHP = DataManager.instance.PlayerDatas[CharIndex].MaxHP;
        CurHP = MaxHP;
        Damage = DataManager.instance.PlayerDatas[CharIndex].Damage;
        BulletIndex = DataManager.instance.PlayerDatas[CharIndex].BulletIndex;
        BulletTimer = GameManager.instance.Bullets[BulletIndex].GetComponent<BulletStatus>().BulletCooltime;
        GetComponentInChildren<HPScript>().HPBarInit();

        //위치..?
    }



    // Update is called once per frame
    void Update()
    {
        if(isMine)
        {
            BulletTimerTemp += Time.deltaTime;
            if (BulletTimer < BulletTimerTemp)
            {
                BulletTimerTemp = 0;

                //총알 발사!
                FireBullet();
            }

            if (Input.GetMouseButtonDown(0))
            {
                ClickDownPos = camera.ScreenToViewportPoint(Input.mousePosition);

                UIManager.instance.JoystickBack.transform.position = Input.mousePosition;
                UIManager.instance.JoystickBack.SetActive(true);
            }
            else if (Input.GetMouseButton(0) && !IsGetHit)
            {
                ClickCurPos = camera.ScreenToViewportPoint(Input.mousePosition);

                Dir = ClickCurPos - ClickDownPos;

                //camera 45 degree
                Dir.y *= Mathf.Sqrt(2);

                if (Dir.magnitude < outline)
                {
                    UIManager.instance.Joystick.transform.position = Input.mousePosition;
                }
                else
                {
                    UIManager.instance.Joystick.transform.localPosition = Dir.normalized * outline * 1800;
                }

                //CalDir();

                Dir = UIManager.instance.Joystick.transform.position - UIManager.instance.JoystickBack.transform.position;
                Dir.z = Dir.y;
                Dir.y = 0;

                //dir 0~1 되도록


                Speed += Dir * Accelerate / (outline * 1800);

                if (Speed.magnitude > MaxSpeed)
                {
                    Speed = Speed.normalized * MaxSpeed;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                UIManager.instance.JoystickBack.SetActive(false);
            }
            else
            {
                //마찰력
                Speed *= (1 - Friction);
            }
                       
        }
        else
        {
            //마찰력
            Speed *= (1 - Friction);
        }
        //움직임
        this.transform.Translate(Speed * Time.deltaTime);

        
    }

    private void FireBullet()
    {
        Vector3 dir = Dir.normalized;
        if (dir == Vector3.zero)
        {
            dir = Speed.normalized;
            if(dir == Vector3.zero)
            {
                dir = Random.insideUnitSphere;
                dir.y = 0;
                dir.Normalize();
            }            
        }
        GameObject bullet = GameManager.instance.BulletMake(BulletIndex);
        bullet.tag = "BulletAlly";
        bullet.GetComponent<BulletStatus>().Dir = dir;
        bullet.GetComponent<BulletStatus>().Damage = Damage;
        bullet.transform.position = this.transform.position;
        bullet.SetActive(true);
        GameManager.instance.BulletList.Add(bullet);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            StartCoroutine(WallPush(other));
        }
        else if (other.CompareTag("BulletEnemy"))
        {
            StartCoroutine(BulletHitCor(other));
        }
        else if (other.CompareTag("Player") || other.CompareTag("AI") || other.CompareTag("Obj"))
        {
            if (this.GetComponent<ObjectStatus>() != null)
            {
                StartCoroutine(PushCor(other));
            }
        }
    }
    
    IEnumerator BulletHitCor(Collider other)
    {
        Vector3 BasicContactBackSpeed = (this.transform.position - other.transform.position).normalized * (2 / Friction);
        BasicContactBackSpeed.y = 0;

        Speed = BasicContactBackSpeed;

        PlayerGetDamage(other.GetComponent<BulletStatus>().Damage);

        IsGetHit = true;
        other.gameObject.SetActive(false);        
        yield return new WaitForSeconds(CannotMoveTimer);
        IsGetHit = false;
    }

    IEnumerator PushCor(Collider other)
    {
        Vector3 OtherForce = other.GetComponent<ObjectStatus>().Speed;
        Vector3 BasicContactBackSpeed = (this.transform.position - other.transform.position).normalized * (2 / Friction);
        BasicContactBackSpeed.y = 0;
        yield return null;

        //내가 당하는방식     상대의 속도  +  내가 얼마나 가벼운지
        Speed = OtherForce + BasicContactBackSpeed;
        //CurHP -= other.GetComponent<ObjectStatus>().Damage * OtherForce.magnitude;

        IsGetHit = true;
        yield return new WaitForSeconds(CannotMoveTimer);
        IsGetHit = false;
    }


    IEnumerator WallPush(Collider other)
    {
        if (other.transform.rotation.y == 0) //z opposite
        {
            if(Speed.z>0)
            {
                Speed.z = -Speed.z - (2 / Friction);
            }
            else
            {
                Speed.z = -Speed.z + (2 / Friction);
            }

        }
        else
        {
            if (Speed.x > 0)
            {
                Speed.x = -Speed.x - (2 / Friction);
            }
            else
            {
                Speed.x = -Speed.x + (2 / Friction);
            }
        }

        PlayerGetDamage(10);

        IsGetHit = true;
        yield return new WaitForSeconds(CannotMoveTimer);
        IsGetHit = false;
    }


    public void PlayerGetDamage(float _damage)
    {
        camera.GetComponent<CameraMove>().CameraShake(0.2f, 0.2f);

        CurHP -= _damage;

        if (CurHP <= 0)
        {
            GameManager.instance.Lose();
        }
    }
}
