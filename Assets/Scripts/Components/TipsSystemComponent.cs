using Core.Interfaces;
using TipsSystem.Interfaces;
using UnityEngine;

namespace TipsSystem
{
    public class TipComponent : MonoBehaviour, IComponent<TipController>
    {
        [SerializeField] private TipView tipView;
        [SerializeField] private TextAsset tipsXml;

        public TipController CreateController()
        {
            ITipView view = tipView;
            var storage = new TipStorage(tipsXml);

            return new TipController(view, storage);
        }
    }
}