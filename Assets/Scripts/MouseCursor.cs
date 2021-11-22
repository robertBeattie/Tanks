using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    Camera camera;
    private void Awake()
    {
        camera = Camera.main;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = camera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;
    }
}
