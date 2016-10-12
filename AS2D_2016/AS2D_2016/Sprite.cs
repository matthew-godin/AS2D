/*
Sprite.cs
---------

By Matthew Godin

Role : DrawableGameComponent
       inheriting from ICollidable that displays
       a sprite using Texture2D

Created : 5 October 2016
Modified : 12 October 2016
Description : Now shows scaled and IsColliding is implemented
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
        const int HALF_SIZE_DIVISOR = 2;
        //const float NO_DEPTH_LAYER = 0.0F;
        //const float NO_ROTATION = 0.0F;
        const float NULL_Y = 0.0F;
        const float NULL_X = 0.0F;
        const float NULL_HEIGHT = 0.0F;
        const float NULL_WIDTH = 0.0F;

        string ImageName { get; set; }
        protected Vector2 Position { get; set; }
        Rectangle DisplayZone { get; set; }
        protected SpriteBatch SpriteMgr { get; set; }
        RessourcesManager<Texture2D> TexturesMgr { get; set; }
        protected Texture2D Image { get; set; }
        float Scale { get; set; }
        //Vector2 Origin { get; set; }
        protected Rectangle RectangleImageDimensionsScaled { get; set; }

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
            Scale = ComputeScale();
            //Origin = new Vector2(NULL_X, NULL_Y);
            RectangleImageDimensionsScaled = new Rectangle(DisplayZone.X, DisplayZone.Y, (int)(Image.Width * Scale), (int)(Image.Height * Scale));
        }

        /// <summary>
        /// Computes scale by computing horizontal and vertical scale and the taking smallest
        /// </summary>
        /// <returns>The smallest of horizontal and vertical scales</returns>
        float ComputeScale()
        {
            float horizontalScale = DisplayZone.Width / Image.Width, verticalScale = DisplayZone.Height / Image.Height;

            return horizontalScale < verticalScale ? horizontalScale : verticalScale;
        }

        /// <summary>
        /// Loads content needed by Sprite
        /// </summary>
        protected override void LoadContent()
        {
            SpriteMgr = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            TexturesMgr = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Image = TexturesMgr.Find(ImageName);
        }

        /// <summary>
        /// Method drawing sprite on the screen
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override void Draw(GameTime gameTime)
        {
            //SpriteMgr.Draw(Image, Position, DisplayZone, Color.White, NO_ROTATION, Origin, Scale, SpriteEffects.None, NO_DEPTH_LAYER);

            SpriteMgr.Draw(Image, RectangleImageDimensionsScaled, Color.White);
        }

        /// <summary>
        /// True if Sprite is colliding with another object
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns></returns>
        public bool IsColliding(object otherObject)
        {
            Sprite otherSprite = (Sprite)otherObject;
            Rectangle rectangleCollision = Rectangle.Intersect(RectangleImageDimensionsScaled, otherSprite.RectangleImageDimensionsScaled);

            return rectangleCollision.Width == NULL_WIDTH && rectangleCollision.Height == NULL_HEIGHT;
        }
    }
}
