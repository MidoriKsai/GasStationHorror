using System.Threading;
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;

namespace Services.Interfaces
{
    public interface IPlayerService
    {
        void Initialize(PlayerDataHandler playerDataHandler);

        Transform GetPlayerTransform();

        UniTask FadeInAsync(CancellationToken ct);

        UniTask FadeOutAsync(CancellationToken ct);
    }
}