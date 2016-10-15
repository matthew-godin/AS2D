/*
Missile.cs
----------

By Matthew Godin

Role : Component inheriting from AnimatedSprite
       managing a missile accelerating
       upwards and its explosion

Created : 5 October 2016
Modifie : 15 October 2016
*/
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace XNAProject
{
    /// <summary>
    /// Component inheriting from AnimatedSprite
    /// </summary>
    public class Missile : AnimatedSprite
    {
        //Property initially managed by the constructor
        float DisplacementUpdateInterval { get; set; }
        string ExplosionImageName { get; set; }
        Vector2 ImageExplosionDescription { get; set; }
        Texture2D ExplosionImage { get; set; }
        float TimeSpentSinceDisplacementUpdate { get; set; }

        /// <summary>
        /// Missile constructor
        /// </summary>
        /// <param name="game">Game object</param>
        /// <param name="imageName">Image name (string)</param>
        /// <param name="position">Position (Vector2)</param>
        /// <param name="displayZonee">Display zone (Rectangle)</param>
        /// <param name="imageDescription">Image description (Vector2)</param>
        /// <param name="animationUpdateInterval">Animation update interval (float)</param>
        /// <param name="displacementUpdateInterval">Displacement update interval (float)</param>
        public Missile(Game game, string imageNameMissile, Vector2 position, Rectangle displayZonee, Vector2 imageDescriptionMissile, string imageNameExplosion, Vector2 imageExplosionDescription, float animationUpdateInterval, float displacementUpdateInterval) : base(game, imageNameMissile, position, displayZonee, imageDescriptionMissile, animationUpdateInterval)
        {
            DisplacementUpdateInterval = displacementUpdateInterval;
            ExplosionImageName = imageNameExplosion;
            ImageExplosionDescription = imageExplosionDescription;
        }

        /// <summary>
        /// Initializes Missile components
        /// </summary>
        public override void Initialize()
        {
            LoadContent();
            base.Initialize();
            TimeSpentSinceDisplacementUpdate = NO_TIME_ELAPSED;
        }

        /// <summary>
        /// Loads the content needed by the Missile
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            ExplosionImage = TexturesMgr.Find(ExplosionImageName);
        }

        /// <summary>
        /// Updates the missile
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            TimeSpentSinceDisplacementUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TimeSpentSinceDisplacementUpdate >= DisplacementUpdateInterval)
            {
                PerformDisplacementUpdate();
                TimeSpentSinceDisplacementUpdate = NO_TIME_ELAPSED;
            }
        }

        /// <summary>
        /// Method updating the Missile displacement according to time elapsed
        /// </summary>
        protected virtual void PerformDisplacementUpdate()
        {
            Position -= Vector2.UnitY;
        }
    }
}
