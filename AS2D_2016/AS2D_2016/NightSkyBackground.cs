/* Author :            Raphael Brule
   File :           NightSkyBackground.cs
   Date :              5 October 2016
   Description :       This DrawableGameComponent allows the background
                       to move top to bottom repeatedly.*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class NightSkyBackground : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //Properties initially managed by the constructor
        string ImageName { get; set; }
        float UpdateInterval { get; set; }

        //Properties initially managed by Initialize
        float TimeElapsedSinceUpdate { get; set; }
        Rectangle DisplayZoneFirstImage { get; set; }
        Rectangle DisplayZoneSecondImage { get; set; }

        //Properties initially managed by LoadContent
        SpriteBatch SpriteMgr { get; set; }
        Texture2D BackgroundImage { get; set; }

        /// <summary>
        /// DrawableGameComponent constructor
        /// </summary>
        /// <param name="game">Game object</param>
        /// <param name="imageName">Image name (string)</param>
        /// <param name="updateInterval">Update interval (float)</param>
        public NightSkyBackground(Game game, string imageName, float updateInterval)
            : base(game)
        {
            ImageName = imageName;
            UpdateInterval = updateInterval;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            TimeElapsedSinceUpdate = 0;
            DisplayZoneFirstImage = new Rectangle(0, -Game.Window.ClientBounds.Height,
                            Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            DisplayZoneSecondImage = new Rectangle(0, 0, 
                            Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            base.Initialize();
        }

        /// <summary>
        /// Loads more content
        /// </summary>
        protected override void LoadContent()
        {
            SpriteMgr = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            BackgroundImage = Game.Content.Load<Texture2D>("Textures/" + ImageName);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += timeElapsed;

            if (TimeElapsedSinceUpdate >= UpdateInterval)
            {
                ManageBackgroundImage();

                TimeElapsedSinceUpdate = 0;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Manages the background images
        /// </summary>
        private void ManageBackgroundImage()
        {
            DisplayZoneFirstImage = new Rectangle(0, DisplayZoneFirstImage.Y + 1,
                        Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            DisplayZoneSecondImage = new Rectangle(0, DisplayZoneSecondImage.Y + 1,
                        Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            if (DisplayZoneFirstImage.Y > Game.Window.ClientBounds.Height)
            {
                DisplayZoneFirstImage = new Rectangle(0, -Game.Window.ClientBounds.Height + 1,
                        Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            }

            if (DisplayZoneSecondImage.Y > Game.Window.ClientBounds.Height)
            {
                DisplayZoneSecondImage = new Rectangle(0, -Game.Window.ClientBounds.Height + 1,
                        Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            }
        }

        /// <summary>
        /// Manages the images
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            SpriteMgr.Draw(BackgroundImage, DisplayZoneFirstImage, Color.White);
            SpriteMgr.Draw(BackgroundImage, DisplayZoneSecondImage, Color.White);

            base.Draw(gameTime);
        }
    }
}
