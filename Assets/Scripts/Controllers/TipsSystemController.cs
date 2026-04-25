using System.Threading;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using TipsSystem.Interfaces;
using UnityEngine;

namespace TipsSystem
{
    public class TipController : IController
    {
        private readonly ITipView _view;
        private readonly TipStorage _storage;

        private CancellationTokenSource _delayCts;

        public TipController(ITipView view, TipStorage storage)
        {
            _view = view;
            _storage = storage;
        }

        public void ShowTip(string id)
        {
            ShowTip(id, null);
        }

        public void ShowTip(string id, CustomerData data)
        {
            if (!_storage.TryGetTip(id, out var tip))
            {
                Debug.LogWarning($"Tip with id '{id}' not found");
                return;
            }

            string text = ApplyData(tip.Text, data);
            _view.Show(text);
        }

        public void HideTip()
        {
            _view.Hide();
        }

        public void ShowTipDelayed(string id, float delaySeconds, CancellationToken externalCt)
        {
            ShowTipDelayed(id, null, delaySeconds, externalCt);
        }

        public void ShowTipDelayed(
            string id,
            CustomerData data,
            float delaySeconds,
            CancellationToken externalCt)
        {
            CancelDelayedTip();

            _delayCts = CancellationTokenSource.CreateLinkedTokenSource(externalCt);
            ShowTipDelayedAsync(id, data, delaySeconds, _delayCts.Token).Forget();
        }

        public void ShowTipTimed(
            string id,
            float delaySeconds,
            float durationSeconds,
            CancellationToken externalCt)
        {
            ShowTipTimed(id, null, delaySeconds, durationSeconds, externalCt);
        }

        public void ShowTipTimed(
            string id,
            CustomerData data,
            float delaySeconds,
            float durationSeconds,
            CancellationToken externalCt)
        {
            CancelDelayedTip();

            _delayCts = CancellationTokenSource.CreateLinkedTokenSource(externalCt);
            ShowTipTimedAsync(id, data, delaySeconds, durationSeconds, _delayCts.Token).Forget();
        }

        public void CancelDelayedTip()
        {
            if (_delayCts == null)
                return;

            _delayCts.Cancel();
            _delayCts.Dispose();
            _delayCts = null;
        }

        private async UniTaskVoid ShowTipDelayedAsync(
            string id,
            CustomerData data,
            float delaySeconds,
            CancellationToken ct)
        {
            await UniTask.Delay((int)(delaySeconds * 1000), cancellationToken: ct);

            if (ct.IsCancellationRequested)
                return;

            ShowTip(id, data);
        }

        private async UniTaskVoid ShowTipTimedAsync(
            string id,
            CustomerData data,
            float delaySeconds,
            float durationSeconds,
            CancellationToken ct)
        {
            await UniTask.Delay((int)(delaySeconds * 1000), cancellationToken: ct);

            if (ct.IsCancellationRequested)
                return;

            ShowTip(id, data);

            await UniTask.Delay((int)(durationSeconds * 1000), cancellationToken: ct);

            if (ct.IsCancellationRequested)
                return;

            HideTip();
        }

        private string ApplyData(string text, CustomerData data)
        {
            if (data == null)
                return text;

            return text
                .Replace("{pump}", data.petrolPumpNumber.ToString())
                .Replace("{liters}", data.literQuantity.ToString())
                .Replace("{fuel}", data.fuelType);
        }

        public void Dispose()
        {
            CancelDelayedTip();
            HideTip();
        }
    }
}