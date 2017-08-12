using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainLogic : MonoBehaviour
{
    public static MainLogic instance = null;

    public float turnDelay = 3.0f;

    public int level;
    public List<Dungeon> floors;
    public God god;

    public bool isEnemyPhase = false;

    private List<Enemy> enemies = new List<Enemy>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        InitGame();
    }

    void Update()
    {
		god.Update();
        UpdateGodTouch();

        if (isEnemyPhase)
        {
            StartCoroutine(EnemyPhase());
        }
        else
        {
            //TODO : Hero -> activate AI
            isEnemyPhase = true;
        }
    }

    void InitGame()
    {
        //TODO : Blind activate
        //           "Level 

        //TODO : if we do not generate maps, erase following line
        floors.Clear();
        this.level = 0;
        LoadLevel(this.level);
        //TODO : Instantiate new Hero

        //TODO : Blind deactivate
    }

    void LoadLevel(int level)
    {

        //TODO : Blind activate
        //          "Level #"

        //TODO : if floors.indexOf(level) Exist => activate it
        //          else Generate floor and add to floors.
        //TODO : IF is is special floor like boss floor, load the stage

        GenerateMonsters(level);
        //TODO : generate items especially STAIR TO GO DOWN

        //TODO : Blind deactivate
    }

    void GenerateMonsters(int level)
    {
        enemies.Clear();

        //TODO : generate Monsters.
    }

    IEnumerator EnemyPhase()
    {
        yield return new WaitForSeconds(turnDelay);

        // Assume that we can ignore times to calculate AI's next action
        foreach (Enemy enemy in enemies)
        {
            //TODO :  enemy -> activate AI;
        }
        
        yield return new WaitForSeconds(turnDelay);
    }

    public void GoToNextFloor()
    {
        this.level++;
        LoadLevel(this.level);
    }

    public void GameOver()
    {
        //TODO : Ending process
        //TODO : restart?
    }

    /*
    public Dungeon dungeon;
    public God god;
    public Hero hero;
    public HeroController characterController;
    public EnemyManager enemyManager;

    public void Update()
    {
        TryFloorDown();
        god.Update();
        UpdateGodTouch();
        if (CanTransferToNextTurn())
            TransferToNextTurn();
    }

    public void TryFloorDown()
    {
        var floor = dungeon.currentFloor;
        if (floor.CheckDownStair(hero.coord))
            dungeon.GoFloorDown();
    }

    public bool CanTransferToNextTurn()
    {
        // TODO
        return false;
    }

    public void TransferToNextTurn()
    {
        characterController.NextTurn();
        enemyManager.NextTurn();
    }*/

    // TODO
    public RaycastHit[] TestGodTouch()
    {
        return null;
    }

    public void UpdateGodTouch()
    {
        var hits = TestGodTouch();
		if (hits == null) return;
        foreach (var hit in hits)
        {
            var hitGO = hit.collider.gameObject;
            var godTouch = hitGO.GetComponent<GodTouchAction>();
            if (godTouch != null) godTouch.Act();
        }
    }
    
}