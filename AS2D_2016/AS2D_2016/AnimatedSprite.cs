﻿/*
AnimatedSprite.cs
--------------

By Matthew Godin

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
        const float NO_DISPLACEMENT = 0.0F;
        const int ORIGIN = 0;

        //fireball
        Vector2 ImageDescription { get; set; }
        float AnimationUpdateInterval { get; set; }

        //Properties initially managed by Initialze
        protected Rectangle SourceRectangle { get; set; }
        public bool ToDestroy { get; set; }
        float TimeElapsedSinceUpdate { get; set; }
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
        public AnimatedSprite(Game game, string imageName, Vector2 position, Rectangle displayZonee, Vector2 imageDescription, float animationUpdateInterval) : base(game, imageName, position, displayZonee)
        {
            ImageDescription = new Vector2(imageDescription.X, imageDescription.Y);
            AnimationUpdateInterval = animationUpdateInterval;
        }

        /// <summary>
        /// Initializes AnimatedSprite's components
        /// </summary>
        public override void Initialize()
        {
            SourceRectangle = new Rectangle(ORIGIN, ORIGIN, (int)Delta.X, (int)Delta.Y);
            Delta = new Vector2(Image.Width, Image.Height) / ImageDescription;
            ToDestroy = false;
            TimeElapsedSinceUpdate = 0;
            //Row = 0;

            base.Initialize();
        }

        /// <summary>
        /// Method update AnimatedSprite according to time elapsed
        /// </summary>
        protected virtual void PerformUpdate()
        {
            //if(Row == ImageDescription.Y)
            //    Row = 0;

            //VariableToChangeName = (SourceRectangle.X + (int)Delta.X) % Image.Width;

            //SourceRectangle = new Rectangle(VariableToChangeName,
            //                       (int)Delta.Y * Row, (int)Delta.X, (int)Delta.Y);

            //if(VariableToChangeName == ImageDescription.X - 1)
            //    ++Row;
            SourceRectangle = new Rectangle((SourceRectangle.X + (int)Delta.X) % Image.Width, SourceRectangle.X > Image.Width - (int)Delta.X ? (SourceRectangle.Y > Image.Height - (int)Delta.Y ? ORIGIN : SourceRectangle.Y + (int)Delta.Y) : SourceRectangle.Y, (int)Delta.X, (int)Delta.Y);
        }

        public override void Update(GameTime gameTime)
        {
            //ToDestroy = IsColliding(this); LINE NOT GOOD TO CHANGE

            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsedSinceUpdate += timeElapsed;
            if (TimeElapsedSinceUpdate >= AnimationUpdateInterval)
            {
                PerformUpdate();
                TimeElapsedSinceUpdate = 0;
            }
        }

        /// <summary>
        /// Draws the AnimatedSprite
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override void Draw(GameTime gameTime)
        {
            SpriteMgr.Draw(Image, Position, SourceRectangle, Color.White);
        }

        /// <summary>
        /// Predicate true if the Sprite is in collision with another object
        /// </summary>
        /// <param name="otherObject"></param>
        /// <returns></returns>
        public override bool IsColliding(object otherObject)
        {
            AnimatedSprite otherSprite = (AnimatedSprite)otherObject;
            Rectangle rectangleCollision = Rectangle.Intersect(RectangleImageDimensionsÀLScale, otherSprite.RectangleImageDimensionsÀLScale);
            bool collision = rectangleCollision.Width == NULL_WIDTH && rectangleCollision.Height == NULL_HEIGHT;

            ToDestroy = collision;
            otherSprite.ToDestroy = collision;

            return collision;
        }
    }
}