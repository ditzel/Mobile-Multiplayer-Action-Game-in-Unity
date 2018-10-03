using DitzeGames.MobileJoystick;
using System.Collections;
using UnityEngine;

public class Controller : MonoBehaviour {

    //Input
    protected Joystick Joystick;
    protected Button Button;
    protected TouchField TouchField;

    //Camera Controll
    public Vector3 Pivot;
    public float CameraDistance;
    protected float InputRotationX;
    protected float InputRotationY;


    [HideInInspector]
    public Player Player;
    protected const float RotationSpeed = 10;
    protected float InputAngle;
    protected float CameraAngle;
    protected float CameraUp = 25f;
    protected float CameraSmooth;
    protected float CurrentCameraVelocity;

    // Use this for initialization
    void Start ()
    {
        Joystick = FindObjectOfType<Joystick>();
        Button = FindObjectOfType<Button>();
        TouchField = FindObjectOfType<TouchField>();
        Player = FindObjectOfType<Player>();
    }
	
	// Update is called once per frame
	void Update () {

        //input
        InputRotationX = InputRotationX + TouchField.TouchDist.x * RotationSpeed * Time.fixedDeltaTime % 360f;
        InputRotationY = Mathf.Clamp(InputRotationY - TouchField.TouchDist.y * RotationSpeed * Time.fixedDeltaTime, -88f, 88f);

        //left and forward
        var characterForward = Quaternion.AngleAxis(InputRotationX, Vector3.up) * Vector3.forward;
        var characterLeft = Quaternion.AngleAxis(InputRotationX + 90, Vector3.up) * Vector3.forward;

        //look and run direction
        var runDirection = characterForward * (Input.GetAxis("Vertical") + Joystick.AxisNormalized.y) + characterLeft * (Input.GetAxis("Horizontal") + Joystick.AxisNormalized.x);
        var lookDirection = Quaternion.AngleAxis(InputRotationY, characterLeft) * characterForward;

        //set player values
        Player.Input.RunX = runDirection.x;
        Player.Input.RunZ = runDirection.z;
        Player.Input.LookX = lookDirection.x;
        Player.Input.LookZ = lookDirection.z;
        Player.Input.Jump = Button.Pressed;

        var characterPivot = Quaternion.AngleAxis(InputRotationX, Vector3.up) * Pivot;

        StartCoroutine(setCamera(lookDirection, characterPivot));
    }

    private IEnumerator setCamera(Vector3 lookDirection, Vector3 characterPivot)
    {
        yield return new WaitForFixedUpdate();

        //set camera values
        Camera.main.transform.position = (transform.position + characterPivot) - lookDirection * CameraDistance;
        Camera.main.transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

    }
}
