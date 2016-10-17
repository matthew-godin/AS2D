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
        const float SLOW_ANIMATION_INTERVAL = 6 * GameProject.STANDARD_INTERVAL;
        //Property initially managed by the constructor
        float DisplacementUpdateInterval { get; set; }
        string ExplosionImageName { get; set; }
        Vector2 ImageExplosionDescription { get; set; }
        Texture2D ExplosionImage { get; set; }
        float TimeSpentSinceDisplacementUpdate { get; set; }
        public bool ExplosionActivated { get; private set; }
        float TimeSpentSinceUpdateExplosion { get; set; }
        int ExplosionPhase { get; set; }
        AnimatedSprite Explosion { get; set; }
        //bool ExplosionDone { get; set; }


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
            ExplosionActivated = false;
            //ExplosionDone = false;
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
                PerformDisplacementUpdate(gameTime);
                TimeSpentSinceDisplacementUpdate = NO_TIME_ELAPSED;
            }
        }

        /// <summary>
        /// Method updating the Missile displacement according to time elapsed
        /// </summary>
        protected virtual void PerformDisplacementUpdate(GameTime gameTime)
        {
            Position -= Vector2.UnitY;
            if (Position.Y <= TopMargin && !ExplosionActivated /*&& !ExplosionDone*/)
            {
                ActivateExplosionMissile();
                ManageExplosion(gameTime);
            }
            /*if (ExplosionDone)
            {
                for (int i = Game.Components.Count - 1; i >= 0; --i)
                {
                    if (Game.Components[i] is IDestructible && ((IDestructible)Game.Components[i]).ToDestroy)
                    {
                        Game.Components.RemoveAt(i);
                    }
                }
            }*/
        }

        /// <summary>
        /// Called when missile must explode
        /// </summary>
        public void ActivateExplosion()
        {
            ToDestroy = true;//ESSENTIAL LINE!!!
        }

        /// <summary>
        /// Activates the Missile's explosion
        /// </summary>
        void ActivateExplosionMissile()
        {
            Explosion = new AnimatedSprite(Game, "Explosion", Position, DisplayZone, ImageExplosionDescription, SLOW_ANIMATION_INTERVAL);
            Game.Components.Add(Explosion);
            ExplosionActivated = true;
            TimeSpentSinceUpdateExplosion = NO_TIME_ELAPSED;
        }

        /// <summary>
        /// Manages the explosion of the missile
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        private void ManageExplosion(GameTime gameTime)
        {
            float TimeElapased = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeSpentSinceUpdateExplosion += TimeElapased;
            if (TimeSpentSinceUpdateExplosion >= SLOW_ANIMATION_INTERVAL)
            {
                ++ExplosionPhase;
                TimeSpentSinceUpdateExplosion = NO_TIME_ELAPSED;
                if (ExplosionPhase >= ImageExplosionDescription.X * ImageExplosionDescription.Y)
                {
                    ExplosionActivated = false;
                    Explosion.ToDestroy = true;
                    //ExplosionDone = true;
                }
            }
        }
    }
}