using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet0 : BulletStatus
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


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Speed * Dir * Time.deltaTime);
    }
}
