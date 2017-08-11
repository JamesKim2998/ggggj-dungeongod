using UnityEngine;

public class ItemBox : Trigger
{
    public GameObject box;
    public GameObject item;

    public override void Act()
    {
        // TODO : 박스 사라짐 + 아이템 드롭 애니메이션
        box.SetActive(false);
        item.SetActive(true);
    }
}
