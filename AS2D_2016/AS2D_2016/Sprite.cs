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
        //const float NO_DEPTH_LAYER = 0.0F;
        //const float NO_ROTATION = 0.0F;
        protected const int NULL_Y = 0, NULL_X = 0, NULL_HEIGHT = 0, NULL_WIDTH = 0, HALF_SIZE_DIVISOR = 2;

        string ImageName { get; set; }
        public Vector2 Position { get; protected set; }
        protected Rectangle DisplayZone { get; set; }
        protected SpriteBatch SpriteMgr { get; set; }
        protected RessourcesManager<Texture2D> TexturesMgr { get; set; }
        /* probably private */ protected Texture2D Image { get; set; }
        protected float Scale { get; set; }
        //Vector2 Origin { get; set; }
        protected Rectangle RectangleImageDimensionsScaled { get; set; }
        protected int RightMargin { get; set; }
        protected int BottomMargin { get; set; }
        protected int LeftMargin { get; set; }
        protected int TopMargin { get; set; }

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
            RectangleImageDimensionsScaled = new Rectangle((int)Position.X, (int)Position.Y, (int)(Image.Width * Scale), (int)(Image.Height * Scale));
            TopMargin = NULL_HEIGHT;
            LeftMargin = NULL_WIDTH;
        }

        /// <summary>
        /// Computes scale by computing horizontal and vertical scale and the taking smallest
        /// </summary>
        /// <returns>The smallest of horizontal and vertical scales</returns>
        protected virtual float ComputeScale()
        {
            //Added float cast because otherwise it would perform an integer division always yielding 0! Anyway it's good and not good, go check AnimatedSprite.
            float horizontalScale = DisplayZone.Width / (float)Image.Width, verticalScale = DisplayZone.Height / (float)Image.Height;

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
            ComputeMargins();
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
        public virtual bool IsColliding(object otherObject)
        {
            //AnimatedSprite otherSprite = (AnimatedSprite)otherObject;
            //Rectangle rectangleCollision = Rectangle.Intersect(RectangleImageDimensionsScaled, otherSprite.RectangleImageDimensionsScaled);
            //bool collision = rectangleCollision.Width == NULL_WIDTH && rectangleCollision.Height == NULL_HEIGHT;

            //otherSprite.ToDestroy = collision;
            //return collision;

            //Rectangle otherRectangle = ((AnimatedSprite)otherObject).DestinationRectangle;

            //return DisplayZone.Intersects(otherRectangle);

            return true;

        }

        /// <summary>
        /// Computes the sprite's margins
        /// </summary>
        protected virtual void ComputeMargins()
        {
            RightMargin = Game.Window.ClientBounds.Width - RectangleImageDimensionsScaled.Width;
            BottomMargin = Game.Window.ClientBounds.Height - RectangleImageDimensionsScaled.Height;
        }
    }
}
