using DitzeGames.MobileJoystick;
using System.Collections;
using UnityEngine;

public class Controller : MonoBehaviour {

    //Input
    protected Joystick Joystick;
    protected Button Button;
    protected TouchField TouchField;
    protected Player Player;

    //Parameters
    protected const float RotationSpeed = 10;

    //Camera Controll
    public Vector3 CameraPivot;
    public float CameraDistance;
    protected float InputRotationX;
    protected float InputRotationY;

    protected Vector3 CharacterPivot;
    protected Vector3 LookDirection;

    // Use this for initialization
    void Start ()
    {
        Joystick = FindObjectOfType<Joystick>();
        Button = FindObjectOfType<Button>();
        TouchField = FindObjectOfType<TouchField>();
        Player = FindObjectOfType<Player>();

        TouchField.UseFixedUpdate = true;
    }
	
	// Update is called once per frame
	void FixedUpdate() {

        //input
        InputRotationX = InputRotationX + TouchField.TouchDist.x * RotationSpeed * Time.deltaTime % 360f;
        InputRotationY = Mathf.Clamp(InputRotationY - TouchField.TouchDist.y * RotationSpeed * Time.deltaTime, -88f, 88f);

        //left and forward
        var characterForward = Quaternion.AngleAxis(InputRotationX, Vector3.up) * Vector3.forward;
        var characterLeft = Quaternion.AngleAxis(InputRotationX + 90, Vector3.up) * Vector3.forward;

        //look and run direction
        var runDirection = characterForward * (Input.GetAxisRaw("Vertical") + Joystick.AxisNormalized.y) + characterLeft * (Input.GetAxisRaw("Horizontal") + Joystick.AxisNormalized.x);
        LookDirection = Quaternion.AngleAxis(InputRotationY, characterLeft) * characterForward;

        //set player values
        Player.Input.RunX = runDirection.x;
        Player.Input.RunZ = runDirection.z;
        Player.Input.LookX = LookDirection.x;
        Player.Input.LookZ = LookDirection.z;
        Player.Input.Jump = Button.Pressed || Input.GetButton("Jump");

        CharacterPivot = Quaternion.AngleAxis(InputRotationX, Vector3.up) * CameraPivot;
    }

    private void LateUpdate()
    {
        //set camera values
        Camera.main.transform.position = (transform.position + CharacterPivot) - LookDirection * CameraDistance;
        Camera.main.transform.rotation = Quaternion.LookRotation(LookDirection, Vector3.up);
    }
}
