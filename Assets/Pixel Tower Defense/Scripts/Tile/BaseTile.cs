namespace Assets.Scripts.Tile
{
    /// <summary>
    /// An abstract base tile class
    /// Used as a superclass for Tiles, Towers and Tower Popups
    /// </summary>
    public abstract class BaseTile : BaseGameObject
    {
        /// <summary>
        /// Every base tile should have a ChoicePack linked to it, specifying which items should go inside the 
        /// circular choice menu
        /// </summary>
        public abstract ChoicePack ChoicePack { get; }

        /// <summary>
        /// Every tile should know how to change the tile that was there before it
        /// An example - a tower choice tile that should be replaced with a tower when we chose one
        /// A tower tile will delete the choice tile first
        /// </summary>
        /// <param name="tile">Tile to behave on</param>
        public abstract void BehaveOnCurrentTile(BaseTile tile);
    }
}