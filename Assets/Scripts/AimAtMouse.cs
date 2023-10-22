using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtMouse : MonoBehaviour
{
    [SerializeField]
    private float offset;
    // Update is called once per frame
    void Update()
    {
        //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        // Calculate the direction from the player to the mouse cursor
        //Vector2 direction = mousePosition - transform.position;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //angle += offset;
        // Set the rotation of the player/flashlight to face the mouse cursor
        //Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        //transform.rotation = rotation;

        if (PauseMenu.GameIsPaused)
        {

        }
        else 
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            // Calculate the direction from the player to the mouse cursor
            Vector2 direction = mousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += offset;
            // Set the rotation of the player/flashlight to face the mouse cursor
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = rotation;
        }
    }
}
