/*
Sprite.cs
---------

By Matthew Godin

Role : DrawableGameComponent
       inheriting from ICollidable that displays
       a sprite using Texture2D

Created : 5 October 2016
Co-author : Raphael Brule
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAProject
{
    /// <summary>
    /// Sprite class from which most of this project's components inherit from
    /// </summary>
    public class Sprite : Microsoft.Xna.Framework.DrawableGameComponent, ICollidable
    {
        protected const int NULL_Y = 0, NULL_X = 0, NULL_HEIGHT = 0, NULL_WIDTH = 0, HALF_SIZE_DIVISOR = 2;

        SpriteBatch SpriteMgr { get; set; }
        string ImageName { get; set; }
        Texture2D Image { get; set; }
        float Scale { get; set; }
        Rectangle ImageRectangleToDisplay { get; set; }
        protected int RightMargin { get; private set; }
        protected int BottomMargin { get; private set; }
        protected int LeftMargin { get; private set; }
        protected int TopMargin { get; private set; }
        protected Rectangle DisplayZone { get; private set; }
        protected Vector2 SpriteDimensions { get; private set; }
        protected Vector2 ImageDimensions { get; private set; }
        protected Rectangle SourceRectangle { get; set; }
        public Vector2 Position { get; protected set; }

        /// <summary>
        /// Sprite's constructor
        /// </summary>
        /// <param name="game">Game object</param>
        /// <param name="imageName">Image file name</param>
        /// <param name="position">Position where the sprite will be</param>
        /// <param name="displayZone">Display zone in which we put the sprite</param>
        public Sprite(Game game, string imageName, Vector2 position, Rectangle displayZone) : base(game)
        {
            ImageName = imageName;
            Position = position;
            DisplayZone = displayZone;
        }

        /// <summary>
        /// Initializes what is needed by the sprite
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            ImageDimensions = ComputeImageDimensions();
            Scale = ComputeScale();
            SpriteDimensions = ComputeSpriteDimensions();
            ComputeImageRectangleToDisplay();
            SourceRectangle = ComputeSourceRectangle();
            ComputeMargins();
        }

        /// <summary>
        /// Computes rectangle covering what will be displayed
        /// </summary>
        /// <returns>Source Rectangle</returns>
        protected virtual Rectangle ComputeSourceRectangle()
        {
            return new Rectangle(NULL_X, NULL_Y, Image.Width, Image.Height);
        }

        /// <summary>
        /// Computes the image dimensions
        /// </summary>
        /// <returns>Vector2 containing the image dimensions</returns>
        Vector2 ComputeImageDimensions()
        {
            return new Vector2(Image.Width, Image.Height);
        }

        /// <summary>
        /// Computes the rectangle representing what will be displayed
        /// </summary>
        /// <returns>Rectangle representing the perimeter of what will be displayed</returns>
        protected void ComputeImageRectangleToDisplay()
        {
            ImageRectangleToDisplay = new Rectangle((int)Position.X, (int)Position.Y, (int)(SpriteDimensions.X), (int)(SpriteDimensions.Y));
        }

        /// <summary>
        /// Computes the dimensions of the sprite as it will be displayed
        /// </summary>
        /// <returns>A Vector2 representing the dimensions of what will be displayed</returns>
        Vector2 ComputeSpriteDimensions()
        {
            return new Vector2(Scale, Scale) * ComputeOriginalSpriteDimensions();
        }

        /// <summary>
        /// Computes sprite dimensions as seen in its file
        /// </summary>
        /// <returns>Returns the Vector2 containing its dimensions</returns>
        protected virtual Vector2 ComputeOriginalSpriteDimensions()
        {
            return new Vector2(Image.Width, Image.Height);
        }

        /// <summary>
        /// Computes scale by computing horizontal and vertical scale and the taking smallest
        /// </summary>
        /// <returns>The smallest of horizontal and vertical scales</returns>
        protected float ComputeScale()
        {
            float horizontalScale = ComputeHorizontalScale(), verticalScale = ComputeVerticalScale();

            return horizontalScale < verticalScale ? horizontalScale : verticalScale;
        }

        /// <summary>
        /// Computes the horizontal scale of the sprite for the Draw method
        /// </summary>
        protected virtual float ComputeHorizontalScale()
        {
            return DisplayZone.Width / (float)Image.Width;
        }

        /// <summary>
        /// Computes the vertical scale of the sprite for the Draw method
        /// </summary>
        protected virtual float ComputeVerticalScale()
        {
            return DisplayZone.Height / (float)Image.Height;
        }

        /// <summary>
        /// Loads content needed by Sprite
        /// </summary>
        protected override void LoadContent()
        {
            SpriteMgr = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            Image = (Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>).Find(ImageName);
        }

        /// <summary>
        /// Method drawing sprite on the screen
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override sealed void Draw(GameTime gameTime)
        {
            SpriteMgr.Draw(Image, ImageRectangleToDisplay, SourceRectangle, Color.White);
        }

        /// <summary>
        /// True if Sprite is colliding with another object
        /// </summary>
        /// <param name="otherObject">The other object that could be colliding with this Sprite</param>
        /// <returns></returns>
        public bool IsColliding(object otherObject)
        {
            Rectangle otherRectangle = ((AnimatedSprite)otherObject).ImageRectangleToDisplay;

            return ImageRectangleToDisplay.Intersects(otherRectangle);
        }

        /// <summary>
        /// Computes the sprite's margins
        /// </summary>
        protected virtual void ComputeMargins()
        {
            RightMargin = Game.Window.ClientBounds.Width - ImageRectangleToDisplay.Width;
            BottomMargin = Game.Window.ClientBounds.Height - ImageRectangleToDisplay.Height;
            TopMargin = NULL_HEIGHT;
            LeftMargin = NULL_WIDTH;
        }
    }
}
