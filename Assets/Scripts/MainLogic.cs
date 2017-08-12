using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainLogic : MonoBehaviour
{
    public static MainLogic instance = null;

    public float turnDelay;

    public Dungeon dungeon;
    public God god;
    public PathFinder pathFinder;

	public Hero hero;
	public HeroController heroController;

    private RaycastHit[] highLighted;

	public HeroHPBar heroHPBar;
	public GodPowerBar godPowerBar;

    public GameObject thunderPrefab;

    public AudioManager audioManager;

    // public bool isEnemyPhase = false;

	Hero heroPlaceholder;
	HeroController heroControllerPlaceholder;

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
        
		heroPlaceholder = hero;
		heroControllerPlaceholder = heroController;
        
        pathFinder = new PathFinder();
        pathFinder.init();
        InitGame();
		StartCoroutine(CoroutineCycleTurnInfinite());

        audioManager = FindObjectOfType<AudioManager>();

        ItemManager.consumalbeDic.Add(ConsumableItemCode.CAKE, 5);
        ItemManager.consumalbeDic.Add(ConsumableItemCode.CHICKEN, 3);
        ItemManager.consumalbeDic.Add(ConsumableItemCode.PIE, 2);
        ItemManager.consumalbeDic.Add(ConsumableItemCode.WINE, 10);
        ItemManager.consumalbeDic.Add(ConsumableItemCode.SPAGETTI, 6);
        ItemManager.consumalbeDic.Add(ConsumableItemCode.EGGJJIM, 4);

        ItemManager.equipDic.Add(EquipmentCode.ARMOR0, new EquipmentInfo(EquipmentType.ARMOR, 0));
        ItemManager.equipDic.Add(EquipmentCode.ARMOR1, new EquipmentInfo(EquipmentType.ARMOR, 20));
        ItemManager.equipDic.Add(EquipmentCode.ARMOR2, new EquipmentInfo(EquipmentType.ARMOR, 30));
        ItemManager.equipDic.Add(EquipmentCode.ARMOR3, new EquipmentInfo(EquipmentType.ARMOR, 40));

        ItemManager.equipDic.Add(EquipmentCode.WEAPON0, new EquipmentInfo(EquipmentType.WEAPON, 0));
        ItemManager.equipDic.Add(EquipmentCode.WEAPON1, new EquipmentInfo(EquipmentType.WEAPON, 20));
        ItemManager.equipDic.Add(EquipmentCode.WEAPON2, new EquipmentInfo(EquipmentType.WEAPON, 30));
        ItemManager.equipDic.Add(EquipmentCode.WEAPON3, new EquipmentInfo(EquipmentType.WEAPON, 40));

        ItemManager.heroEquipInfo.Add(EquipmentType.ARMOR, EquipmentCode.ARMOR0);
        ItemManager.heroEquipInfo.Add(EquipmentType.WEAPON, EquipmentCode.WEAPON0);
    }

    void Update()
    {
		god.Update();
        deHighLighObject();
        highLightObject();
        if (Input.GetMouseButtonDown(0) && god.powerLeft >= 20)
            UpdateGodTouch();
		UpdateCamera();
		UpdateFogOfWar();
		UpdateUI();
		UpdateDebugInput();

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

	void UpdateCamera()
	{
		// TODO: Debug
		SetCameraXZToCenter(hero.transform.position);
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

	void UpdateDebugInput()
	{
		var inputController = hero.GetComponent<CharacterInputController>();
		if (canControlHeroWithInput)
		{
			if (inputController == null)
				hero.gameObject.AddComponent<CharacterInputController>();
		}
		else
		{
			if (inputController != null)
				Destroy(inputController);
		}
	}

	const string prefKey_CanControlHeroWithInput= "MainLogic_CanControlHeroByInput";
	static bool canControlHeroWithInput
	{
		get { return PlayerPrefs.GetInt(prefKey_CanControlHeroWithInput) == 1; }
		set { PlayerPrefs.SetInt(prefKey_CanControlHeroWithInput, value ? 1 : 0); }
	}

	void OnGUI()
	{
		canControlHeroWithInput = GUILayout.Toggle(canControlHeroWithInput, "ControlHeroByInput");
	}

    void InitGame()
    {
        // Init level
        dungeon.Clear();
		var entrance = dungeon.LoadInitLevel();
		SpawnNewHero(entrance);
		SetCameraXZToCenter(entrance);
		godPowerBar.powerMax = god.powerMax;
    }

	void SpawnNewHero(Vector3 position)
	{
		hero = HeroFactory.InstantiateRandom();
		hero.transform.position = position;
		hero.onHitExit += GoToNextFloor;
		hero.onDead += OnHeroDead;
		// TODO: AI ?�성?�서 ??�?
		heroController = hero.gameObject.AddComponent<HeroController>();
	}

	void SetCameraXZToCenter(Vector3 positionToCenter)
	{
		var cam = Camera.main;
		var camY = cam.transform.position.y;
		var ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1));
		var yRatio = camY / ray.direction.y;
		var xzOffset = new Vector3(ray.direction.x * yRatio, 0, ray.direction.z * yRatio);

		var newPosition = positionToCenter + xzOffset;
		newPosition.y = camY;
		var delta = newPosition - cam.transform.position;
		var camParent= cam.transform.parent;
		camParent.position = camParent.position + delta;
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

	public IEnumerator CoroutineCycleTurnInfinite()
	{
		while (true)
		{
			yield return HeroPhase();
			yield return EnemyPhase();
		}
	}

	IEnumerator HeroPhase()
	{
		if (canControlHeroWithInput)
		{
			yield return null;
		}
		else
		{
			yield return new WaitForSeconds(turnDelay);
			heroController.NextTurn();
		}
	}

    IEnumerator EnemyPhase()
    {
		yield return new WaitForSeconds(turnDelay);
		// Assume that we can ignore times to calculate AI's next action
		var enemies = dungeon.currentFloor.EachEnemy();
		foreach (var enemy in enemies)
			enemy.GetComponent<EnemyController>().NextTurn();
    }

	void GoToNextFloor()
	{
		var entrance = dungeon.GoToNextFloor();
		hero.transform.position = entrance;
	}

	void GoBackToFirstFloor()
	{
		var entrance = dungeon.GoBackToFirstFloor();
		SpawnNewHero(entrance);
		SetCameraXZToCenter(entrance);
	}

    void OnHeroDead()
    {
		hero = heroPlaceholder;
		heroController = heroControllerPlaceholder;
		GoBackToFirstFloor();
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
                StartCoroutine(AudioManager.playSFX(Camera.main.gameObject.AddComponent<AudioSource>(), audioManager.SFXs[5]));
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
                //hero.condition = new Condition(ConditionType.RUNAWAY, 4);
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