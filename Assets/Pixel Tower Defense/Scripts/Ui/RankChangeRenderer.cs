using System;
using Assets.Scripts.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ui
{
    /// <summary>
    /// Rank changed renderer
    /// Changes sprites when tower's rank changes
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class RankChangeRenderer : MonoBehaviour
    {
        /// <summary>
        /// All the rank sprites
        /// Be careful, if there are less sprites than tower ranks, it won't be able to change the icon further
        /// </summary>
        public Sprite[] RankSprites;

        /// <summary>
        /// Linked tower. Usually it's our parent (the rank renderer is a child of the tower)
        /// </summary>
        private Tower _linkedTower;

        /// <summary>
        /// Cached SpriteRenderer reference
        /// </summary>
        private SpriteRenderer _rankImage;

        private void Awake()
        {
            _linkedTower = GetComponentInParent<Tower>();

            // We should change our rank when it actually changes
            _linkedTower.RankedHasChangedEvent += SetImageSpriteBasedOnRank;

            _rankImage = GetComponent<SpriteRenderer>();

            SetImageSpriteBasedOnRank(_linkedTower.Rank);
        }

        /// <summary>
        /// Rank changed handler
        /// We change the Rank sprite here
        /// </summary>
        /// <param name="rank">New rank</param>
        private void SetImageSpriteBasedOnRank(int rank)
        {
            // If there will be no OutOfRangeException we change the sprite
            if (rank - 1 < RankSprites.Length)
            {
                _rankImage.sprite = RankSprites[rank - 1];
            }
        }
    }
}