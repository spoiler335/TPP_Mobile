using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Myplayer : MonoBehaviour
{
    public bool mobileInput = false;

    public float moveSpeed = 3f;
    public float smoothRotaionTime = 0.25f;
    float currVelocity;
    float currentSpeed;
    float speedVelocty;
    
    private Animator anim;

    Transform cameraTransform;

    public FixedJoystick joystick;
    public Transform rayOrigin;
    public GameObject crossHairPrefab;

    Vector3 crosshairVel;
    void Start()
    {
        cameraTransform = Camera.main.transform;
        anim = GetComponent<Animator>();
        crossHairPrefab = Instantiate(crossHairPrefab);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }

        Vector2 input=Vector2.zero;
        if (mobileInput)
        {
            input = new Vector2(joystick.input.x, joystick.input.y); 
        }
        else
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        Vector2 inputDir = input.normalized;
        if (inputDir != Vector2.zero)
        {
            float rotation= Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg+cameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref currVelocity,smoothRotaionTime);
        }

        float targetSpeed = moveSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocty, 0.1f);

        if(inputDir.magnitude>0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        transform.Translate(transform.forward *currentSpeed* Time.deltaTime, Space.World);


    }

    private void LateUpdate()
    {
        postionCrossHair();
    }

    void postionCrossHair()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        int layerMask = LayerMask.GetMask("Default");
        if(Physics.Raycast(ray,out hit,100f,layerMask))
        {
            Vector3 start = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
            // crossHairPrefab.transform.position = Vector3.SmoothDamp(crossHairPrefab.transform.position, ray.GetPoint(10), ref crosshairVel, 0.05f);
            crossHairPrefab.transform.position = ray.GetPoint(10);
            crossHairPrefab.transform.LookAt(Camera.main.transform);
        }
    }
    public void Fire()
    {
        anim.SetTrigger("Fire");
        RaycastHit hit;

        if(Physics.Raycast(rayOrigin.position,cameraTransform.forward,out hit,25f))
        {
            print(hit.transform.gameObject.name);
        }

        Debug.DrawRay(rayOrigin.position, cameraTransform.forward * 25f, Color.green);
    }
}
