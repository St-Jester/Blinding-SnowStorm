using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float cam_speed = 50f;
    public float screen_boarders = 15f;

    private bool canMove = true;

    // Update is called once per frame
    void LateUpdate () {

        if (Input.GetKey(KeyCode.Escape))//for testing
            canMove = false;

        if (!canMove)
            return;

        if ((Input.GetKey("a") || Input.mousePosition.x <= screen_boarders))//left
        {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
            Quaternion.Euler(new Vector3(0f, -42f,  0f)), 
            cam_speed * Time.deltaTime);
        }
        if ((Input.GetKey("d") || Input.mousePosition.x >= Screen.width - screen_boarders))//right
            {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.Euler(new Vector3(0f, 42f, 0f)), 
                cam_speed * Time.deltaTime);

        }

    }
}
