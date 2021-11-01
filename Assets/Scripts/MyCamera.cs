using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{

    public float yAxis;
    public float xAxis;
    public float rotationSensityvity=8f;
    public float offset;

    public Transform target;

    float minRotation=-40f;
    float maxRotation=80f;
    public float smoothTime = 0.12f;
    Vector3 targetRotation;
    Vector3 currVelocity;

    public bool mobileInput = false;

    public FixedTouchField touchField;

    
    void Start()
    {
        if (mobileInput) rotationSensityvity = 0.2f;
    }

    void Update()
    {
        if(mobileInput)
        {
            yAxis += touchField.TouchDist.x * rotationSensityvity;
            xAxis -= touchField.TouchDist.y * rotationSensityvity;
        }
        else
        {
            yAxis += Input.GetAxis("Mouse X") * rotationSensityvity;
            xAxis -= Input.GetAxis("Mouse Y") * rotationSensityvity;
        }

        

        xAxis = Mathf.Clamp(xAxis, minRotation, maxRotation);

        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(xAxis, yAxis), ref currVelocity, smoothTime);
        transform.eulerAngles = targetRotation;
        Vector3 _offset = target.position - transform.forward * offset;
        _offset.y = 1.5f;
        transform.position = _offset;
    }
}
