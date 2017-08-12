using UnityEngine;
using System.Collections;

public class ItemBox : Trigger
{
    public GameObject box;
    public GameObject item;
    public bool isActive = true;

    public override void Act()
    {
        if (!isActive)
            return;
        // TODO : 박스 사라짐 + 아이템 드롭 애니메이션
        StartCoroutine(FadeOut());
        item.SetActive(true);
    }

    IEnumerator FadeOut()
    {
        for (float i = 1; i >= 0; i -= 0.04f) {
            box.GetComponent<MeshRenderer>().material.color = new Color (1,1,1,i);
            yield return new WaitForSeconds(1 / 60f);
        }
        box.SetActive(false);
    }
}
