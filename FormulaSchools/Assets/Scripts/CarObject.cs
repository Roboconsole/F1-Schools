using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Car",menuName ="Cars")]
public class CarObject : ScriptableObject {

    public new string name;
    public Sprite artwork;
    public GameObject carSprite;
    public int CpMin;
    public int CpMax()
    {
        return CpMin + CPWeightPerUpgrade * 30;
    }
    public int GetAmpoule() {


        return PlayerPrefs.GetInt("Ampou," + name, 00);

    }
    public int GetAerodynamics()
    {

        return PlayerPrefs.GetInt("Aero," + name, 0);
    }
    public int GetCarWeight()
    {

        return PlayerPrefs.GetInt("Weight," + name, 0);
    }
    public void SetAmpoule(int i)
    {


         PlayerPrefs.SetInt("Ampou," + name,i);

    }
    public void SetAerodynamics(int i)
    {

         PlayerPrefs.SetInt("Aero," + name,i);
    }
    public void SetCarWeight(int i)
    {

         PlayerPrefs.SetInt("Weight," + name,i);
    }
    public int CPWeightPerUpgrade;
    public int StartingUpgradePrice;
    public int IncreasePriceBy;
    public int CurrentCP() {

        return CpMin+(GetAmpoule()*CPWeightPerUpgrade)+ (GetAerodynamics() * CPWeightPerUpgrade)+ (GetCarWeight() * CPWeightPerUpgrade);

    }
}
