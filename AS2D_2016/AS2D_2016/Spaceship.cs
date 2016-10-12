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

//CLASS VERY INCOMPLETE!!

namespace XNAProject
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Spaceship : AnimatedSprite
    {
        //Constant
        const int NUM_PIXELS_MOVING = 1;

        //Property initially managed by the constructor
        float DisplacementUpdateInterval { get; set; }

        //Property initially managed by LoadContent
        bool IsMoving { get; set; }
        InputManager InputMgr { get; set; }

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

        protected override void LoadContent()
        {
            base.LoadContent();

            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;

        }

        protected override void PerformUpdate()
        {
            base.PerformUpdate();
            ManageKeyboard();
            if (IsMoving)
            {

            }
        }

        void ManageKeyboard()
        {
            if (InputMgr.IsKeyboardActive)
            {
                int horizontalDisplacement = ManageKey(Keys.D) - ManageKey(Keys.A);
                int displacementVertical = ManageKey(Keys.S) - ManageKey(Keys.W);
                if (horizontalDisplacement != 0 || displacementVertical != 0)
                {
                    IsMoving = true;
                    AdjustPosition(horizontalDisplacement, displacementVertical);
                }
                else
                {
                    IsMoving = false;
                }
            }
        }

        int ManageKey(Keys key)
        {
            return InputMgr.IsPressed(key) ? NUM_PIXELS_MOVING : 0;
        }

        void AdjustPosition(int horizontalDisplacement, int displacementVertical)
        {
            float posX = ComputePosition(horizontalDisplacement, Position.X, LeftMargin, RightMargin);
            float posY = ComputePosition(displacementVertical, Position.Y, TopMargin, BottomMargin);
            Position = new Vector2(posX, posY);
        }

        float ComputePosition(int displacement, float currentPosition, int MinThreshold, int MaxThreshold)
        {
            float position = currentPosition + displacement;
            return MathHelper.Min(MathHelper.Max(MinThreshold, position), MaxThreshold);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            PerformUpdate();

            base.Update(gameTime);
        }
    }
}
