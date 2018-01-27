using UnityEngine;
using System.Collections;

public class MouseRotate : MonoBehaviour
{
    private bool _isMoving = false;
    private bool _isRotating = false;
    private Vector3 _lastMousePosition;

    void Start()
    {

    }

    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) &&
            hitInfo.transform == transform)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isRotating = true;
            }
            if (Input.GetMouseButtonDown(1))
            {
                _isMoving = true;
            }
        }

        if (_isRotating)
        {
            Vector3 mouseDelta = Camera.main.transform.rotation * new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
            transform.Rotate(mouseDelta * 10);
        }

        if (_isMoving)
        {
            Vector3 mouseDelta = Input.mousePosition - _lastMousePosition;
            mouseDelta /= 100f;

            GetComponent<Rigidbody>().MovePosition(transform.position + mouseDelta);
        }

        _lastMousePosition = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            _isRotating = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            _isMoving = false;
        }
    }
}
