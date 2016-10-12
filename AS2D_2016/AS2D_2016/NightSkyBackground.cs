/* Author :            Raphael Brule
   File :           NightSkyBackground.cs
   Date :              5 October 2016
   Description :       This DrawableGameComponent allows the background
                       to move top to bottom repeatedly.*/

 
// Co-Author : Matthew Godin
// Modified : 11 October 2016
//Description : The entirety of the code has been modified to make the background
//              functional and not crushed

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
        Vector2 NullOrigin { get; set; }
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
            TimeElapsedSinceUpdate = NO_TIME_ELAPSED;
            NullOrigin = new Vector2(NULL_X, NULL_Y);
            IncrementVector = new Vector2(NULL_X, Y_INCREMENT);
            base.Initialize();
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
            SpriteMgr = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            BackgroundImage = Game.Content.Load<Texture2D>("Textures/" + ImageName);
        }

        /// <summary>
        /// Method updating the content and takes care of time management
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override void Update(GameTime gameTime)
        {
            TimeElapsedSinceUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
            VerifierSiIncrementationNecessaire();
        }

        /// <summary>
        /// Checks if enough time has elapsed to move or even swap the backgrounds
        /// </summary>
        void VerifierSiIncrementationNecessaire()
        {
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
            SpriteMgr.Draw(BackgroundImage, FirstBackgroundPosition, null, Color.White, NO_ANGLE, NullOrigin, SCALE, SpriteEffects.None, NO_LAYER_DEPTH);
            SpriteMgr.Draw(BackgroundImage, SecondBackgroundPosition, null, Color.White, NO_ANGLE, NullOrigin, SCALE, SpriteEffects.None, NO_LAYER_DEPTH);
        }
    }
}
