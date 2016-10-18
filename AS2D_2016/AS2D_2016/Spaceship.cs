/* Author :            Raphael Brule
   File :           Spaceship.cs
   Date :              05 October 2016
   Description :       This component, child of AnimatedSprite,
                       manages the spaceship.*/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNAProject
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Spaceship : AnimatedSprite
    {
        //Constant
        const int NOT_MOVING = 0;
        const int MOVING = 1;
        const int NUM_PIXELS_MOVING = 5;

        //Property initially managed by the constructor
        float DisplacementUpdateInterval { get; set; }

        //Property initially managed by Initialize
        float TimeSpentSinceUpdate { get; set; }
        int AnimationAccordingToMove { get; set; }
        Vector2 PreviousPosition { get; set; }

        //Property initially managed by LoadContent
        InputManager InputMgr { get; set; }

        //to check
        Vector2 ResultingDisplacement { get; set; }


        /// <summary>
        /// Spaceship constructor
        /// </summary>
        /// <param name="game">Game object</param>
        /// <param name="imageName">Image name (string)</param>
        /// <param name="position">Position (Vector2)</param>
        /// <param name="displayZonee">Display zone (Rectangle)</param>
        /// <param name="imageDescription">Image description (Vector2)</param>
        /// <param name="animationUpdateInterval">Animation update interval (float)</param>
        /// <param name="displacementUpdateInterval">Displacement update interval (float)</param>
        public Spaceship(Game game, string imageName,
                               Vector2 position, Rectangle displayZonee,
                               Vector2 imageDescription, float animationUpdateInterval,
                               float displacementUpdateInterval)
            : base(game, imageName, position, displayZonee,
                  imageDescription, animationUpdateInterval)
        {
            DisplacementUpdateInterval = displacementUpdateInterval;
        }

        public override void Initialize()
        {
            base.Initialize();

            Position = new Vector2(Position.X - DestinationRectangle.Width/2,
                                   Game.Window.ClientBounds.Height - DestinationRectangle.Height);
            TimeSpentSinceUpdate = 0;
            AnimationAccordingToMove = 0;
            PreviousPosition = new Vector2(Position.X, Position.Y);

            
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        protected override void PerformAnimationUpdate()
        {
            SourceRectangle = new Rectangle((SourceRectangle.X + (int)Delta.X) % Image.Width,
                             (int)Delta.Y *AnimationAccordingToMove,
                             (int)Delta.X, (int)Delta.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Added for missile
            if (InputMgr.IsNewKey(Keys.Space))
                LaunchMissile();

            float TimeElapased = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeSpentSinceUpdate += TimeElapased;
            if (TimeSpentSinceUpdate >= DisplacementUpdateInterval)
            {
                PerformDisplacementUpdate();
                TimeSpentSinceUpdate = 0;
            }
        }

        void PerformDisplacementUpdate()
        {
            PreviousPosition = new Vector2(Position.X, Position.Y);
            
            ManageKeyboard();

            ResultingDisplacement = Position - PreviousPosition;

            AnimationAccordingToMove = (IsMoving()? MOVING : NOT_MOVING);


        }

        void ManageKeyboard()
        {
            if (InputMgr.IsKeyboardActive)
            {
                int horizontalDisplacement = ManageKey(Keys.D) - ManageKey(Keys.A);
                AdjustPosition(horizontalDisplacement);
            }
        }

        int ManageKey(Keys key)
        {
            return InputMgr.IsPressed(key) ? NUM_PIXELS_MOVING : 0;
        }

        void AdjustPosition(int horizontalDisplacement)
        {
            float posX = ComputePosition(horizontalDisplacement, Position.X, LeftMargin, RightMargin);

            Position = new Vector2(posX, Position.Y);
        }

        float ComputePosition(int displacement, float currentPosition, int MinThreshold, int MaxThreshold)
        {
            float position = currentPosition + displacement;
            return MathHelper.Min(MathHelper.Max(MinThreshold, position), MaxThreshold);
        }

        bool IsMoving()
        {
            return ResultingDisplacement != Vector2.Zero;
        }

        //public override void Draw(GameTime gameTime)
        //{
        //    //SpriteMgr.Draw(Image, Position, SourceRectangle, Color.White);

        //    SpriteMgr.Draw(Image, new Rectangle((int)Position.X, (int)Position.Y, (int)(Delta.X*Scale), (int)(Delta.Y*Scale)),
        //                        SourceRectangle, Color.White);
        //}

        void LaunchMissile()
        {
            int numMissiles = (Game.Components.Where(component => component is Missile && !((Missile)component).ToDestroy)).ToList().Count();

            if(numMissiles < 3)
            {
                Missile missile = new Missile(Game,
                                                "Missile",
                                                new Vector2(DestinationRectangle.X + DestinationRectangle.Width/2 - 4, DestinationRectangle.Y - DestinationRectangle.Height/4),
                                                new Rectangle(0, 0, 30, 40),
                                                new Vector2(25, 1),
                                                "Explosion",
                                                new Vector2(5, 4),
                                                1.5f * GameProject.STANDARD_INTERVAL,
                                                GameProject.STANDARD_INTERVAL);
                Game.Components.Add(missile);
            }
        }

    }
}
