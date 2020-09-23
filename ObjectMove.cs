using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : ObjectStatus
{
    private void Update()
    {
        //마찰력
        Speed *= (1 - Friction);

        //움직임
        this.transform.Translate(Speed * Time.deltaTime);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!transform.CompareTag("Wall"))
        {
            if (other.CompareTag("Wall"))
            {
                this.gameObject.SetActive(false);
            }

            if (transform.CompareTag("Destroy"))
            {
                if (other.CompareTag("Player") || other.CompareTag("AI") || other.CompareTag("Obj"))
                {
                    //부서지도록
                    this.gameObject.SetActive(false);
                }
                else if(other.CompareTag("BulletAlly") || other.CompareTag("BulletEnemy"))
                {
                    other.gameObject.SetActive(false);
                }
            }
            else
            {
                if (other.CompareTag("Player") || other.CompareTag("AI") || other.CompareTag("Obj"))
                {
                    if (this.GetComponent<ObjectStatus>() != null)
                    {
                        StartCoroutine(PushCor(other));
                    }
                }
                else if (other.CompareTag("BulletAlly") || other.CompareTag("BulletEnemy"))
                {
                    other.gameObject.SetActive(false);
                }
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
        CurHP -= other.GetComponent<ObjectStatus>().Damage * OtherForce.magnitude;

        IsGetHit = true;
        yield return new WaitForSeconds(CannotMoveTimer);
        IsGetHit = false;
    }

}
