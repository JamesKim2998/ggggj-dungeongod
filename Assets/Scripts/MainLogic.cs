using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainLogic : MonoBehaviour
{
    public static MainLogic instance = null;

    public float turnDelay;

    public Dungeon dungeon;
    public God god;

	public Hero hero;
	public HeroController heroController;

    private RaycastHit[] highLighted;

	public HeroHPBar heroHPBar;
	public GodPowerBar godPowerBar;

    public GameObject thunderPrefab;

    // public bool isEnemyPhase = false;

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
        deHighLighObject();
        highLightObject();
        if (Input.GetMouseButtonDown(0) && god.powerLeft >= 20)
            UpdateGodTouch();
		UpdateFogOfWar();
		UpdateUI();

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

	void UpdateFogOfWar()
	{
		var floor = dungeon.currentFloor;
		floor.fogOfWar.UpdateVisibilty(hero, hero.visibleDistance);
	}

	void UpdateUI()
	{
		heroHPBar.SetMaxHP(hero.maxHP, false);
		heroHPBar.SetHP(hero.HP);
		godPowerBar.SetSmooth(god.powerLeft);
	}

    void InitGame()
    {
        // Init level
        dungeon.Clear();
		var entrance = dungeon.LoadInitLevel();

		// Instantiate hero
		hero = HeroFactory.InstantiateRandom();
		hero.transform.position = entrance;
		hero.onHitExit += GoToNextFloor;
		// TODO: AI ?�성?�서 ??�?
		// heroController = hero.gameObject.AddComponent<HeroController>();
		hero.gameObject.AddComponent<CharacterInputController>();

		godPowerBar.powerMax = god.powerMax;
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
        while (true)
        {
            yield return new WaitForSeconds(turnDelay);
            // Assume that we can ignore times to calculate AI's next action
			var enemies = dungeon.currentFloor.EachEnemy();
            foreach (Enemy enemy in enemies)
            {
                enemy.GetComponent<EnemyController>().NextTurn();
            }
        }
    }

    public void GoToNextFloor()
    {
		var entrance = dungeon.GoToNextFloor();
		hero.transform.position = entrance;
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
        return Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
    }

    public void UpdateGodTouch()
    {
        GameObject thunder = null;
        bool hitOnGround = true;

        var hits = TestGodTouch();
		if (hits == null) return;
        bool isThunder = true;
        foreach (var hit in hits)
        {
            if (!dungeon.currentFloor.fogOfWar.IsVisibleByGod(Coord.Round(hit.transform.position)))
                return;
            if (isThunder)
            {
                thunder = Instantiate(thunderPrefab);
                thunder.transform.position = new Vector3(hit.transform.position.x, thunder.transform.position.y, hit.transform.position.z);
                isThunder = false;
                god.powerLeft -= 20;
            }
            var hitGO = hit.collider.gameObject;
            var godTouch = hitGO.GetComponent<GodTouchAction>();
            if (godTouch != null)
            {
                hitOnGround = false;
                godTouch.Act();
            }
        }
        
        if (hitOnGround && thunder != null)
        {
            if (Coord.distance(hero.coord, Coord.Round(thunder.transform.position)) <= hero.visibleDistance)
            {
                Debug.Log("RUN");
                hero.condition = new Condition(ConditionType.RUNAWAY, 4);
            }
        }
        
    }

    public void highLightObject()
    {
        highLighted = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
        foreach (var hit in highLighted)
        {
            hit.transform.GetComponentInChildren<MeshRenderer>().material.color = new Color(0.5f, 0.8f, 0.5f);
        }
    }

    public void deHighLighObject()
    {
        if (highLighted == null)
            return;
        foreach (var hit in highLighted)
        {
            if (hit.transform == null)
                continue;
            hit.transform.GetComponentInChildren<MeshRenderer>().material.color = new Color(1, 1, 1);
        }
    }
}