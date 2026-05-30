using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Footsteps")]
    [SerializeField] private float footstepsDelay = 0.5f;

    [SerializeField] private Vector2 footstepsPitchRange =
        new Vector2(0.8f, 1.2f);

    private Rigidbody playerRigidbody;

    private float movementSpeed;

    private Camera playerCamera;

    private InputHandler inputHandler;

    private AudioSource footstepsAudioSource;

    private Vector3 moveDirection;

    private bool _isMovementEnabled = true;

    private Vector3 cameraDefaultPosition;

    private float frequency;

    private float amplitude;

    private float localMovementTime;

    private CancellationTokenSource footstepsAudioCts;

    public void Initialize(
        Rigidbody playerRigidbody,
        float movementSpeed,
        Camera playerCamera,
        InputHandler inputHandler,
        float cameraJitterFrequency,
        float cameraJitterAmplitude,
        AudioSource footstepsAudioSource)
    {
        this.playerRigidbody = playerRigidbody;

        this.movementSpeed = movementSpeed;

        this.playerCamera = playerCamera;

        this.inputHandler = inputHandler;

        this.frequency = cameraJitterFrequency;

        this.amplitude = cameraJitterAmplitude;

        this.footstepsAudioSource = footstepsAudioSource;

        cameraDefaultPosition =
            playerCamera.transform.localPosition;

        footstepsAudioCts =
            new CancellationTokenSource();

        PlayFootstepsAudioAsync(
            footstepsDelay,
            footstepsAudioCts.Token).Forget();
    }

    private void Update()
    {
        if (!_isMovementEnabled)
        {
            moveDirection = Vector3.zero;
            return;
        }

        float horizontal =
            inputHandler.horizontalInput;

        float vertical =
            inputHandler.verticalInput;

        Vector2 localMovement =
            new Vector2(horizontal, vertical).normalized;

        moveDirection =
            transform.right * localMovement.x +
            transform.forward * localMovement.y;
    }

    private void FixedUpdate()
    {
        if (!_isMovementEnabled)
        {
            playerRigidbody.linearVelocity =
                Vector3.zero;

            localMovementTime = 0;

            DefaultCamera();

            return;
        }

        if (IsMoving())
        {
            ApplySineCosineForCamera();

            localMovementTime += Time.deltaTime;

            playerRigidbody.linearVelocity =
                moveDirection * movementSpeed;
        }
        else
        {
            localMovementTime = 0;

            playerRigidbody.linearVelocity =
                Vector3.zero;

            DefaultCamera();
        }
    }

    private bool IsMoving()
    {
        return _isMovementEnabled &&
               (inputHandler.horizontalInput != 0 ||
                inputHandler.verticalInput != 0);
    }

    private void ApplySineCosineForCamera()
    {
        float xShift =
            Mathf.Cos(
                localMovementTime *
                frequency *
                0.5f) * amplitude;

        float yShift =
            Mathf.Sin(
                -1 *
                localMovementTime *
                frequency) * amplitude;

        Vector3 newCameraPosition =
            new Vector3(
                cameraDefaultPosition.x + xShift,
                cameraDefaultPosition.y + yShift,
                cameraDefaultPosition.z
            );

        playerCamera.transform.localPosition =
            Vector3.Lerp(
                playerCamera.transform.localPosition,
                newCameraPosition,
                Time.deltaTime * 10f
            );
    }

    private void DefaultCamera()
    {
        playerCamera.transform.localPosition =
            Vector3.Lerp(
                playerCamera.transform.localPosition,
                cameraDefaultPosition,
                Time.deltaTime * 10f
            );
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _isMovementEnabled = isEnabled;

        if (!isEnabled)
        {
            moveDirection = Vector3.zero;

            playerRigidbody.linearVelocity =
                Vector3.zero;

            localMovementTime = 0;

            DefaultCamera();
        }
    }

    private async UniTaskVoid PlayFootstepsAudioAsync(
        float delay,
        CancellationToken ct)
    {
        while (true)
        {
            if (ct.IsCancellationRequested)
                return;

            if (!_isMovementEnabled || !IsMoving())
            {
                await UniTask.Yield(ct);
                continue;
            }

            if (footstepsAudioSource != null)
            {
                footstepsAudioSource.pitch =
                    UnityEngine.Random.Range(
                        footstepsPitchRange.x,
                        footstepsPitchRange.y);

                footstepsAudioSource.Play();
            }

            await UniTask.Delay(
                TimeSpan.FromSeconds(delay),
                cancellationToken: ct);
        }
    }

    private void OnDestroy()
    {
        footstepsAudioCts?.Cancel();

        footstepsAudioCts?.Dispose();

        footstepsAudioCts = null;
    }
}