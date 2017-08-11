using UnityEngine;

public class DoorTrigger : Trigger
{
    public bool open; //true시 문열기, false시 문닫기
    public GameObject linkedDoor;

    public override void Act()
    {
        //TODO : 문 여닫는 애니메이션 추가
        linkedDoor.SetActive(open);
        open = !open;
    }
}
