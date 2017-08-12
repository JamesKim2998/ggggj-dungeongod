using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainLogic : MonoBehaviour
{
    public static MainLogic instance = null;

    public float turnDelay = 3.0f;

    public Dungeon dungeon;
    public God god;

	public Hero hero;
	public HeroController heroController;

    // public bool isEnemyPhase = false;
    private List<Enemy> enemies {
		get { return dungeon.currentFloor.enemies; }
	}

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
		StartCoroutine(HeroPhase());
		StartCoroutine(EnemyPhase());
    }

    void Update()
    {
		god.Update();
        UpdateGodTouch();

		/*
        if (isEnemyPhase)
        {
        }
        else
        {
            //TODO : Hero -> activate AI
            isEnemyPhase = true;
        }
		*/
    }

    void InitGame()
    {
        //TODO : Blind activate
        //           "Level 

        // Init level
        dungeon.Clear();
		dungeon.LoadInitLevel();

		// Instantiate hero
		hero = HeroFactory.InstantiateRandom();
		hero.onHitExit += GoToNextFloor;
		// heroController = hero.gameObject.AddComponent<HeroController>();
		hero.gameObject.AddComponent<CharacterInputController>();

        //TODO : Blind deactivate
    }

	/*
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
	*/

	IEnumerator HeroPhase()
	{
		while (true)
		{
			yield return new WaitForSeconds(turnDelay);
			heroController.NextTurn();
		}
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
		dungeon.GoToNextFloor();
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