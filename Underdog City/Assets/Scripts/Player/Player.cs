using Photon.Pun;
using UnityEngine;

namespace UnderdogCity
{
    public class Player : MonoBehaviourPun, IPunObservable
    {
        [HideInInspector]
        public InputStr Input;
        public struct InputStr
        {
            public float LookX;
            public float LookZ;
            public float RunX;
            public float RunZ;
            public bool Jump;
        }

        public const float Speed = 10f;
        public const float JumpForce = 5f;

        protected Rigidbody Rigidbody;
        protected Animator Animator;
        protected Quaternion LookRotation;

        protected bool Grounded;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();

            //destroy the controller if the player is not controlled by me
            if (!photonView.IsMine && GetComponent<Controller>() != null)
                Destroy(GetComponent<Controller>());
        }

        private void Update()
        {
            Animator.SetBool("Grounded", Grounded);

            var localVelocity = Quaternion.Inverse(transform.rotation) * (Rigidbody.velocity / Speed);
            Animator.SetFloat("RunX", localVelocity.x);
            Animator.SetFloat("RunZ", localVelocity.z);


        }

        void FixedUpdate()
        {

            var inputRun = Vector3.ClampMagnitude(new Vector3(Input.RunX, 0, Input.RunZ), 1);
            var inputLook = Vector3.ClampMagnitude(new Vector3(Input.LookX, 0, Input.LookZ), 1);

            Rigidbody.velocity = new Vector3(inputRun.x * Speed, Rigidbody.velocity.y, inputRun.z * Speed);

            //rotation to go target
            if (inputLook.magnitude > 0.01f)
                LookRotation = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.forward, inputLook, Vector3.up), Vector3.up);

            transform.rotation = LookRotation;
            Grounded = Physics.OverlapSphere(transform.position, 0.3f, 1).Length > 1;

            if (Input.Jump)
            {
                if (Grounded)
                {
                    Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, JumpForce, Rigidbody.velocity.z);
                }
            }
        }

        private void LateUpdate()
        {
            Animator.transform.localPosition = Vector3.zero;
            Animator.transform.localRotation = Quaternion.identity;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(Input.RunX);
                stream.SendNext(Input.RunZ);
                stream.SendNext(Input.LookX);
                stream.SendNext(Input.LookZ);
            }
            else
            {
                Input.RunX = (float)stream.ReceiveNext();
                Input.RunZ = (float)stream.ReceiveNext();
                Input.LookX = (float)stream.ReceiveNext();
                Input.LookZ = (float)stream.ReceiveNext();
            }
        }
    }
}