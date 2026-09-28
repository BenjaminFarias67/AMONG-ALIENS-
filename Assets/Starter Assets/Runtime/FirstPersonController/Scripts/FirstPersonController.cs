using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]

#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif

    public class FirstPersonController : MonoBehaviour
    {
        [Header("Player")]
        public float MoveSpeed = 4.0f;
        public float SprintSpeed = 6.0f;
        public float RotationSpeed = 1.0f;
        public float SpeedChangeRate = 10.0f;

        [Space(10)]
        public float JumpHeight = 1.2f;
        public float Gravity = -20.0f;

        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.5f;
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 90.0f;
        public float BottomClamp = -90.0f;

        private float _cinemachineTargetPitch;
        private float _speed;
        private float _rotationVelocity;
        private float _verticalVelocity;

        private const float TerminalVelocity = 53.0f;
        private const float GroundVelocity = -2.0f;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif

        private CharacterController _controller;
        private StarterAssetsInputs _input;

        private const float Threshold = 0.01f;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput != null &&
                       _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();

#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#endif
        }

        private void Start()
        {
            // Garante que o Character Controller possa detectar colisões.
            _controller.detectCollisions = true;
            _controller.enableOverlapRecovery = true;

            // Começa com uma pequena força para baixo,
            // mantendo o jogador encostado no chão.
            _verticalVelocity = GroundVelocity;

            Grounded = _controller.isGrounded;
        }

        private void Update()
        {
            Move();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void Move()
        {
            // =========================================
            // MOVIMENTO HORIZONTAL
            // =========================================

            float targetSpeed = _input.sprint
                ? SprintSpeed
                : MoveSpeed;

            if (_input.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }

            float currentHorizontalSpeed =
                new Vector3(
                    _controller.velocity.x,
                    0.0f,
                    _controller.velocity.z
                ).magnitude;

            float speedOffset = 0.1f;

            if (currentHorizontalSpeed <
                    targetSpeed - speedOffset ||
                currentHorizontalSpeed >
                    targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(
                    currentHorizontalSpeed,
                    targetSpeed,
                    Time.deltaTime * SpeedChangeRate
                );

                _speed =
                    Mathf.Round(_speed * 1000.0f) / 1000.0f;
            }
            else
            {
                _speed = targetSpeed;
            }

            Vector3 inputDirection = new Vector3(
                _input.move.x,
                0.0f,
                _input.move.y
            ).normalized;

            if (_input.move != Vector2.zero)
            {
                inputDirection =
                    transform.right * _input.move.x +
                    transform.forward * _input.move.y;
            }

            Vector3 horizontalMovement =
                inputDirection.normalized * _speed;

            // =========================================
            // VERIFICAÇÃO DO CHÃO
            // =========================================

            Grounded = _controller.isGrounded;

            // =========================================
            // PULO E GRAVIDADE
            // =========================================

            if (Grounded)
            {
                // Mantém o jogador encostado no chão.
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = GroundVelocity;
                }

                // PULO
                if (_input.jump)
                {
                    _verticalVelocity =
                        Mathf.Sqrt(
                            JumpHeight * -2.0f * Gravity
                        );

                    Grounded = false;

                    // Impede que o mesmo comando
                    // seja usado repetidamente.
                    _input.jump = false;
                }
            }

            // Aplica gravidade quando estiver no ar.
            if (!Grounded)
            {
                if (_verticalVelocity > -TerminalVelocity)
                {
                    _verticalVelocity +=
                        Gravity * Time.deltaTime;
                }
            }

            // =========================================
            // MOVIMENTO VERTICAL
            // =========================================

            Vector3 verticalMovement =
                Vector3.up * _verticalVelocity;

            // =========================================
            // MOVIMENTO FINAL
            // =========================================

            CollisionFlags collisionFlags =
                _controller.Move(
                    (horizontalMovement + verticalMovement)
                    * Time.deltaTime
                );

            // =========================================
            // COLISÃO COM O CHÃO
            // =========================================

            if ((collisionFlags & CollisionFlags.Below) != 0)
            {
                Grounded = true;

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = GroundVelocity;
                }
            }

            // =========================================
            // COLISÃO COM O TETO
            // =========================================

            if ((collisionFlags & CollisionFlags.Above) != 0)
            {
                if (_verticalVelocity > 0.0f)
                {
                    _verticalVelocity = 0.0f;
                }
            }
        }

        private void CameraRotation()
        {
            if (_input == null)
            {
                return;
            }

            if (_input.look.sqrMagnitude >= Threshold)
            {
                float deltaTimeMultiplier =
                    IsCurrentDeviceMouse
                        ? 1.0f
                        : Time.deltaTime;

                _cinemachineTargetPitch +=
                    _input.look.y *
                    RotationSpeed *
                    deltaTimeMultiplier;

                _rotationVelocity =
                    _input.look.x *
                    RotationSpeed *
                    deltaTimeMultiplier;

                _cinemachineTargetPitch =
                    ClampAngle(
                        _cinemachineTargetPitch,
                        BottomClamp,
                        TopClamp
                    );

                if (CinemachineCameraTarget != null)
                {
                    CinemachineCameraTarget
                        .transform
                        .localRotation =
                        Quaternion.Euler(
                            _cinemachineTargetPitch,
                            0.0f,
                            0.0f
                        );
                }

                transform.Rotate(
                    Vector3.up * _rotationVelocity
                );
            }
        }

        private static float ClampAngle(
            float angle,
            float min,
            float max)
        {
            if (angle < -360.0f)
            {
                angle += 360.0f;
            }

            if (angle > 360.0f)
            {
                angle -= 360.0f;
            }

            return Mathf.Clamp(angle, min, max);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color =
                Grounded
                    ? new Color(
                        0.0f,
                        1.0f,
                        0.0f,
                        0.35f)
                    : new Color(
                        1.0f,
                        0.0f,
                        0.0f,
                        0.35f);

            Gizmos.DrawSphere(
                new Vector3(
                    transform.position.x,
                    transform.position.y - GroundedOffset,
                    transform.position.z
                ),
                GroundedRadius
            );
        }
    }
}