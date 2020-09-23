using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraMove : MonoBehaviour
{
    public void CameraShake(float timer,float Power)
    {
        transform.DORewind();
        transform.DOShakePosition(timer, Power,5,0);
    }

}
