using Assets.Utils;
using UnityEngine;

namespace Assets.Scripts.Tile
{
    public class Tile : BaseTile
    {
        public override ChoicePack ChoicePack
        {
            get { return TowerChoicePreferences.Instance.ChosenChoiceItems; }
        }

        public override void BehaveOnCurrentTile(BaseTile tile)
        {
            Instantiate(this, tile.TransformCache.position, Quaternion.identity);
        }
    }
}