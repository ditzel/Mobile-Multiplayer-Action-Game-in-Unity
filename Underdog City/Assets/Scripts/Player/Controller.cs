using DitzeGames.MobileJoystick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderdogCity
{

    public class Controller : MonoBehaviour
    {

        //Input
        protected Joystick Joystick;
        protected Button JumpButton;
        protected Button ShootButton;
        protected Button CarButton;
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

        protected Vector3 CameraVelocity;

        // Use this for initialization
        void Start()
        {
            Joystick = FindObjectOfType<Joystick>();
            var buttons = new List<Button>(FindObjectsOfType<Button>());
            JumpButton = buttons.Find(b => b.gameObject.name == "btnJump");
            ShootButton = buttons.Find(b => b.gameObject.name == "btnShoot");
            CarButton = buttons.Find(b => b.gameObject.name == "btnCar");
            TouchField = FindObjectOfType<TouchField>();
            Player = GetComponent<Player>();

            TouchField.UseFixedUpdate = true;
        }

        // Update is called once per frame
        void FixedUpdate()
        {

            //input
            InputRotationX = InputRotationX + TouchField.TouchDist.x * RotationSpeed * Time.deltaTime % 360f;
            InputRotationY = Mathf.Clamp(InputRotationY - TouchField.TouchDist.y * RotationSpeed * Time.deltaTime, -88f, 88f);

            //left and forward
            var characterForward = Quaternion.AngleAxis(InputRotationX, Vector3.up) * Vector3.forward;
            var characterLeft = Quaternion.AngleAxis(InputRotationX + 90, Vector3.up) * Vector3.forward;

            //look and run direction
            var runDirection = characterForward * (Input.GetAxisRaw("Vertical") + Joystick.AxisNormalized.y) + characterLeft * (Input.GetAxisRaw("Horizontal") + Joystick.AxisNormalized.x);
            LookDirection = Quaternion.AngleAxis(InputRotationY, characterLeft) * characterForward;

            switch (Player.State)
            {
                case Player.PlayerState.NORMAL:

                    //set player values
                    Player.Input.RunX = runDirection.x;
                    Player.Input.RunZ = runDirection.z;
                    Player.Input.LookX = LookDirection.x;
                    Player.Input.LookZ = LookDirection.z;
                    Player.Input.Jump = JumpButton.Pressed || Input.GetButton("Jump");

                    if (ShootButton.Pressed)
                    {
                        ShootButton.Pressed = false;
                        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
                        RaycastHit hitInfo;
                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            var player = hitInfo.collider.GetComponent<Player>();
                            if (player != null)
                                player.OnHit(ray.direction);
                        }
                    }

                    CharacterPivot = Quaternion.AngleAxis(InputRotationX, Vector3.up) * CameraPivot;

                    //car button visibility
                    CarButton.gameObject.SetActive(Player.NearestCar != null);

                    break;

                case Player.PlayerState.IN_CAR:

                    //set car values
                    Player.NearestCar.CarPhysics.Input.Forward = Input.GetAxisRaw("Vertical") + Joystick.InputVector.y;
                    Player.NearestCar.CarPhysics.Input.Steer = Input.GetAxisRaw("Horizontal") + Joystick.InputVector.x;

                    //car button visibility
                    CarButton.gameObject.SetActive(true);

                    var target = Player.NearestCar.transform.position + (Player.NearestCar.transform.forward * -1.5f + Player.NearestCar.transform.up) * CameraDistance;
                    Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, target, ref CameraVelocity, 0.3f);
                    Camera.main.transform.LookAt(Player.NearestCar.transform.position + Player.NearestCar.transform.forward * 1.5f * CameraDistance, Vector3.up);
                    break;
            }

            if (CarButton.Pressed)
                Player.EnterCar();
        }

        private void LateUpdate()
        {
            //set camera values
            switch (Player.State)
            {
                case Player.PlayerState.NORMAL:
                case Player.PlayerState.TRANSITION:
                    Camera.main.transform.position = (transform.position + CharacterPivot) - LookDirection * CameraDistance;
                    Camera.main.transform.rotation = Quaternion.LookRotation(LookDirection, Vector3.up);
                    break;

                case Player.PlayerState.IN_CAR:
                    break;
            }
            }
    }
}