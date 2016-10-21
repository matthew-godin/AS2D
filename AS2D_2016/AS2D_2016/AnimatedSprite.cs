/*
AnimatedSprite.cs
--------------

By Matthew Godin and Raphael Brule

Role : Component inheriting from Sprite and 
       animates the sprite that will
       be displayed on the screen by showing different
       frames in the same loaded image

Created : 5 October 2016
Modified : 12 October 2016
Description : Large modifications for IsColliding to make ToDestroy and others
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
        protected const int NO_TIME_ELAPSED = 0, NO_DISPLACEMENT = 0, ORIGIN = 0;

        //fireball
        Vector2 ImageDescription { get; set; }
        protected float AnimationUpdateInterval { get; set; }

        //Properties initially managed by Initialze
        protected Rectangle SourceRectangle { get; set; }
        public bool ToDestroy { get; set; }
        float TimeElapsedSinceAnimationUpdate { get; set; }
        //int Row { get; set; }
        //int VariableToChangeName { get; set; }
        protected Vector2 Delta { get; set; }

        /// <summary>
        /// AnimatedSprite's constructor
        /// </summary>
        /// <param name="game">Game object</param>
        /// <param name="imageName">Sprite file name</param>
        /// <param name="position">Sprite starting position</param>
        /// <param name="displayZonee">Sprite's display zone</param>
        /// <param name="imageDescription">Number of x and y sprites in loaded image</param>
        /// <param name="animationUpdateInterval">Sprite animation update interval</param>
        public AnimatedSprite(Game game, string imageName, Vector2 position, Rectangle displayZonee, Vector2 imageDescription, float animationUpdateInterval) 
            : base(game, imageName, position, displayZonee)
        {
            ImageDescription = new Vector2(imageDescription.X, imageDescription.Y);
            AnimationUpdateInterval = animationUpdateInterval;
        }

        /// <summary>
        /// Initializes AnimatedSprite's components
        /// </summary>
        public override void Initialize()
        {
            LoadContent();
            Delta = ComputeOriginalSpriteDimensions();
            SourceRectangle = new Rectangle(ORIGIN, ORIGIN, (int)Delta.X, (int)Delta.Y);
            ToDestroy = false;
            TimeElapsedSinceAnimationUpdate = NO_TIME_ELAPSED;
            base.Initialize();
        }

        /// <summary>
        /// Computes sprite dimensions as seen in its file
        /// </summary>
        /// <returns>Returns the Vector2 containing its dimensions</returns>
        protected override Vector2 ComputeOriginalSpriteDimensions()
        {
            return base.ComputeOriginalSpriteDimensions() / ImageDescription;
        }

        /// <summary>
        /// Computes the sprite horizontal scale for the Draw method
        /// </summary>
        protected override float ComputeHorizontalScale()
        {
            return DisplayZone.Width / Delta.X;
        }

        /// <summary>
        /// Computes the sprite vertical scale for the Draw method
        /// </summary>
        protected override float ComputeVerticalScale()
        {
            return DisplayZone.Height / Delta.Y;
        }

        /// <summary>
        /// Method update AnimatedSprite according to time elapsed
        /// </summary>
        protected virtual void PerformAnimationUpdate()
        {
            SourceRectangle = new Rectangle((SourceRectangle.X + (int)Delta.X) % Image.Width, SourceRectangle.X >= Image.Width - (int)Delta.X ? (SourceRectangle.Y >= Image.Height - (int)Delta.Y ? ORIGIN : SourceRectangle.Y + (int)Delta.Y) : SourceRectangle.Y, (int)Delta.X, (int)Delta.Y);
        }

        public override void Update(GameTime gameTime)
        {
            TimeElapsedSinceAnimationUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TimeElapsedSinceAnimationUpdate >= AnimationUpdateInterval)
            {
                PerformAnimationUpdate();
                TimeElapsedSinceAnimationUpdate = NO_TIME_ELAPSED;
            }
        }

        /// <summary>
        /// Draws the AnimatedSprite
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override void Draw(GameTime gameTime)
        {
            SpriteMgr.Draw(Image, RectangleImageDimensionsÀLScale, SourceRectangle, Color.White);
        }

        /// <summary>
        /// Predicate true if the Sprite is in collision with another object
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns></returns>
        public override bool IsColliding(object otherObject)
        {
            Rectangle otherRectangle = ((AnimatedSprite)otherObject).RectangleImageDimensionsÀLScale;

            return RectangleImageDimensionsÀLScale.Intersects(otherRectangle);
        }
    }
}