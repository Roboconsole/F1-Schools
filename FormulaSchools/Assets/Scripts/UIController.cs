using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    private MenuManager menu;
    [ SerializeField ]
    private Shop shop;
    public Upgrades upgrades;
    public GameObject Car;
    private GameObject CurrentCar;
	// Use this for initialization
	void Start () {
        shop.lookingat = PlayerPrefs.GetInt("CurrentCar", 0);
        menu = FindObjectOfType<MenuManager>();
       CurrentCar=  Instantiate(menu.currentlySelectedCar.carSprite, Car.transform.position, Quaternion.identity, Car.transform);
	}
    public void ChangeCar()
    {
        if (CurrentCar != null)
        {
            Destroy(CurrentCar);
            CurrentCar = Instantiate(menu.currentlySelectedCar.carSprite, Car.transform.position, Quaternion.identity, Car.transform);

        }
    }

	// Update is called once per frame
	void Update () {
        menu = FindObjectOfType<MenuManager>();
        menu.shop = shop;
	}
    public void Right()
    {
        if (shop.lookingat + 1 < menu.Cars.Length)
        {

            
            shop.lookingat++;
            Debug.Log("Right"+shop.lookingat);
        }

    }
    public void Left()
    {

        if (shop.lookingat > 0)
        {
            shop.lookingat--;
        }

    }
    public void select()
    {

        menu.select();

    }
    public void Quit() {

        PlayerPrefs.DeleteAll();

    }

public void LoadGame(){

Application.LoadLevel(1);

}
    public void increaseAero()
    {
        menu.increaseAero();
    }
    public void increaseAmpoule()
    {
        menu.increaseAmpoule();
    }
    public void increaseWeight()
    {
        menu.increaseWeight();
    }
}
