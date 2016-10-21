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
        protected const int NO_TIME_ELAPSED = 0, NO_DISPLACEMENT = 0;

        //fireball
        Vector2 ImageDescription { get; set; }
        protected float AnimationUpdateInterval { get; set; }

        //Properties initially managed by Initialze
        public bool ToDestroy { get; set; }
        float TimeElapsedSinceAnimationUpdate { get; set; }
        //int Row { get; set; }
        //int VariableToChangeName { get; set; }
        protected Vector2 Delta { get; set; }
        int AnimationFrameWidth { get; set; }
        int AnimationFrameHeight { get; set; }

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
            ToDestroy = false;
            TimeElapsedSinceAnimationUpdate = NO_TIME_ELAPSED;
            base.Initialize();
            AnimationFrameWidth = (int)ImageDimensions.X - (int)Delta.X;
            AnimationFrameHeight = (int)ImageDimensions.Y - (int)Delta.Y;
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
        /// Computes rectangle covering what will be displayed
        /// </summary>
        /// <returns>Source Rectangle</returns>
        protected override Rectangle ComputeSourceRectangle()
        {
            return new Rectangle(NULL_X, NULL_Y, (int)Delta.X, (int)Delta.Y);
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
            SourceRectangle = new Rectangle((SourceRectangle.X + (int)Delta.X) % (int)ImageDimensions.X, SourceRectangle.X >= AnimationFrameWidth ? (SourceRectangle.Y >= AnimationFrameHeight ? NULL_Y : SourceRectangle.Y + (int)Delta.Y) : SourceRectangle.Y, (int)Delta.X, (int)Delta.Y);
        }

        /// <summary>
        /// Updates the animated sprite
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
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
        /// Predicate true if the Sprite is in collision with another object
        /// </summary>
        /// <param name="otherObject">L'autre objet qui pourrait être en collision</param>
        /// <returns></returns>
        public override bool IsColliding(object otherObject)
        {
            Rectangle otherRectangle = ((AnimatedSprite)otherObject).RectangleImageÀAfficher;

            return RectangleImageÀAfficher.Intersects(otherRectangle);
        }
    }
}