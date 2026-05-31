using System;
using System.Threading;
using Components;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interactables.Interface;
using Services.Interfaces;
using UnityEngine;

namespace Interactables
{
    public class GrillInteractable : MonoBehaviour, IInteractable
    {
        [Header("Grill Points")]
        [SerializeField] private Transform[] grillPoints;

        [Header("Prefabs")]
        [SerializeField] private GameObject cookedSausagePrefab;

        [Header("Components")]
        [SerializeField] private GrabbablesComponent grabbablesComponent;

        [Header("Cooking Settings")]
        [SerializeField] private float cookTime = 3f;

        [SerializeField] private float rotationDuration = 1f;

        [SerializeField] private Vector3 rotationAxis =
            new Vector3(360f, 0f, 0f);

        [Header("Slot Settings")]
        [SerializeField] private float cookedSausageFreeDistance = 0.35f;

        [Header("Audio")]
        [SerializeField] private AudioClip cookSound;

        private IInventoryService _inventoryService;

        private ISoundService _soundService;

        private GrillSlot[] _slots;

        private CancellationTokenSource _destroyCts;

        private void Awake()
        {
            _destroyCts = new CancellationTokenSource();

            InitializeSlots();
        }

        public void Initialize(
            IInventoryService inventoryService,
            ISoundService soundService)
        {
            _inventoryService = inventoryService;

            _soundService = soundService;

            InitializeSlots();
        }

        private void InitializeSlots()
        {
            if (grillPoints == null)
                return;

            if (_slots != null &&
                _slots.Length == grillPoints.Length)
                return;

            _slots = new GrillSlot[grillPoints.Length];

            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i] = new GrillSlot
                {
                    Point = grillPoints[i],
                    IsCooking = false,
                    CurrentRawSausage = null,
                    CurrentCookedSausage = null,
                    RotationTween = null
                };
            }
        }

        public void Interact()
        {
            if (!CanInteract())
                return;

            GrillSlot freeSlot = GetFreeSlot();

            if (freeSlot == null)
                return;

            Grabbable item =
                _inventoryService.GetGrabbableInInventory();

            if (item == null)
                return;

            _inventoryService.RemoveItem(false);

            PlaceSausageOnGrill(item, freeSlot);

            Cook(
                item,
                freeSlot,
                _destroyCts.Token).Forget();
        }

        private void PlaceSausageOnGrill(
            Grabbable item,
            GrillSlot slot)
        {
            if (item == null ||
                slot == null ||
                slot.Point == null)
                return;

            slot.IsCooking = true;

            slot.CurrentRawSausage = item;

            slot.CurrentCookedSausage = null;

            PrepareItemForStaticPoint(item);

            item.transform.SetParent(slot.Point);

            item.transform.localPosition = Vector3.zero;

            item.transform.localRotation = Quaternion.identity;

            slot.RotationTween?.Kill();

            slot.RotationTween = null;

            slot.RotationTween =
                item.transform
                    .DOLocalRotate(
                        rotationAxis,
                        rotationDuration,
                        RotateMode.LocalAxisAdd)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Restart);
        }

        private async UniTaskVoid Cook(
            Grabbable item,
            GrillSlot slot,
            CancellationToken cancellationToken)
        {
            AudioSource audioSource = null;

            try
            {
                if (_soundService != null &&
                    cookSound != null)
                {
                    audioSource =
                        _soundService.Play3DSound(
                            slot.Point.position,
                            cookSound,
                            1f);
                }

                await UniTask.Delay(
                    (int)(cookTime * 1000),
                    cancellationToken: cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return;

                if (this == null)
                    return;

                if (slot == null || slot.Point == null)
                    return;

                slot.RotationTween?.Kill();

                slot.RotationTween = null;

                Vector3 spawnPosition =
                    slot.Point.position;

                Quaternion spawnRotation =
                    slot.Point.rotation;

                if (item != null)
                    Destroy(item.gameObject);

                slot.CurrentRawSausage = null;

                slot.IsCooking = false;

                if (cookedSausagePrefab == null)
                    return;

                GameObject cookedSausage = Instantiate(
                    cookedSausagePrefab,
                    spawnPosition,
                    spawnRotation);

                slot.CurrentCookedSausage =
                    cookedSausage;

                CookedSausageInteractable cookedInteractable =
                    cookedSausage
                        .GetComponent<CookedSausageInteractable>();

                if (cookedInteractable != null)
                {
                    cookedInteractable.Initialize(
                        _inventoryService,
                        grabbablesComponent);
                }

                if (audioSource != null)
                {
                    audioSource.Stop();
                }
            }
            catch (OperationCanceledException)
            {
                if (audioSource != null)
                {
                    audioSource.Stop();
                }
            }
        }

        public bool CanInteract()
        {
            RefreshCookedSausageSlots();

            if (_inventoryService == null)
                return false;

            if (_inventoryService.IsInventoryEmpty())
                return false;

            if (!HasFreeSlot())
                return false;

            Grabbable item =
                _inventoryService.GetGrabbableInInventory();

            if (item == null)
                return false;

            return IsRawSausage(item);
        }

        private void PrepareItemForStaticPoint(
            Grabbable item)
        {
            if (item == null)
                return;

            Rigidbody[] rigidbodies =
                item.GetComponentsInChildren<Rigidbody>(true);

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                rigidbodies[i].linearVelocity =
                    Vector3.zero;

                rigidbodies[i].angularVelocity =
                    Vector3.zero;

                rigidbodies[i].useGravity = false;

                rigidbodies[i].isKinematic = true;
            }

            Collider[] colliders =
                item.GetComponentsInChildren<Collider>(true);

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }
        }

        private void RefreshCookedSausageSlots()
        {
            if (_slots == null)
                return;

            for (int i = 0; i < _slots.Length; i++)
            {
                GrillSlot slot = _slots[i];

                if (slot == null)
                    continue;

                if (slot.IsCooking)
                    continue;

                if (slot.CurrentCookedSausage == null)
                {
                    slot.CurrentCookedSausage = null;
                    continue;
                }

                if (slot.Point == null)
                    continue;

                float distance = Vector3.Distance(
                    slot.CurrentCookedSausage.transform.position,
                    slot.Point.position);

                if (distance > cookedSausageFreeDistance)
                {
                    slot.CurrentCookedSausage = null;
                }
            }
        }

        private bool HasFreeSlot()
        {
            if (_slots == null ||
                _slots.Length == 0)
                return false;

            for (int i = 0; i < _slots.Length; i++)
            {
                if (IsSlotFree(_slots[i]))
                    return true;
            }

            return false;
        }

        private GrillSlot GetFreeSlot()
        {
            RefreshCookedSausageSlots();

            if (_slots == null ||
                _slots.Length == 0)
                return null;

            for (int i = 0; i < _slots.Length; i++)
            {
                if (IsSlotFree(_slots[i]))
                    return _slots[i];
            }

            return null;
        }

        private bool IsSlotFree(GrillSlot slot)
        {
            if (slot == null)
                return false;

            if (slot.Point == null)
                return false;

            if (slot.IsCooking)
                return false;

            if (slot.CurrentRawSausage != null)
                return false;

            if (slot.CurrentCookedSausage != null)
                return false;

            return true;
        }

        private bool IsRawSausage(
            Grabbable grabbable)
        {
            if (grabbable == null)
                return false;

            if (grabbable.TryGetComponent(
                    out FoodItem foodItem))
            {
                return foodItem.Type ==
                       FoodType.RawSausage;
            }

            foodItem =
                grabbable.GetComponentInChildren<FoodItem>();

            if (foodItem != null)
            {
                return foodItem.Type ==
                       FoodType.RawSausage;
            }

            foodItem =
                grabbable.GetComponentInParent<FoodItem>();

            if (foodItem != null)
            {
                return foodItem.Type ==
                       FoodType.RawSausage;
            }

            return false;
        }

        private void OnDestroy()
        {
            if (_destroyCts != null)
            {
                _destroyCts.Cancel();

                _destroyCts.Dispose();

                _destroyCts = null;
            }

            if (_slots == null)
                return;

            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] == null)
                    continue;

                _slots[i].RotationTween?.Kill();

                _slots[i].RotationTween = null;
            }
        }

        private class GrillSlot
        {
            public Transform Point;

            public bool IsCooking;

            public Grabbable CurrentRawSausage;

            public GameObject CurrentCookedSausage;

            public Tween RotationTween;
        }
    }
}