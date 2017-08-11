using UnityEngine;

public class GenerateWallTrigger : Trigger
{
    public GameObject wall; //생성되는 벽

    public override void Act()
    {
        // TODO : 벽이 떨어지는 애니메이션
        // 만약 벽이 떨어지는 자리에 hero 또는 enemy가 존재하는 경우?
        wall.SetActive(true);
    }
}