using System.Threading;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using TipsSystem.Interfaces;
using UnityEngine;

namespace TipsSystem
{
    public class TipController : IController
    {
        private readonly ITipView _tipsView;
        private readonly ITipView _infoView;

        private readonly TipStorage _tipsStorage;
        private readonly TipStorage _infoStorage;

        private CancellationTokenSource _delayCts;

        public TipController(
            ITipView tipsView,
            ITipView infoView,
            TipStorage tipsStorage,
            TipStorage infoStorage)
        {
            _tipsView = tipsView;
            _infoView = infoView;

            _tipsStorage = tipsStorage;
            _infoStorage = infoStorage;
        }

        public void ShowTip(string id)
        {
            ShowInternal(id, null, _tipsStorage, _tipsView);
        }

        public void HideTip()
        {
            _tipsView.Hide();
        }
        
        public void ShowInfo(string id)
        {
            ShowInternal(id, null, _tipsStorage, _tipsView);
        }

        public void ShowInfo(string id, CustomerData data)
        {
            ShowInternal(id, data, _infoStorage, _infoView);
        }

        public void HideInfo()
        {
            _infoView.Hide();
        }

        public void ShowTipTimed(string id, float delay, float duration, CancellationToken ct)
        {
            ShowTimedInternal(id, null, delay, duration, ct, _tipsStorage, _tipsView);
        }
        
        public void ShowInfoTimed(string id, float delay, float duration, CancellationToken ct)
        {
            ShowTimedInternal(id, null, delay, duration, ct, _infoStorage, _infoView);
        }

        public void ShowInfoTimed(string id, CustomerData data, float delay, float duration, CancellationToken ct)
        {
            ShowTimedInternal(id, data, delay, duration, ct, _infoStorage, _infoView);
        }

        private void ShowInternal(
            string id,
            CustomerData data,
            TipStorage storage,
            ITipView view)
        {
            if (!storage.TryGetTip(id, out var tip))
            {
                Debug.LogWarning($"Tip with id '{id}' not found");
                return;
            }

            string text = CustomerTextFormatter.ApplyData(tip.Text, data);
            view.Show(text);
        }

        private void ShowTimedInternal(
            string id,
            CustomerData data,
            float delay,
            float duration,
            CancellationToken externalCt,
            TipStorage storage,
            ITipView view)
        {
            CancelDelayed();

            _delayCts = CancellationTokenSource.CreateLinkedTokenSource(externalCt);
            ShowTimedAsync(id, data, delay, duration, _delayCts.Token, storage, view).Forget();
        }

        private async UniTaskVoid ShowTimedAsync(
            string id,
            CustomerData data,
            float delay,
            float duration,
            CancellationToken ct,
            TipStorage storage,
            ITipView view)
        {
            await UniTask.Delay((int)(delay * 1000), cancellationToken: ct);
            if (ct.IsCancellationRequested) return;

            ShowInternal(id, data, storage, view);

            await UniTask.Delay((int)(duration * 1000), cancellationToken: ct);
            if (ct.IsCancellationRequested) return;

            view.Hide();
        }

        private void CancelDelayed()
        {
            if (_delayCts == null)
                return;

            _delayCts.Cancel();
            _delayCts.Dispose();
            _delayCts = null;
        }
        
        public void Dispose()
        {
            CancelDelayed();
            _tipsView.Hide();
            _infoView.Hide();
        }
    }
}