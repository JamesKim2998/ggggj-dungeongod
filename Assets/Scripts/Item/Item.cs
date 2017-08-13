using UnityEngine;

public class Item : MonoBehaviour
{
	public Coord coord { get { return Coord.Round(transform.position); } }
}