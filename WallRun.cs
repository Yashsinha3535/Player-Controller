using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{

    [Header("LayerMask And Transform")]
    public Transform orientation;
    public LayerMask runnableWall;

    Rigidbody rb;

    [Header("Detection")]
    public float wallRunDistance;
    public Playermovement pm;

    [Header("Movement")]
    public float wallRunForwardSpeed;
    public float wallStickiness;
    public float wallSwitchForce;

    [Header("Tilt")]
    public float tilt;
    float normalTilt;
    

    public float tiltTime;



    bool wallLeft;
    bool wallRight;
    bool isWallRunning = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //WallRun
        CheckForWall();

        if (wallLeft && !pm.isGrounded)
        {
            Debug.Log("Left");
            StartWallRun();
        }
        else if (wallRight && !pm.isGrounded)
        {
            Debug.Log("Right");
            StartWallRun();
        }
        else
        {
            StopWallRun();
        }

    }


    void CheckForWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, wallRunDistance, runnableWall);
        wallRight = Physics.Raycast(transform.position, orientation.right, wallRunDistance, runnableWall);
    }

    void StartWallRun()
    {
        rb.useGravity = false;
        isWallRunning = true;

        

        //Always add forward force
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(orientation.forward * wallRunForwardSpeed * Time.deltaTime, ForceMode.VelocityChange);
            rb.AddForce(-orientation.up * 2f, ForceMode.Force);
            
        }
        else
        {
            StopWallRun();
        }

        //Add side force according to the side
        if (wallLeft)
        {
            rb.AddForce(-orientation.right * wallStickiness * Time.deltaTime, ForceMode.VelocityChange);
        }
        else if (wallRight)
        {
            rb.AddForce(orientation.right * wallStickiness * Time.deltaTime, ForceMode.VelocityChange);
        }


        //Jumping or wall Switch
        if (Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.D) && wallLeft)
            {
                rb.AddForce(orientation.right * wallSwitchForce * Time.deltaTime,ForceMode.VelocityChange);
                rb.AddForce(orientation.up * 1.5f, ForceMode.VelocityChange);
                rb.useGravity = false;
            }
            else if(Input.GetKey(KeyCode.A) && wallRight)
            {
                rb.AddForce(-orientation.right * wallSwitchForce * Time.deltaTime, ForceMode.VelocityChange);
                rb.AddForce(orientation.up * 1.5f,ForceMode.VelocityChange);
                rb.useGravity = false;
            }

            else if(wallLeft || wallRight)
            {
                rb.AddForce(orientation.up * pm.jumpForce);
                if (wallLeft)
                {
                    rb.AddForce(orientation.right * 1f * Time.deltaTime,ForceMode.VelocityChange);
                }
                else if (wallRight)
                {
                    rb.AddForce(-orientation.right * 1f * Time.deltaTime, ForceMode.VelocityChange);
                }
            }

        }

        //tilt System
        if(wallRight){
            tilt = Mathf.Lerp(tilt,100,tiltTime * Time.deltaTime);
        }
        else if(wallLeft){
            tilt = Mathf.Lerp(tilt,-100,tiltTime * Time.deltaTime);
        }
 


    }


    void StopWallRun()
    {
        rb.useGravity = true;
        isWallRunning = false;

        tilt = 0f;
    }

}
