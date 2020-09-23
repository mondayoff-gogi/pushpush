using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {

        instance = this;

        GameInit();
    }

    [Header("Init")]
    public MapManager mapManager;
    public GameObject Player;
    public int EnemyCount;

    [Header("Bullets")]
    public GameObject[] Bullets;
    private GameObject[][] BulletsTemp;
    private int[] BulletIndex;
    public GameObject BulletParent;
    private const int Num_Bullet = 20;
    [HideInInspector]
    public List<GameObject> BulletList;

    [Header("Enemy")]
    public GameObject[] Enemys;
    public GameObject[][] EnemysTemp;
    private int[] EnemyIndex;
    public GameObject EnemyParent;
    private const int Num_Enemy = 20;
    [HideInInspector]
    public List<GameObject> EnemyList;

    [Header("UI")]
    public Text scoreText;
    private int myScore;
    private int enemyScore;


    #region 적
    public void EnemyOneDead()
    {
        EnemyCount--;
        if(EnemyCount<=0)
        {
            Win();            
        }
    }
    #endregion

    #region 끝날때
    public void Win()
    {
        myScore++;
        scoreText.text = "<color=#00ff00>" + myScore.ToString() + "</color>:<color=#ff0000>" + enemyScore.ToString() + "</color>";
        SetWin();
    }
    public void Lose()
    {
        enemyScore++;
        scoreText.text = "<color=#00ff00>" + myScore.ToString() + "</color>:<color=#ff0000>" + enemyScore.ToString() + "</color>";
        SetLose();
    }
    #endregion

    #region init
    public void GameInit()
    {        
        myScore = 0;
        enemyScore = 0;

        MakeBullets();
        MakeEnemys();
        EnemyBulletSpawnManage();

        mapManager.MapInitSetting();       
    }

    public void SetWin()
    {
        Destroy(mapManager.mapObjtemp);
        mapManager.MapInitSetting();        
        EnemyBulletSpawnManage();
        UIManager.instance.WinPanelOn();
    }

    public void SetLose()
    {
        Destroy(mapManager.mapObjtemp);
        mapManager.MapInitSetting();
        EnemyBulletSpawnManage();
        Player.transform.position = new Vector3(-10, 1, -10);
        UIManager.instance.CharSelectPanelOn();
    }

    

    private void MakeBullets()
    {
        //bool NeedToMAke = false;

        BulletsTemp = new GameObject[Bullets.Length][];
        BulletIndex = new int[Bullets.Length];
        for (int i = 0; i < BulletIndex.Length; i++)
        {
            //NeedToMAke = false;
            /*
            for (int j = 0; j < 5; j++)
            {
                if (DataManager.instance.MyDeckList[DataManager.instance.MyPresetNum, j] == i)
                {
                    NeedToMAke = true;
                    break;
                }
                if (NeedToMAke) break;
            }
            if (!NeedToMAke) continue;
            */


            BulletsTemp[i] = new GameObject[Num_Bullet];
            BulletIndex[i] = 0;

            for (int j = 0; j < Num_Bullet; j++)
            {
                BulletsTemp[i][j] = Instantiate(Bullets[i], BulletParent.transform);
                BulletsTemp[i][j].SetActive(false);
            }
        }
    }

    public GameObject BulletMake(int index)
    {
        int count = 0;

        BulletIndex[index] = (BulletIndex[index] + 1) % Num_Bullet;
        
        while (count < Num_Bullet)
        {
            count++;

            if (BulletsTemp[index][BulletIndex[index]].activeSelf)
            {
                BulletIndex[index] = (BulletIndex[index] + 1) % Num_Bullet;
            }
            else
                break;
        }
        return BulletsTemp[index][BulletIndex[index]];
    }


    private void MakeEnemys()
    {
        EnemysTemp = new GameObject[Num_Enemy][];
        EnemyIndex = new int[Enemys.Length];

        for (int i = 0; i < EnemyIndex.Length; i++)
        {
            EnemysTemp[i] = new GameObject[Num_Enemy];
            EnemyIndex[i] = 0;

            for (int j = 0; j < Num_Enemy; j++)
            {
                EnemysTemp[i][j] = Instantiate(Enemys[i], EnemyParent.transform);
                EnemysTemp[i][j].SetActive(false);
            }
        }
    }

    private GameObject EnemySpawn(int index)
    {
        int count = 0;

        EnemyIndex[index] = (EnemyIndex[index] + 1) % Num_Bullet;

        while (count < Num_Enemy)
        {
            count++;

            if (EnemysTemp[index][EnemyIndex[index]].activeSelf)
            {
                EnemyIndex[index] = (EnemyIndex[index] + 1) % Num_Enemy;
            }
            else
                break;
        }
        EnemysTemp[index][EnemyIndex[index]].SetActive(true);
        return EnemysTemp[index][EnemyIndex[index]];
    }

    public void EnemyBulletSpawnManage()
    {
        for(int i=0;i<EnemyList.Count;i++)
        {
            EnemyList[i].SetActive(false);
        }
        EnemyList.RemoveRange(0, EnemyList.Count);



        GameObject enemyTemp;
        Vector3 EnemyPos;
        for(int i=0;i<3;i++)
        {
            enemyTemp = EnemySpawn(Random.Range(0,Enemys.Length));
            EnemyPos = new Vector3(Random.Range(-mapManager.MapSizeX[mapManager.mapindex]+ 5, mapManager.MapSizeX[mapManager.mapindex] - 5), 0, Random.Range(-mapManager.MapSizeZ[mapManager.mapindex] + 5, mapManager.MapSizeZ[mapManager.mapindex] - 5));
            while (Vector3.Distance(Player.transform.position, EnemyPos)<5)
            {
                EnemyPos = new Vector3(Random.Range(-mapManager.MapSizeX[mapManager.mapindex] + 5, mapManager.MapSizeX[mapManager.mapindex] - 5), 0, Random.Range(-mapManager.MapSizeZ[mapManager.mapindex] + 5, mapManager.MapSizeZ[mapManager.mapindex] - 5));
            }

            enemyTemp.transform.position = EnemyPos;

            EnemyList.Add(enemyTemp);
        }
        EnemyCount = 3;

        for (int i = 0; i < BulletList.Count; i++)
        {
            BulletList[i].SetActive(false);
        }
        BulletList.RemoveRange(0, BulletList.Count);
    }
    
    #endregion
}
