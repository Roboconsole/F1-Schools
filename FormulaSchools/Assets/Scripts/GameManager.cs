using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private fillbar Fillbar;
    public Text InfoText;
    private bool HasStarted;
    private bool CanStart;
    private bool hasStartedCorrectly;
    private bool EnemyStarted;
    public Button LaunchButton;
    public Car Player;
    public Car Enemy;
    public Camera MainCamera;
    public GameObject Finishline;
    public float CameraOffset;
    public int FinishLine = 20;
    bool isPlayerWinning = true;
    public CarObject currentcar;
    private Levels lvl;
    public GameObject PlayingCanvas;
    public GameObject ResultsCanvas;
    public Text Result;
    public Text enemyTimeText;
    public Text playerTimeText;
    public Text MoneyText;
    public Text RestartNextText;
    public float TimeCounter;
    private float EnemyTime;
    private float PlayerTime;
    private bool enemyFinished;
    private bool playerFinished;
    bool gaveCash=false;
    Vector3 GoToEnemy;
    Vector3 GoToPlayer;
    public Material EnemyMat;
	// Use this for initialization
	void Start () {
        EnemyMat.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GoToPlayer = new Vector2(FinishLine + 5+Player.car.transform.position.x, Player.car.transform.position.y);
        GoToEnemy= new Vector2(FinishLine + 5 + Enemy.car.transform.position.x, Enemy.car.transform.position.y);
        currentcar =FindObjectOfType<MenuManager>().currentlySelectedCar;
        Instantiate(currentcar.carSprite, Player.car.transform.position, Quaternion.identity, Player.car.transform);
      //  Player.car.GetComponent<SpriteRenderer>().sprite = currentcar.carSprite;
        Fillbar = FindObjectOfType<fillbar>();
        LaunchButton.GetComponentInChildren<Text>().text = "Launch";
        lvl = FindObjectOfType<MenuManager>().GetCurrentLevel(FindObjectOfType<MenuManager>().levels);
        Enemy.cp = lvl.Cp;
        Instantiate(lvl.CarSprite, Enemy.car.transform.position, Quaternion.identity, Enemy.car.transform);
    }
    public void MainMenu() {


        Application.LoadLevel(0);

    }
	// Update is called once per frame
	void Update () {
        Player.CpText.text = "CP: " + currentcar.CurrentCP();
        Enemy.CpText.text = "CP: " + Enemy.cp;
        Finishline.transform.position = new Vector3(FinishLine, Finishline.transform.position.y,Finishline.transform.position.z);

        if (CanStart)
        {
            TimeCounter += Time.deltaTime;
        }
        if(Player.car.transform.position.x > GoToPlayer.x - 5f&&!playerFinished)
        {
            playerFinished = true;
            PlayerTime = TimeCounter;
        }
        if(Enemy.car.transform.position.x > GoToEnemy.x - 5f && !enemyFinished)
        {
            enemyFinished = true;
            EnemyTime = TimeCounter;
        }
            if (Player.car.transform.position.x < GoToPlayer.x-5f &&Enemy.car.transform.position.x<GoToEnemy.x-5f)
        {
            //if player closer to the end then follow player
            if (Vector2.Distance(Player.car.transform.position, new Vector2(FinishLine, Player.car.transform.position.y)) <= Vector2.Distance(Enemy.car.transform.position, new Vector2(FinishLine, Enemy.car.transform.position.y)))
            {
                //MainCamera.transform.position = new Vector3(Player.car.transform.position.x+CameraOffset, MainCamera.transform.position.y,-10);
                isPlayerWinning = true;
            }
            else
            {
                isPlayerWinning = false;
             //   MainCamera.transform.position = new Vector3(Enemy.car.transform.position.x+CameraOffset, MainCamera.transform.position.y,-10);
            }
        }
        else
        {
            PlayingCanvas.SetActive(false);
            ResultsCanvas.SetActive(true);
            if (isPlayerWinning&&!gaveCash)
            {
                Result.text = "You won";
                lvl.SetFinishdedTrue();
                int money=PlayerPrefs.GetInt("Money",0);
                PlayerPrefs.SetInt("Money", money + lvl.WinningCash);
                gaveCash = true;
                MoneyText.text = "You got: " + lvl.WinningCash + "$";
                RestartNextText.text = "Next";
                
            }
            else if (!gaveCash)
            {
                Result.text = "You lost";
                int money = PlayerPrefs.GetInt("Money", 0);
                PlayerPrefs.SetInt("Money", money + lvl.LoosingCash);
                gaveCash = true;
                RestartNextText.text="Restart";
                MoneyText.text = "You got: " + lvl.LoosingCash + "$";
            }
            if (EnemyTime > 0)
            {
                enemyTimeText.text = "Enemy time: " + EnemyTime.ToString("F2") + "s";
            }
            else
            {
                enemyTimeText.text = "Enemy time: " + TimeCounter.ToString("F2") + "s";
            }
            if (PlayerTime>0)
            {
                playerTimeText.text = "Player time: " + PlayerTime.ToString("F2") + "s";
            }
            else
            {
                playerTimeText.text = "Player time: " + TimeCounter.ToString("F2") + "s";
            }
        }





        if (EnemyStarted)
        {
            UpdateEnemy();
        }
        if (hasStartedCorrectly)
        {
            UpdatePlayer();
        }
	}
    private void UpdatePlayer()
    {
         Player.car.transform.position = Vector3.Lerp(Player.car.transform.position, new Vector3(FinishLine+5, Player.car.transform.position.y,Player.car.transform.position.z),100f/((50000/currentcar.CurrentCP())*(1/Fillbar.SliderValue()) )* Time.deltaTime);
       
    }
    private void UpdateEnemy()
    {

        Enemy.car.transform.position = Vector3.Lerp(Enemy.car.transform.position, new Vector3(FinishLine+5, Enemy.car.transform.position.y,Enemy.car.transform.position.z), 100f/((50000f / Enemy.cp)) *Time.deltaTime);
    }
    public void LaunchStart()
    {
        if (!HasStarted)
        {
        Fillbar.IsGathering = false;
        StartCoroutine(Startting());
        
        }
        else if(CanStart)
        {
            InfoText.text = "";
            hasStartedCorrectly = true;
        }
        else
        {
            InfoText.text = "You failed to start correctly";
            LaunchButton.interactable = false;
        }
    }

    IEnumerator Startting()
    {
        HasStarted = true;
        LaunchButton.GetComponentInChildren<Text>().text = "Start";
        InfoText.text = "Starting in: 3";
        yield return new WaitForSeconds(1);

        InfoText.text = "Starting in: 2";
        yield return new WaitForSeconds(1);
        InfoText.text = "Starting in: 1";
        yield return new WaitForSeconds(1);
        if (LaunchButton.interactable)
        {
            InfoText.text = "START NOW!";
            CanStart = true;
        }
        yield return new WaitForSeconds(0.15f);
        EnemyStarted = true;
    }
    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
[System.Serializable]
public class Car {

    public GameObject car;
	public int cp;
    public Text CpText;
}
