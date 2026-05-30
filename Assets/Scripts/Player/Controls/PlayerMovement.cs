using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private float movementSpeed;
    private Camera playerCamera;
    private InputHandler inputHandler;
    private Vector3 moveDirection;
    private bool _isMovementEnabled = true;
    private Vector3 cameraDefaultPosition;
    private float frequency;
    private float amplitude;
    private float localMovementTime;
    private AudioSource footstepsAudioSource;
    private CancellationTokenSource footstepsAudioCts;

    public void Initialize(
        Rigidbody playerRigidbody, 
        float movementSpeed,
        Camera playerCamera, 
        InputHandler inputHandler,
        float CameraJitterFrequency,
        float CameraJitterAmplitude,
        AudioSource footstepsAudioSource)
    {
        this.playerRigidbody = playerRigidbody;
        this.movementSpeed = movementSpeed;
        this.playerCamera = playerCamera;
        this.inputHandler = inputHandler;
        cameraDefaultPosition = playerCamera.transform.localPosition;
        frequency = CameraJitterFrequency;
        amplitude = CameraJitterAmplitude;
        this.footstepsAudioSource = footstepsAudioSource;

        footstepsAudioCts = new CancellationTokenSource();
        PlayFootstepsAudioAsync(0.5f, footstepsAudioCts.Token).Forget();
    }

    void Update()
    {
        if (!_isMovementEnabled)
        {
            moveDirection = Vector3.zero;
            return;
        }
        
        float horizontal = inputHandler.horizontalInput;
        float vertical = inputHandler.verticalInput;

        Vector2 localMovement = new Vector2(horizontal, vertical).normalized;

        moveDirection = transform.right * localMovement.x + transform.forward * localMovement.y;
    }

    void FixedUpdate()
    {
        if (IsMoving())
        {
            ApplySineCosineForCamera();
            localMovementTime += Time.deltaTime;
            playerRigidbody.linearVelocity = moveDirection * movementSpeed;
        }
        else
        {
            localMovementTime = 0;
            playerRigidbody.linearVelocity = Vector3.zero;
            DefaultCamera();
        }
    }

    private bool IsMoving()
        => inputHandler.horizontalInput != 0 || inputHandler.verticalInput != 0;

    private void ApplySineCosineForCamera()
    {
        float xShift = Mathf.Cos(localMovementTime * frequency * 0.5f) * amplitude;
        float yShift = Mathf.Sin(-1 * localMovementTime * frequency) * amplitude;

        Vector3 newCameraPosition = new Vector3(
            cameraDefaultPosition.x + xShift, 
            cameraDefaultPosition.y + yShift, 
            cameraDefaultPosition.z
        );

        playerCamera.transform.localPosition = Vector3.Lerp(
            playerCamera.transform.localPosition, 
            newCameraPosition, 
            Time.deltaTime * 10f
        );
    }

    private void DefaultCamera()
    {
        playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, cameraDefaultPosition, Time.deltaTime * 10f);
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        _isMovementEnabled = isEnabled;

        if (!isEnabled)
        {
            moveDirection = Vector3.zero;
            playerRigidbody.linearVelocity = new Vector3(0f, playerRigidbody.linearVelocity.y, 0f);
        }
    }

    private async UniTask PlayFootstepsAudioAsync(float delay, CancellationToken ct)
    {
        while (true)
        {
            if(ct.IsCancellationRequested)
            {
                return;
            }

            if (!IsMoving())
            {
                await UniTask.Yield(ct);
                continue;
            }

            footstepsAudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            footstepsAudioSource.Play();
            Debug.Log("Playing footsteps audio");

            await UniTask.Delay(TimeSpan.FromSeconds(delay));
        }
    }

    void OnDestroy()
    {
        footstepsAudioCts?.Cancel();
        footstepsAudioCts?.Dispose();
        footstepsAudioCts = null;
    }
}
