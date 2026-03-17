using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Utils
{
    /// <summary>
    /// Extension class for DOTween'ers
    /// </summary>
    public static class DoTweenExtensions
    {
        /// <summary>
        /// Turn tween object into UniTask and start awaiting
        /// As UniTask has known issue: https://github.com/Cysharp/UniTask/issues/429 so prefer use this method
        /// </summary>
        public static async UniTask AwaitAsync(this Tween tween, CancellationToken cancellationToken)
        {
            if (!tween.IsActive())
            {
                return;
            }

            await using var registration = cancellationToken.Register(() =>
            {
                if (tween.IsActive())
                {
                    tween.Kill();
                }
            });

            await tween.ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken);
        }
    }
}