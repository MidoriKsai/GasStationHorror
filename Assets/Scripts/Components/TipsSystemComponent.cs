using Core.Interfaces;
using TipsSystem.Interfaces;
using UnityEngine;

namespace TipsSystem
{
    public class TipComponent : MonoBehaviour, IComponent<TipController>
    {
        [SerializeField] private TipView wrongActionView;
        [SerializeField] private TipView serviceTipView;
        [SerializeField] private TextAsset tipsXml;
        [SerializeField] private TextAsset infoXml;

        public TipController CreateController()
        {
            var tipsStorage = new TipStorage(tipsXml);
            var infoStorage = new TipStorage(infoXml);

            return new TipController(
                wrongActionView,
                serviceTipView,
                tipsStorage,
                infoStorage);
        }
    }
}