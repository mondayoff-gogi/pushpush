using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : ObjectStatus
{
    public float InputTimer;

    private GameObject Player;
    private const float BackSpeedCosnt = 10;

    private enum Status
    {
        Idle,
        Moving,
        Chase,
        Runaway,
    }
    [SerializeField]
    private Status status;

    private float BulletTimer;

    private void OnEnable()
    {
        BulletTimer = GameManager.instance.Bullets[BulletIndex].GetComponent<BulletStatus>().BulletCooltime;
        CurHP = MaxHP;
        IsGetHit = false;
        status = Status.Moving;
        Player = GameManager.instance.Player;
    }
    // Update is called once per frame
    void Update()
    {
        switch(status)
        {
            case Status.Idle:
                break;
            case Status.Moving:
                status = Status.Idle;
                StopAllCoroutines();
                StartCoroutine(ChaseCor());
                break;
            case Status.Chase:
                status = Status.Idle;
                StopAllCoroutines();
                StartCoroutine(ChaseCor());
                break;
            case Status.Runaway:

                break;
        }

        if(IsGetHit)
        {
            //마찰력
            Speed *= (1 - Friction);
        }        

        //움직임
        this.transform.Translate(Speed * Time.deltaTime);
    }
    private void FireBullet()
    {
        Vector3 dir = Speed.normalized;
        if (dir == Vector3.zero)
        {
            dir = Random.insideUnitSphere;
            dir.y = 0;
            dir.Normalize();
        }

        GameObject bullet = GameManager.instance.BulletMake(BulletIndex);
        bullet.tag = "BulletEnemy";
        bullet.GetComponent<BulletStatus>().Dir = dir;
        bullet.GetComponent<BulletStatus>().Damage = Damage;
        bullet.transform.position = this.transform.position;
        bullet.SetActive(true);
        GameManager.instance.BulletList.Add(bullet);
    }
    IEnumerator ChaseCor()
    {
        float timer = 0;
        float BulletTimerTemp = 0;
        Vector3 Dir;
        while (true)
        {
            Dir = Player.transform.position - this.transform.position;
            Dir.y = 0;
            Dir.Normalize();

            while (timer < InputTimer)
            {
                timer += Time.deltaTime;

                if(!IsGetHit)
                {
                    Speed += Dir * Accelerate;                 

                    if (Speed.magnitude > MaxSpeed)
                    {
                        Speed = Speed.normalized * MaxSpeed;
                    }
                }
                yield return null;

                BulletTimerTemp += Time.deltaTime;

                if(BulletTimer<BulletTimerTemp)
                {
                    BulletTimerTemp = 0;

                    //총알 발사!
                    FireBullet();
                }

            }
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            StartCoroutine(WallPush(other));
        }
        else if (other.CompareTag("BulletAlly"))
        {
            StartCoroutine(BulletHitCor(other));
        }
        else if (other.CompareTag("Player")|| other.CompareTag("AI") || other.CompareTag("Obj"))
        {
            if (this.GetComponent<ObjectStatus>() != null)
            {
                StopAllCoroutines();
                StartCoroutine(PushCor(other));
            }
        }
    }

    IEnumerator PushCor(Collider other)
    {
        Vector3 OtherForce = other.GetComponent<ObjectStatus>().Speed;
        Vector3 BasicContactBackSpeed = (this.transform.position - other.transform.position).normalized * (2 / Friction);
        BasicContactBackSpeed.y = 0;
        yield return null;

        //내가 당하는방식
        Speed = OtherForce + BasicContactBackSpeed;

        EnemyGetDamage(other.GetComponent<ObjectStatus>().Damage);

        IsGetHit = true;
        yield return new WaitForSeconds(CannotMoveTimer);
        IsGetHit = false;
             
        status = Status.Moving;
    }
    IEnumerator BulletHitCor(Collider other)
    {
        Vector3 BasicContactBackSpeed = (this.transform.position - other.transform.position).normalized * (2 / Friction);
        BasicContactBackSpeed.y = 0;

        Speed = BasicContactBackSpeed;

        EnemyGetDamage(other.GetComponent<BulletStatus>().Damage);

        IsGetHit = true;

        other.gameObject.SetActive(false);

        yield return new WaitForSeconds(CannotMoveTimer);
        IsGetHit = false;
    }
    IEnumerator WallPush(Collider other)
    {
        if (other.transform.rotation.y == 0) //z opposite
        {
            if (Speed.z > 0)
            {
                Speed.z = -(2 / Friction) - BackSpeedCosnt;
            }
            else
            {
                Speed.z = (2 / Friction) + BackSpeedCosnt;
            }

        }
        else
        {
            if (Speed.x > 0)
            {
                Speed.x = - (2 / Friction) - BackSpeedCosnt;
            }
            else
            {
                Speed.x = (2 / Friction) + BackSpeedCosnt;
            }
        }

        EnemyGetDamage(10);

        IsGetHit = true;
        yield return new WaitForSeconds(CannotMoveTimer);
        IsGetHit = false;
    }
    public void EnemyGetDamage(float _damage)
    {
        CurHP -= _damage;

        if (CurHP <= 0)
        {            
            GameManager.instance.EnemyOneDead();
            this.gameObject.SetActive(false);
        }
    }

}
