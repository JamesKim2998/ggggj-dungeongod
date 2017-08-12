using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector2 preMousePos;

    private void Awake()
    {
        preMousePos = Input.mousePosition;
    }
    private void Update()
    {
        //if (Input.mousePosition.x < 100 || Screen.width - Input.mousePosition.x < 100 || Input.mousePosition.y < 100 || Screen.height - Input.mousePosition.y < 100)
        //    Camera.main.transform.position += Vector3.ClampMagnitude(new Vector3(Input.mousePosition.x - Screen.width / 2f, 0, Input.mousePosition.y - Screen.height / 2f),20 * Time.deltaTime);
        //if (Input.GetMouseButton(2))
        //{
        //    Camera.main.transform.position -= Quaternion.Euler(0, -Camera.main.transform.rotation.eulerAngles.z,0) * new Vector3(Input.mousePosition.x - preMousePos.x, 0, Input.mousePosition.y - preMousePos.y) * (Camera.main.fieldOfView / 1200f);
        //}
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            Camera.main.fieldOfView = Mathf.Max(5, Camera.main.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * 20);
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            Camera.main.fieldOfView = Mathf.Min(30, Camera.main.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * 20);
        if (Input.GetMouseButton(1))
        {
            Camera.main.transform.Rotate(0, 0, 360 * (Input.mousePosition.x - preMousePos.x)/ Screen.width);
        }
        preMousePos = Input.mousePosition;
    }
}
