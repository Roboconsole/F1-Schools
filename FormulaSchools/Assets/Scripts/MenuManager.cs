using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public CarObject[] Cars;
    public Levels[] levels;
    public CarObject currentlySelectedCar;
    [HideInInspector]
    public Shop shop;

    private Upgrades upgrades;
    bool isOnMainMenu;
    
    private void Awake()
    {
        MenuManager[] objs = GameObject.FindObjectsOfType<MenuManager>();
        
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        if (Application.loadedLevel == 0)
        {
            isOnMainMenu = true;
            currentlySelectedCar = Cars[PlayerPrefs.GetInt("CurrentCar", 0)];
            upgrades = FindObjectOfType<UIController>().upgrades;
        }
       
    }

    // Use this for initialization
    void Start() {
      
        }

    // Update is called once per frame
    void Update()
    {

        if (Application.loadedLevel == 0)
        {
            isOnMainMenu = true;
            currentlySelectedCar = Cars[PlayerPrefs.GetInt("CurrentCar", 0)];
            upgrades = FindObjectOfType<UIController>().upgrades;
        }
        else
        {
            isOnMainMenu = false;
        }
        if (isOnMainMenu)
        {
            UpdateShop();
            UpdateUpgrades();
        }
    }

    private void UpdateShop()
    {
        shop.CpMax.text = "CP Max: " + Cars[shop.lookingat].CpMax();
        shop.CpMin.text = "CP: " + Cars[shop.lookingat].CurrentCP();
        foreach (Image sp in shop.image)
        {
            sp.sprite = Cars[shop.lookingat].artwork;
        }
        if (currentlySelectedCar == Cars[shop.lookingat])
        {
            shop.Select_Buy.GetComponentInChildren<Text>().text = "Selected";
        }
        else
        {
            shop.Select_Buy.GetComponentInChildren<Text>().text = "Select";
        }
        foreach (Text txt in shop.MoneyTxt)
        {
            txt.text = PlayerPrefs.GetInt("Money", 0).ToString("###,###,###")+"$";
        }
    }
    private void UpdateUpgrades()
    {
        upgrades.AeroText.text = "Aerodynamics: " + currentlySelectedCar.GetAerodynamics() + "/10";
        upgrades.WeightText.text = "Weight: " + currentlySelectedCar.GetCarWeight() + "/10";
        upgrades.AmpouleText.text = "Ampoule: " + currentlySelectedCar.GetAmpoule() + "/10";
        upgrades.AmpouleCash.text = (currentlySelectedCar.StartingUpgradePrice + (currentlySelectedCar.IncreasePriceBy * currentlySelectedCar.GetAmpoule())) + "$";
        upgrades.WeightCash.text = (currentlySelectedCar.StartingUpgradePrice + (currentlySelectedCar.IncreasePriceBy * currentlySelectedCar.GetCarWeight())) + "$";
        upgrades.AeroCash.text = (currentlySelectedCar.StartingUpgradePrice + (currentlySelectedCar.IncreasePriceBy * currentlySelectedCar.GetAerodynamics())) + "$";
        upgrades.CPCurrent.text = "CP: " + currentlySelectedCar.CurrentCP();

    }

    // >
    public void select()
    {
        currentlySelectedCar = Cars[shop.lookingat];
        PlayerPrefs.SetInt("CurrentCar", shop.lookingat);
        FindObjectOfType<UIController>().ChangeCar();
    }


    //upgrades
    public void increaseAero()
    {
        if ((PlayerPrefs.GetInt("Money", 0) - (currentlySelectedCar.StartingUpgradePrice + (currentlySelectedCar.IncreasePriceBy * currentlySelectedCar.GetAerodynamics()))) >= 0)
        {
            if (currentlySelectedCar.GetAerodynamics() < 10)
            {
                currentlySelectedCar.SetAerodynamics(currentlySelectedCar.GetAerodynamics() + 1);
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) - (currentlySelectedCar.StartingUpgradePrice + (currentlySelectedCar.IncreasePriceBy * currentlySelectedCar.GetAerodynamics())));
            }
        }
    }
    public void increaseAmpoule()
    {
        if ((PlayerPrefs.GetInt("Money", 0) - (currentlySelectedCar.StartingUpgradePrice + (currentlySelectedCar.IncreasePriceBy * currentlySelectedCar.GetCarWeight()))) >= 0)
        {
            if (currentlySelectedCar.GetAmpoule() < 10)
            {
                currentlySelectedCar.SetAmpoule(currentlySelectedCar.GetAmpoule() + 1);
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) - (currentlySelectedCar.StartingUpgradePrice + (currentlySelectedCar.IncreasePriceBy * currentlySelectedCar.GetAmpoule())));

            }
        }
    }
    public void increaseWeight()
        {
            if ((PlayerPrefs.GetInt("Money", 0) - (currentlySelectedCar.StartingUpgradePrice + (currentlySelectedCar.IncreasePriceBy * currentlySelectedCar.GetCarWeight())) )>= 0)
            {
                if (currentlySelectedCar.GetCarWeight() < 10)
                {
                    currentlySelectedCar.SetCarWeight(currentlySelectedCar.GetCarWeight() + 1);
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) - (currentlySelectedCar.StartingUpgradePrice + (currentlySelectedCar.IncreasePriceBy * currentlySelectedCar.GetCarWeight())));

            }
        }
        }

    public Levels GetCurrentLevel(Levels[] lvl)
    {
        int i = 0;
        while (i < lvl.Length)
        {
            
                if (!lvl[i].HasBeenFInished())
                {
                    return lvl[i];
                }

                i++;
            
        }
        return lvl[lvl.Length - 1];
    }

}
[System.Serializable]
public class Levels {

    public GameObject CarSprite;
    public int Cp;
    public int WinningCash;
    public int LoosingCash;
    public bool HasBeenFInished()
    {
        return (PlayerPrefs.GetInt("CP Race" + Cp, 0) == 1) ? true : false;
    }
    public void SetFinishdedTrue()
    {
        PlayerPrefs.SetInt("CP Race" + Cp, 1);
    }
}
[System.Serializable]
public class Shop {
    public int lookingat = 0;
    public Image[] image;
    public Text[] MoneyTxt;
    public Text CpMin;
    public Text CpMax;
    public Button Select_Buy;
}
[System.Serializable]
public class Upgrades {

    public Text AeroText;
    public Text AmpouleText;
    public Text WeightText;
    public Text CPCurrent;
    public Text AeroCash;
    public Text AmpouleCash;
    public Text WeightCash;
}