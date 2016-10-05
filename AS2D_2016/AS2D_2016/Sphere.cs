/* Author :            Raphael Brule
   File :           Sphere.cs
   Date :              05 October 2016
   Description :       This component, child of AnimatedSprite, allows
                       to manage a sphere.*/
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
    public class Sphere : AnimatedSprite
    {
        //Properties initially managed by the constructor
        float DisplacementUpdateInterval { get; set; }

        /// <summary>
        /// Sphere constructor
        /// </summary>
        /// <param name="game">Game object</param>
        /// <param name="imageName">Image name (string)</param>
        /// <param name="position">Position (Vector2)</param>
        /// <param name="displayZone">Display zone (Rectangle)</param>
        /// <param name="imageDescription">Image description (Vector2)</param>
        /// <param name="animationUpdateInterval">Animation update interval (float)</param>
        /// <param name="displacementUpdateInterval">Displacement update interval (float)</param>
        public Sphere(Game game, string imageName, Vector2 position,
                      Rectangle displayZone, Vector2 imageDescription,
                      float animationUpdateInterval, float displacementUpdateInterval)
            : base(game, imageName, position, displayZone,
                   imageDescription, animationUpdateInterval)
        {
            DisplacementUpdateInterval = displacementUpdateInterval;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
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
