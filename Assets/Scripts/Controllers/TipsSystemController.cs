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
            if (!_storage.TryGetTip(id, out var tip))
            {
                Debug.LogWarning($"Tip with id '{id}' not found");
                return;
            }

            _view.Show(tip.Text);
        }

        public void HideTip()
        {
            _view.Hide();
        }

        public void ShowTipDelayed(string id, float delaySeconds, CancellationToken externalCt)
        {
            CancelDelayedTip();

            _delayCts = CancellationTokenSource.CreateLinkedTokenSource(externalCt);
            ShowTipDelayedAsync(id, delaySeconds, _delayCts.Token).Forget();
        }

        public void CancelDelayedTip()
        {
            if (_delayCts == null)
                return;

            _delayCts.Cancel();
            _delayCts.Dispose();
            _delayCts = null;
        }

        private async UniTaskVoid ShowTipDelayedAsync(string id, float delaySeconds, CancellationToken ct)
        {
            await UniTask.Delay((int)(delaySeconds * 1000), cancellationToken: ct);

            if (ct.IsCancellationRequested)
                return;

            ShowTip(id);
        }

        public void Dispose()
        {
            CancelDelayedTip();
            HideTip();
        }
    }
}