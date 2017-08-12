using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
	DungeonFloor curFloor { get { return MainLogic.instance.dungeon.currentFloor; } }
	PathFinder curPathFinder { get { return curFloor.pathFinder; } }

	protected Hero character;

	public int combatHP = 6;
	int combarHPOnLastTurn;
	public int runAwayDistance = 5;

	ConditionType? nextCondition;
	int countdown = 0;

	GameObject targetToGather;
	Enemy targetToCombat;
	Enemy targetToRunAway;


	void Awake()
	{
		character = GetComponent<Hero>();
	}

	public void TransferToCondition(ConditionType next)
	{
		character.condition = next;

		if (next != ConditionType.GATHER)
			targetToGather = null;
		if (next != ConditionType.COMBAT)
			targetToCombat = null;
		if (next != ConditionType.RUNAWAY)
			targetToRunAway = null;

		switch (next)
		{
			case ConditionType.PARALYZED:
				Paralyzed_OnEnter();
				return;
			case ConditionType.PANIC:
				Panic_OnEnter();
				return;
		}
	}

	public void NextTurn()
	{
		if (nextCondition.HasValue)
		{
			TransferToCondition(nextCondition.Value);
			nextCondition = null;
		}

		switch (character.condition)
		{
			case ConditionType.EXPLORE: Explore_NextTurn(); break;
			case ConditionType.GATHER: Gather_NextTurn(); break;
			case ConditionType.COMBAT: Combat_NextTurn(); break;
			case ConditionType.RUNAWAY: RunAway_NextTurn(); break;
			case ConditionType.PARALYZED: Paralyzed_NextTurn(); break;
			case ConditionType.PANIC: Panic_NextTurn(); break;
			default: break;
		}
	}

	Item GetDirToReachableVisibleItem(out Dir dir)
	{
		foreach (var item in curFloor.EachItem())
		{
			if (item == null) continue;
			var itemCoord = Coord.Round(item.transform.position);
			if (!character.CanSee(itemCoord)) continue;
			var testDir = curFloor.pathFinder.FindPath(character.coord, itemCoord);
			if (testDir == Dir.Stay) continue;
			dir = testDir;
			return item;
		}

		dir = Dir.Stay;
		return null;
	}

	Enemy GetDirToReachableVisibleEnemy(out Dir dir)
	{
		foreach (var enemy in curFloor.EachEnemy())
		{
			if (enemy == null) continue;
			var enemyCoord = Coord.Round(enemy.transform.position);
			if (!character.CanSee(enemyCoord)) continue;
			var testDir = curPathFinder.FindPath(character.coord, enemyCoord);
			if (testDir == Dir.Stay) continue;
			dir = testDir;
			return enemy;
		}

		dir = Dir.Stay;
		return null;
	}

	bool GetDirToNearestReachableFog(out Dir dir)
	{
		var exploreDistance = character.visibleDistance + 10;
		foreach (var testDelta in Range.DistanceNearToFar(exploreDistance))
		{
			var testCoord = testDelta + character.coord;
			var noFog = curFloor.fogOfWar.IsVisibleByHero(testCoord);
			if (noFog) continue;
			var dirToFog = curPathFinder.FindPath(character.coord, testCoord);
			if (dirToFog == Dir.Stay) continue;
			dir = dirToFog;
			return true;
		}
		dir = Dir.Stay;
		return false;
	}

	Dir Explore_NextDir()
	{
		Dir dir;

		// 시야범위 안에 아이템이 떨어져 있으면 획득 상태에 들어간다
		var targetToGather = GetDirToReachableVisibleItem(out dir);
		if (targetToGather)
		{
			nextCondition = ConditionType.GATHER;
			return dir;
		}

		// 시야범위 안에 적이 있고 체력이 n 이상이면 전투 상태에 들어간다
		if (character.HP >= combatHP)
		{
			targetToCombat = GetDirToReachableVisibleEnemy(out dir);
			if (targetToCombat)
			{
				nextCondition = ConditionType.COMBAT;
				return dir;
			}
		}

		// 안개로 덮인 가장 이동거리 가까운 곳을 목표로 이동한다
		if (GetDirToNearestReachableFog(out dir)) return dir;

		return Dir.Stay;
	}

	// 탐색 상태는 영웅 캐릭터의 기본 상태이다
	void Explore_NextTurn()
	{
		var dir = Explore_NextDir();
		if (dir != Dir.Stay)
			character.TryToMove(dir);
		// XXX: 보류.
		// 시야범위 안에 번개가 떨어지면 도주 상태에 들어간다
		// 직접 번개를 맞으면 마비 상태에 들어간다
	}

	// 아이템을 목표로 이동한다
	// 아이템이 없어지거나 획득하면 기본 상태로 돌아간다
	void Gather_NextTurn()
	{
		if (targetToGather == null)
		{
			nextCondition = ConditionType.EXPLORE;
			return;
		}

		var dest = Coord.Round(targetToGather.transform.position);
		var dir = curPathFinder.FindPath(character.coord, dest);
		if (dir == Dir.Stay)
		{
			nextCondition = ConditionType.EXPLORE;
			return;
		}

		character.TryToMove(dir);
	}

	// 전투
	// 적을 목표로 이동한다 (이동방향에 적이 있으면 공격한다)
	// 적이 없어지면 기본 상태로 돌아간다
	// 영웅 캐릭터는 체력이 한번에 2 이상 깎이면 도주 상태에 들어간다
	// 적 캐릭터도 도주 조건이 있을수도?
	// 적 캐릭터는 지정위치에서 일정이상 멀어지면 복귀 상태에 들어간다
	void Combat_NextTurn()
	{
		if (targetToCombat == null)
		{
			nextCondition = ConditionType.EXPLORE;
			return;
		}

		if (character.HP <= combarHPOnLastTurn - 2)
		{
			nextCondition = ConditionType.RUNAWAY;
			targetToRunAway = targetToCombat;
		}

		var targetCoord = Coord.Round(targetToCombat.transform.position);
		var dir = curPathFinder.FindPath(character.coord, targetCoord);
		if (dir == Dir.Stay)
		{
			nextCondition = ConditionType.EXPLORE;
			return;
		}

		character.TryToMove(dir);
	}

	void RunAway_NextTurn()
	{
		if (targetToRunAway == null)
		{
			nextCondition = ConditionType.EXPLORE;
			return;
		}

		countdown--;
		if (countdown <= 0)
		{
			nextCondition = ConditionType.EXPLORE;
			countdown = 0;
		}

		var targetCoord = Coord.Round(targetToRunAway.transform.position);
		if (Coord.distance(targetCoord, character.coord) >= runAwayDistance)
		{
			nextCondition = ConditionType.EXPLORE;
			countdown = 0;
		}

		var dir = curPathFinder.FindPath(character.coord, targetCoord);
		if (dir == Dir.Stay)
		{
			nextCondition = ConditionType.EXPLORE;
			return;
		}

		var testDirs = new[] { dir.Reverse(), dir.Clockwise(), dir.CounterClockwise(), };
		foreach (var testDir in testDirs)
		{
			if (!curFloor.CheckWallExists(character.coord + dir.ToCoord()))
			{
				character.TryToMove(dir.Reverse());
				return;
			}
		}

		character.TryToMove(dir);
	}

	void Paralyzed_OnEnter()
	{
		countdown = Random.Range(1, 3);
	}

	void Paralyzed_NextTurn()
	{
		--countdown;
		if (countdown <= 0)
		{
			nextCondition = ConditionType.EXPLORE;
			countdown = 0;
		}
	}

	void Panic_OnEnter()
	{
		countdown = 4;
	}

	static Dir RandomDir()
	{
		switch (Random.Range(0, 5))
		{
			case 0: return Dir.Down;
			case 1: return Dir.Left;
			case 2: return Dir.Right;
			case 3: return Dir.Up;
			default: return Dir.Stay;
		}
	}

	void Panic_NextTurn()
	{
		if (countdown > 0)
		{
			var dir = RandomDir();
			character.TryToMove(dir);
		}

		--countdown;
		if (countdown <= 0)
		{
			nextCondition = ConditionType.EXPLORE;
			countdown = 0;
		}
	}
}