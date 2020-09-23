using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        instance = this;

        charSelectInt = 0;

        SetStatusImage();

        Time.timeScale = 0;
    }
    public GameObject JoystickBack;
    public GameObject Joystick;


    #region FirstPanel
    [Header("CharSelectPanel")]
    public Text charSelectTxt;
    public GameObject charSelectPanel;
    public Image[] StatusImage;
    private int charSelectInt;

    public void CharSelectPanelOn()
    {
        JoystickBack.SetActive(false);
        GameManager.instance.Player.GetComponent<PlayerMove>().enabled = false;
        charSelectPanel.SetActive(true);
        SetStatusImage();
        Time.timeScale = 0;
    }

    public void CharSelectRightBtn()
    {
        if(charSelectInt<DataManager.instance.PlayerDatas.Length-1)
            charSelectInt++;
        charSelectTxt.text = charSelectInt.ToString();
        SetStatusImage();
    }
    public void CharSelectLeftBtn()
    {
        if(charSelectInt>0)
            charSelectInt--;
        charSelectTxt.text = charSelectInt.ToString();
        SetStatusImage();
    }
    public void CharSelectOKBtn()
    {
        GameManager.instance.Player.GetComponent<PlayerMove>().CharIndex = charSelectInt;

        GameManager.instance.Player.GetComponent<PlayerMove>().enabled = true;

        GameManager.instance.Player.GetComponent<PlayerMove>().CharInit();

        Time.timeScale = 1;

        charSelectPanel.SetActive(false);
    }

    private void SetStatusImage()
    {
        StatusImage[0].DOFillAmount(DataManager.instance.PlayerDatas[charSelectInt].MaxSpeed / 20f, 0.2f).SetUpdate(true);
        StatusImage[1].DOFillAmount(DataManager.instance.PlayerDatas[charSelectInt].Accelerate / 400, 0.2f).SetUpdate(true);
        StatusImage[2].DOFillAmount(DataManager.instance.PlayerDatas[charSelectInt].Friction / 1f, 0.2f).SetUpdate(true);
        StatusImage[3].DOFillAmount(DataManager.MaxCannotMoveTimer / DataManager.instance.PlayerDatas[charSelectInt].CannotMoveTimer, 0.2f).SetUpdate(true);
        StatusImage[4].DOFillAmount(DataManager.instance.PlayerDatas[charSelectInt].MaxHP / 100f, 0.2f).SetUpdate(true);
        StatusImage[5].DOFillAmount(DataManager.instance.PlayerDatas[charSelectInt].Damage / 100f, 0.2f).SetUpdate(true);
    }

    #endregion



    #region WinPanel
    [Header("WinPanel")]
    public GameObject WinPanel;
        
    public Text[] RewardText;

    public string[] rewardExample;

    private int[] rewardarrIndex = new int[3];

    public void WinPanelOn()
    {
        JoystickBack.SetActive(false);
        GameManager.instance.Player.GetComponent<PlayerMove>().enabled = false;

        for(int i=0;i< RewardText.Length;i++)
        {
            int rewardIndex = Random.Range(0, rewardExample.Length);

            RewardText[i].text = rewardExample[rewardIndex];

            rewardarrIndex[i] = rewardIndex;
        }

        WinPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RewardBtn(int index)
    {
        //보상
    }

    #endregion
}
