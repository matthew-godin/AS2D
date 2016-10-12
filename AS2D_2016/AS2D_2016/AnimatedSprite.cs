/*
AnimatedSprite.cs
--------------

By Matthew Godin

Role : Component inheriting from Sprite and
       animates the sprite that will
       be displayed on the screen by showing different
       frames in the same loaded image

Created : 5 October 2016
*/
using Microsoft.Xna.Framework;

namespace XNAProject
{
    /// <summary>
    /// Component displaying an animated sprite showing different frames coming from the same image
    /// </summary>
    public class AnimatedSprite : Sprite, IDestructible
    {
        //Constants
        const float NO_DISPLACEMENT = 0.0F;
        const int ORIGIN = 0;

        //fireball
        Vector2 ImageDescription { get; set; }
        float AnimationUpdateInterval { get; set; }

        //fireball
        Rectangle SourceRectangle { get; set; }
        public bool ToDestroy { get; set; }

        //fireball
        protected Vector2 Delta { get; set; }
        protected int RightMargin { get; set; }
        protected int BottomMargin { get; set; }
        protected int LeftMargin { get; set; }
        protected int TopMargin { get; set; }

        /// <summary>
        /// AnimatedSprite's constructor
        /// </summary>
        /// <param name="game">Game object</param>
        /// <param name="imageName">Sprite file name</param>
        /// <param name="position">Sprite starting position</param>
        /// <param name="displayZonee">Sprite's display zone</param>
        /// <param name="imageDescription">Number of x and y sprites in loaded image</param>
        /// <param name="animationUpdateInterval">Sprite animation update interval</param>
        public AnimatedSprite(Game game, string imageName, Vector2 position, Rectangle displayZonee,
                           Vector2 imageDescription, float animationUpdateInterval) 
            : base(game, imageName, position, displayZonee)
        {
            ImageDescription = new Vector2(imageDescription.X, imageDescription.Y);
            AnimationUpdateInterval = animationUpdateInterval;
        }

        /// <summary>
        /// Loading method
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            SourceRectangle = new Rectangle(ORIGIN, ORIGIN, (int)Delta.X, (int)Delta.Y);
            ToDestroy = false;
        }

        /// <summary>
        /// Computes the sprite's margins
        /// </summary>
        protected void ComputeMargins()
        {
            Delta = new Vector2(Image.Width, Image.Height) / ImageDescription;
            RightMargin = Game.Window.ClientBounds.Width - (int)Delta.X;
            BottomMargin = Game.Window.ClientBounds.Height - (int)Delta.Y;
            LeftMargin = 0; TopMargin = 0;
        }

        /// <summary>
        /// Method update AnimatedSprite according to time elapsed
        /// </summary>
        protected virtual void PerformUpdate()
        {
            ToDestroy = IsColliding(this);
        }



        //protected virtual void AnimatedSpriteOnOneLine()//#linetomatthew
        //{
        //    SourceRectangle = new Rectangle((SourceRectangle.X + (int)Delta.X) % Image.Width, SourceRectangle.X > Image.Width - (int)Delta.X ? (SourceRectangle.Y > Image.Height - (int)Delta.Y ? ORIGIN : SourceRectangle.Y + (int)Delta.Y) : SourceRectangle.Y, (int)Delta.X, (int)Delta.Y);
        //}

        /// <summary>
        /// Draws the AnimatedSprite
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override void Draw(GameTime gameTime)
        {
            SpriteMgr.Draw(Image, Position, SourceRectangle, Color.White);
        }
    }
}