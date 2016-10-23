/*
NightSkyBackground.cs
---------------------

By Matthew Godin

Role : Component containing
       a night sky
       background
Created : 5 October 2016
Co-author : Raphael Brule
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAProject
{
    /// <summary>
    /// Component displaying a background slowly moving from top to bottom
    /// </summary>
    public class NightSkyBackground : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const float NO_TIME_ELAPSED = 0.0F;
        const float NULL_Y = 0.0F;
        const float NULL_X = 0.0F;
        const float Y_INCREMENT = 0.3F;
        const float SCALE = 4.0F / 7.0F;
        const float NO_ANGLE = 0.0F;
        const float NO_LAYER_DEPTH = 0.0F;

        float UpdateInterval { get; set; }
        float TimeElapsedSinceUpdate { get; set; }
        float ScaledImageHeight { get; set; }
        Vector2 IncrementVector { get; set; }
        Vector2 FirstBackgroundPosition { get; set; }
        Vector2 SecondBackgroundPosition { get; set; }
        SpriteBatch SpriteMgr { get; set; }
        string ImageName { get; set; }
        Texture2D BackgroundImage { get; set; }

        /// <summary>
        /// Night sky background constructor
        /// </summary>
        /// <param name="game">Game object</param>
        /// <param name="imageName">String representing background image to display file name</param>
        /// <param name="updateInterval">Update interval by which the background must move</param>
        public NightSkyBackground(Game game, string imageName, float updateInterval) : base(game)
        {
            ImageName = imageName;
            UpdateInterval = updateInterval;
        }

        /// <summary>
        /// Method intializing the different properties of the night sky background
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            TimeElapsedSinceUpdate = NO_TIME_ELAPSED;
            IncrementVector = new Vector2(NULL_X, Y_INCREMENT);
            ScaledImageHeight = BackgroundImage.Height * SCALE;
            ReplaceBackgrounds();
        }

        /// <summary>
        /// Swaps backgrounds indefinitely and moves them down slowly
        /// </summary>
        void ReplaceBackgrounds()
        {
            FirstBackgroundPosition = new Vector2(NULL_X, NULL_Y);
            SecondBackgroundPosition = new Vector2(NULL_X, -ScaledImageHeight);
        }

        /// <summary>
        /// Loads content needed by the night sky background
        /// </summary>
        protected override void LoadContent()
        {
            BackgroundImage = (Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>).Find(ImageName);
            SpriteMgr = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
        }

        /// <summary>
        /// Method updating the content and takes care of time management
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override void Update(GameTime gameTime)
        {
            TimeElapsedSinceUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TimeElapsedSinceUpdate >= UpdateInterval)
            {
                TimeElapsedSinceUpdate = NO_TIME_ELAPSED;
                FirstBackgroundPosition += IncrementVector;
                SecondBackgroundPosition += IncrementVector;
                VerifyIfSwappingNecessary();
            }
        }

        /// <summary>
        /// Swap backgrounds if needed
        /// </summary>
        void VerifyIfSwappingNecessary()
        {
            if (FirstBackgroundPosition.Y >= ScaledImageHeight)
            {
                ReplaceBackgrounds();
            }
        }

        /// <summary>
        /// Draws the two backgrounds
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override void Draw(GameTime gameTime)
        {
            SpriteMgr.Draw(BackgroundImage, FirstBackgroundPosition, null, Color.White, NO_ANGLE, Vector2.Zero, SCALE, SpriteEffects.None, NO_LAYER_DEPTH);
            SpriteMgr.Draw(BackgroundImage, SecondBackgroundPosition, null, Color.White, NO_ANGLE, Vector2.Zero, SCALE, SpriteEffects.None, NO_LAYER_DEPTH);
        }
    }
}
