/*
Missile.cs
----------

By Matthew Godin

Role : Component inheriting from AnimatedSprite
       managing a missile accelerating
       upwards and its explosion

Created : 5 October 2016
Co-author : Raphael Brule
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
{
    /// <summary>
    /// Component inheriting from AnimatedSprite
    /// </summary>
    public class Missile : AnimatedSprite
    {
        const float SLOW_ANIMATION_INTERVAL = 6 * GameProject.STANDARD_INTERVAL, ImageName = 1 / 4000F, Y_DISPLACEMENT_UPDATE = 4.0F;
        const int BEFORE_EXPLOSION_FIRST_PHASE = 0, EXPLOSION_DIMENSIONS = 40;
        const string EXPLOSION_IMAGE_STRING = "Explosion";
        
        float DisplacementUpdateInterval { get; set; }
        string ExplosionImageName { get; set; }
        Vector2 ImageExplosionDescription { get; set; }
        Texture2D ExplosionImage { get; set; }
        float TimeSpentSinceDisplacementUpdate { get; set; }
        int ExplosionPhase { get; set; }
        Vector2 DisplacementUpdateVector { get; set; }
        bool Collision { get; set; }
        Rectangle ExplosionZone { get; set; }
        AnimatedSprite Explosion { get; set; }
        int ExplosionNumPhases { get; set; }
        public bool ExplosionActivated { get; private set; }

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
            DisplacementUpdateVector = new Vector2(NULL_X, Y_DISPLACEMENT_UPDATE);
            Collision = false;
            ExplosionZone = new Rectangle(NULL_X, NULL_Y, EXPLOSION_DIMENSIONS, EXPLOSION_DIMENSIONS);
        }

        /// <summary>
        /// Updates the missile
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override void Update(GameTime gameTime)
        {
            VerifierExplosionActivated(gameTime);
            VerifyCollision();
        }

        /// <summary>
        /// Checks if ExplosionActivated == true
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        void VerifierExplosionActivated(GameTime gameTime)
        {
            if (ExplosionActivated)
            {
                UpdateExplosion(gameTime);
            }
            else
            {
                UpdateMissile(gameTime);   
            }
        }

        /// <summary>
        /// Called if ExplosionActivated == true
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        void UpdateExplosion(GameTime gameTime)
        {
            TimeSpentSinceDisplacementUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TimeSpentSinceDisplacementUpdate >= SLOW_ANIMATION_INTERVAL)
            {
                TimeSpentSinceDisplacementUpdate = NO_TIME_ELAPSED;
                ManageExplosion();
            }
        }

        /// <summary>
        /// Called if ExplosionActivated == false
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        void UpdateMissile(GameTime gameTime)
        {
            base.Update(gameTime);
            TimeSpentSinceDisplacementUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TimeSpentSinceDisplacementUpdate >= DisplacementUpdateInterval)
            {
                TimeSpentSinceDisplacementUpdate = NO_TIME_ELAPSED;
                PerformDisplacementUpdate();
            }
        }

        /// <summary>
        /// Verifies if Collision == true and creates the explosion if that is the case by putting back Collison = false
        /// </summary>
        void VerifyCollision()
        {
            if (Collision)
            {
                Collision = false;
                Game.Components.Add(Explosion);
            }
        }

        /// <summary>
        /// Method updating the Missile displacement according to time elapsed
        /// </summary>
        protected virtual void PerformDisplacementUpdate()
        {
            Position -= DisplacementUpdateVector;
            DisplacementUpdateInterval -= ImageName;
            ComputeImageRectangleToDisplay();
            VerifyCeilingCollision();
        }

        /// <summary>
        /// Verifies if there is collision with the ceiling and activates explosion if that is the case
        /// </summary>
        void VerifyCeilingCollision()
        {
            if (Position.Y <= TopMargin && !ExplosionActivated)
            {
                ActivateExplosion();
            }
        }
        
        /// <summary>
        /// Called when missile must explode
        /// </summary>
        public void ActivateExplosion()
        {
            Visible = false;
            Explosion = new AnimatedSprite(Game, EXPLOSION_IMAGE_STRING, Position, ExplosionZone, ImageExplosionDescription, SLOW_ANIMATION_INTERVAL);
            ExplosionActivated = true;
            ExplosionPhase = BEFORE_EXPLOSION_FIRST_PHASE;
            Collision = true;
            ExplosionNumPhases = (int)(ImageExplosionDescription.X * ImageExplosionDescription.Y);
        }

        /// <summary>
        /// Manages the explosion of the missile
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        void ManageExplosion()
        {
            ++ExplosionPhase;
            if (ExplosionPhase >= ExplosionNumPhases)
            {
                ExplosionActivated = false;
                Explosion.ToDestroy = true;
                ToDestroy = true;
            }
        }
    }
}