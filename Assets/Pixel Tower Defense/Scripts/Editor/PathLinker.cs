using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    /// <summary>
    /// Path linker utility
    /// Contains logic for linking the tiles together
    /// It starts with the closest tile (close to the BaseTower) and find the closest tile to each consequent tile until it hits the last one
    /// </summary>
    public class PathLinker : EditorWindow
    {
        [MenuItem("GameObject/Pixel Tower Defense/Link path tiles")]
        public static void LinkPathTiles()
        {
            // Finding all the tiles
            PathTile[] tiles = FindObjectsOfType<PathTile>();

            // If we don't have enought tiles, return
            if (tiles.Length < 2)
            {
                Debug.LogError("Not enough path tiles to link");
                return;
            }

            // For every found tile - untag
            foreach (var pathTile in tiles)
            {
                pathTile.tag = Tags.Untagged;
            }

            // Find base tower(s)
            GameObject[] baseTowers = GameObject.FindGameObjectsWithTag(Tags.Base);

            // There should be only one
            if (baseTowers.Length == 0 || baseTowers.Length > 1)
            {
                Debug.LogError("Something's wrong with your base tower. There are " + baseTowers.Length + " base towers");
                return;
            }

            // If there is one, we get it
            var baseTowerTransform = baseTowers[0].GetComponent<Transform>();

            // And sort all the tiles (it also links them there)
            SortTiles(tiles, baseTowerTransform);

            Debug.Log("Path tiles were linked");
        }

        private static void SortTiles(IEnumerable<PathTile> tiles, Transform baseTransform)
        {
            // Cached version of the list of tiles
            var tilesList = tiles.ToList();

            // We find the closest tile to the tower
            PathTile currentlyClosestTile = tiles.OrderBy(tile =>
                (tile.GetComponent<Transform>().position - baseTransform.position).sqrMagnitude)
                .First();

            // And remove it from the list (it will be the first one anyway)
            tilesList.Remove(currentlyClosestTile);

            // Count local field for simplicity
            int count = tilesList.Count;

            // For every tile we find the closest one and link our current tile to this closest one
            for (int i = count; i > 0; i--)
            {
                // Some simple LINQ expressions
                var newClosestTile = tilesList.OrderBy(tile =>
                    (tile.GetComponent<Transform>().position - currentlyClosestTile.GetComponent<Transform>().position)
                        .sqrMagnitude)
                    .First();

                // Linking our new tile to the current one (backward linking)
                newClosestTile.NextTile = currentlyClosestTile.GetComponent<PathTile>();
                currentlyClosestTile = newClosestTile;

                // Remove newly found tile so we won't handle it again
                tilesList.Remove(newClosestTile);

                EditorUtility.SetDirty(currentlyClosestTile);
            }

            // The last tile will be the start
            currentlyClosestTile.tag = Tags.PathStart;
            EditorUtility.SetDirty(currentlyClosestTile);            
        }
    }
}
