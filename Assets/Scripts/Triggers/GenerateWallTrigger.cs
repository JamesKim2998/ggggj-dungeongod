﻿using UnityEngine;
using System.Collections;

public class GenerateWallTrigger : Trigger
{
    public GameObject wall; //생성되는 벽
    public float gravity;

    public override void Act()
    {
        // TODO : 벽이 떨어지는 애니메이션
        // 만약 벽이 떨어지는 자리에 hero 또는 enemy가 존재하는 경우?
        wall.SetActive(true);
        StartCoroutine(FallDown());
    }
    IEnumerator FallDown()
    {
        float t = 0;
        while (wall.transform.position.y > 1)
        {
            wall.transform.position -= new Vector3(0, gravity * (Mathf.Pow(t, 2) - Mathf.Pow(t - 1 / 60f, 2)), 0);
            t += 1 / 60f;
            yield return new WaitForSeconds(1 / 60f);
        }
        wall.transform.position += new Vector3(0, 1f - wall.transform.position.y, 0);
    }
}