using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public InputStr Input;
    public struct InputStr
    {
        public float LookX;
        public float LookZ;
        public float RunX;
        public float RunZ;
    }

    public const float Speed = 10f;
    public const float JumpForce = 7f;

    public Rigidbody Rigidbody;

    [HideInInspector]
    public Quaternion LookRotation;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        var inputRun = new Vector3(Input.RunX, 0, Input.RunZ);
        if (inputRun.magnitude > 1)
            inputRun.Normalize();
        var inputLook = new Vector3(Input.LookX, 0, Input.LookZ);
        if (inputLook.magnitude > 1)
            inputLook.Normalize();

        Rigidbody.velocity = new Vector3(inputRun.x * Speed, Rigidbody.velocity.y, inputRun.z * Speed);

        //rotation to go target
        if (inputLook.magnitude > 0.01f)
            LookRotation = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.forward, inputLook, Vector3.up), Vector3.up);

        transform.rotation = LookRotation;
    }
}
