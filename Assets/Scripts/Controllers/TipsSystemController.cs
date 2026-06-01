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

        private CancellationTokenSource _popupCts;

        private string _currentObjectiveId;

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

        public void ShowObjective(string id)
        {
            _currentObjectiveId = id;

            ShowInternal(
                id,
                null,
                _tipsStorage,
                _tipsView);
        }

        public void HideObjective()
        {
            _currentObjectiveId = null;

            _tipsView.Hide();
        }

        public void ShowPopup(
            string id,
            float duration = 2f)
        {
            ShowPopupInternal(
                id,
                null,
                duration);
        }

        public void ShowPopup(
            string id,
            CustomerData data,
            float duration = 2f)
        {
            ShowPopupInternal(
                id,
                data,
                duration);
        }

        public void ShowInfo(string id)
        {
            ShowInternal(
                id,
                null,
                _infoStorage,
                _infoView);
        }

        public void ShowInfo(
            string id,
            CustomerData data)
        {
            ShowInternal(
                id,
                data,
                _infoStorage,
                _infoView);
        }

        public void HideInfo()
        {
            _infoView.Hide();
        }

        private void ShowPopupInternal(
            string id,
            CustomerData data,
            float duration)
        {
            CancelPopup();

            _popupCts =
                new CancellationTokenSource();

            ShowPopupAsync(
                id,
                data,
                duration,
                _popupCts.Token).Forget();
        }

        private async UniTaskVoid ShowPopupAsync(
            string id,
            CustomerData data,
            float duration,
            CancellationToken ct)
        {
            ShowInternal(
                id,
                data,
                _tipsStorage,
                _tipsView);

            await UniTask.Delay(
                (int)(duration * 1000),
                cancellationToken: ct);

            if (ct.IsCancellationRequested)
                return;

            RestoreObjective();
        }

        private void RestoreObjective()
        {
            if (string.IsNullOrEmpty(
                    _currentObjectiveId))
            {
                _tipsView.Hide();
                return;
            }

            ShowInternal(
                _currentObjectiveId,
                null,
                _tipsStorage,
                _tipsView);
        }

        private void ShowInternal(
            string id,
            CustomerData data,
            TipStorage storage,
            ITipView view)
        {
            if (!storage.TryGetTip(
                    id,
                    out var tip))
            {
                Debug.LogWarning(
                    $"Tip with id '{id}' not found");

                return;
            }

            string text =
                CustomerTextFormatter.ApplyData(
                    tip.Text,
                    data);

            view.Show(text);
        }

        private void CancelPopup()
        {
            if (_popupCts == null)
                return;

            _popupCts.Cancel();

            _popupCts.Dispose();

            _popupCts = null;
        }

        public void Dispose()
        {
            CancelPopup();

            _tipsView.Hide();

            _infoView.Hide();
        }
    }
}