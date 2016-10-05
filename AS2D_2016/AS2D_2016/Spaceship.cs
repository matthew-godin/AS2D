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
        const int MOVING = 1;

        //Property initially managed by the constructor
        float DisplacementUpdateInterval { get; set; }

        int DeltaX { get; set; }
        int DeltaY { get; set; }
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
    }
}
