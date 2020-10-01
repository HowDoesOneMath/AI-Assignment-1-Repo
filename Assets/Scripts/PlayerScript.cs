using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The player, in all its glory.
[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    public Transform mainCam;
    public Controller mainController;
    public UnityEngine.UI.Image cursorImage;
    public float speed = 5;
    public float reach = 3;

    Vector2 properAngles = Vector2.zero;

    Rigidbody rb;
    Color originalCursorColor;

    public static PlayerScript mainPlayer = null;

    //Initializing certain data, such as setting the main player and locking the cursor.
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainPlayer = this;
        originalCursorColor = cursorImage.color;

        Cursor.lockState = CursorLockMode.Locked;
    }

    //Movement code.
    private void FixedUpdate()
    {
        Vector3 move = mainController.movement.GetDirectionalInput();

        //Capping the speed
        if (move.sqrMagnitude > 0)
        {
            move = move.normalized;
        }

        Vector3 dir = transform.rotation * move;

        //Directly set velocity.
        rb.velocity = dir * speed;
    }

    //Controls rotation of the camera, as well as general-purpose controls.
    private void Update()
    {
        if (mainController.quit.tapped)
        {
            Application.Quit();
            return;
        }

        //reset the entire level
        if (mainController.recreate.tapped)
        {
            SpawnTheBeanz.mainBeanSpawner.Recreate();
        }

        //Rotate camera
        Vector3 delta = mainController.rotate.GetDirectionalInput();

        properAngles += new Vector2(-delta.y, delta.x);

        properAngles.x = Mathf.Clamp(properAngles.x, -90f, 90f);

        properAngles.y = properAngles.y % 360f;

        mainCam.transform.localRotation = Quaternion.Euler(properAngles.x, 0, 0);
        transform.localRotation = Quaternion.Euler(0, properAngles.y, 0);

        //Detonate beans, if they're in range and the detonate button is pressed.
        //Raycasts every frame are inefficient, but necessary to edit cursor color.
        cursorImage.color = originalCursorColor;

        RaycastHit rch;

        if (Physics.Raycast(mainCam.position, mainCam.forward, out rch, reach))
        {
            BeanScript bs = null;

            if (rch.collider.TryGetComponent(out bs))
            {
                cursorImage.color = Color.red;

                if (mainController.detonate.tapped)
                {
                    bs.PushButton();
                }
            }
        }
    }
}
