using UnityEngine;

public class God : MonoBehaviour
{
    public float powerLeft;
    public float powerMax;
    public float powerIncreasePerSec;

    public void Update()
    {
        powerLeft = Mathf.Min(powerLeft + powerIncreasePerSec * Time.deltaTime, powerMax);
    }
}