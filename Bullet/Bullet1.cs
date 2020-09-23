using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : BulletStatus
{    
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(BulletDeactive());        
    }
    IEnumerator BulletDeactive()
    {
        yield return new WaitForSeconds(BulletLifeTime);

        this.gameObject.SetActive(false);
    }
}
