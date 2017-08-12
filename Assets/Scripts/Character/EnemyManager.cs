using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyController[] enemies;

    private void Start()
    {
        enemies = FindObjectsOfType<EnemyController>();
    }

    public void NextTurn()
    {
        foreach (var enemy in enemies)
        {
            enemy.NextTurn();
        }
        // TODO: AI 코드 실행.
    }
}