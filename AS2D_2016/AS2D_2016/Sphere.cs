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
        const int STARTING_MINIMAL_DISPLACEMENT_ANGLE = 15, STARTING_MAXIMAL_DISPLACEMENT_ANGLE = 75, RIGHT_ANGLE = 90, MINIMAL_360_DEGREES_CIRCLE_FACTOR = 0, MAXIMAL_EXCLUSIVE_360_DEGREES_CIRCLE_FACTOR = 4;

        float DisplacementUpdateInterval { get; set; }
        Random RandomNumberGenerator { get; set; }
        float TimeElpasedSinceDisplacementUpdate { get; set; }
        float DisplacementAngle { get; set; }
        Vector2 UpdateDisplacementUnitVector { get; set; }

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
            LoadContent();
            base.Initialize();
            /* Maybe +1 to generator cause excluded*/
            Position = new Vector2(RandomNumberGenerator.Next(NULL_X, RightMargin), RandomNumberGenerator.Next(NULL_Y, BottomMargin / HALF_SIZE_DIVISOR));
            DisplacementAngle = RandomNumberGenerator.Next(MINIMAL_360_DEGREES_CIRCLE_FACTOR, MAXIMAL_EXCLUSIVE_360_DEGREES_CIRCLE_FACTOR) * RIGHT_ANGLE + RandomNumberGenerator.Next(STARTING_MINIMAL_DISPLACEMENT_ANGLE, STARTING_MAXIMAL_DISPLACEMENT_ANGLE);
            UpdateDisplacementUnitVector = new Vector2((float)Math.Cos(MathHelper.ToRadians(DisplacementAngle)), (float)Math.Sin(MathHelper.ToRadians(DisplacementAngle)));
        }

        /// <summary>
        /// Loads content needed by the sphere
        /// </summary>
        protected override void LoadContent()
        {
            RandomNumberGenerator = Game.Services.GetService(typeof(Random)) as Random;
            base.LoadContent();
        }

        /// <summary>
        /// Updates the sphere
        /// </summary>
        /// <param name="gameTime">Contains time information</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            TimeElpasedSinceDisplacementUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TimeElpasedSinceDisplacementUpdate >= DisplacementUpdateInterval)
            {
                PerformDisplacementUpdate();
                TimeElpasedSinceDisplacementUpdate = NO_TIME_ELAPSED;
            }
        }

        /// <summary>
        /// Method updating sphere displacement according to time elapsed
        /// </summary>
        protected virtual void PerformDisplacementUpdate()
        {
            Position += UpdateDisplacementUnitVector;
            
        }
    }
}