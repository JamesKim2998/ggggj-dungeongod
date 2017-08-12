using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour {

    private void Awake()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        for (float i = 1; i>=0; i -= 0.1f)
        {
            GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, i);
            GetComponentInChildren<Light>().intensity = 15 * i;
            yield return new WaitForSeconds(1 / 60f);
        }
        Destroy(this.gameObject);
    }
}
