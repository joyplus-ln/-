using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPower : MonoBehaviour
{
    private int currentPower;
    private int normalpower = 20;
    private int levelpower { get; set; }
    private int otherpower { get; set; }

    private int MaxPower
    {
        get { return normalpower + levelpower + otherpower; }
    }
    public Text powerText;
    public Image powerproImage;
    // Use this for initialization
    void Start()
    {

    }

    public void Init()
    {

    }

    public void RefreshPower()
    {
        powerText.text = String.Format("{0}/{1}", MaxPower, currentPower);
        powerproImage.fillAmount = (float)currentPower / MaxPower;
    }
    public void UsePower(int power = 9)
    {
        if (currentPower >= power)
            currentPower -= power;
        RefreshPower();
    }

    public void AddPower(int power = 8)
    {
        currentPower += 8;
        if (currentPower > MaxPower) currentPower = MaxPower;
        RefreshPower();
    }

    public int GetCurrentPower()
    {
        return currentPower;
    }
}
