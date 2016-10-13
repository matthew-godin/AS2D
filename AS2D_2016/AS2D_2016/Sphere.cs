/*
Sphere.cs
---------

By Matthew Godin

Role : Component inheriting from AnimatedSprite
       showing a sphere bouncing on
       the edges of the screen

Created : 12 October 2016
*/
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
    /// Sphere bouncing on the edges of the screen
    /// </summary>
    public class Sphere : AnimatedSprite
    {
        float DisplacementUpdateInterval { get; set; }

        /// <summary>
        /// Sphere constructor
        /// </summary>
        /// <param name="game">Game object</param>
        /// <param name="imageName">Sphere file name</param>
        /// <param name="position">Sphere starting position</param>
        /// <param name="displayZone">Sphere display zone</param>
        /// <param name="imageDescription">Number of sphere sprites in x and y contained in the loaded image</param>
        /// <param name="animationUpdateInterval">Sphere animation update interval</param>
        public Sphere(Game game, string imageName, Vector2 position, Rectangle displayZone, Vector2 imageDescription, float animationUpdateInterval, float displacementUpdateInterval) : base(game, imageName, position, displayZone, imageDescription, animationUpdateInterval)
        {
            DisplacementUpdateInterval = displacementUpdateInterval;
        }

        /// <summary>
        /// Initializes the sphere components
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// Updates the sphere
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }
}