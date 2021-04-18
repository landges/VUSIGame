using UnityEngine;

public class Zoom : MonoBehaviour
{
    Vector3 touchStart;
    public float maxZoom;
    public float minZoom;
    public float sensitivity;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            ZoomCamera(difference * 0.01f);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            GetComponent<Camera>().transform.position += direction;
        }
        ZoomCamera(Input.GetAxis("Mouse ScrollWheel"));
    }
    void ZoomCamera(float increment)
    {
        GetComponent<Camera>().orthographicSize = Mathf.Clamp(GetComponent<Camera>().orthographicSize - increment * sensitivity, minZoom, maxZoom);
    }
}