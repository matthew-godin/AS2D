﻿/*
Sphere.cs
---------

By Matthew Godin

Role : Component inheriting from AnimatedSprite
       showing a sphere bouncing on
       the edges of the screen

Created : 12 October 2016
Co-author : Raphael Brule
*/
using System;
using Microsoft.Xna.Framework;

namespace XNAProject
{
    /// <summary>
    /// Sphere bouncing on the edges of the screen
    /// </summary>
    public class Sphere : AnimatedSprite
    {
        const int STARTING_MINIMAL_DISPLACEMENT_ANGLE = 15, STARTING_MAXIMAL_DISPLACEMENT_ANGLE = 75, RIGHT_ANGLE = 90, MINIMAL_360_DEGREES_CIRCLE_FACTOR = 0, MAXIMAL_EXCLUSIVE_360_DEGREES_CIRCLE_FACTOR = 4, FLAT_ANGLE = 180, FULL_ANGLE = 360, X_UNIT = 1, Y_UNIT = 1;
        const float DISPLACEMENT_VECTOR_NORM = 3.5F;

        float DisplacementUpdateInterval { get; set; }
        Random RandomNumberGenerator { get; set; }
        float TimeElpasedSinceDisplacementUpdate { get; set; }
        float DisplacementAngle { get; set; }
        Vector2 DisplacementUpdateVector { get; set; }

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
            HazardousComponentsInitialization();
            DisplacementUpdateVector = new Vector2(DISPLACEMENT_VECTOR_NORM * (float)Math.Cos(MathHelper.ToRadians(DisplacementAngle)), DISPLACEMENT_VECTOR_NORM * (float)Math.Sin(MathHelper.ToRadians(DisplacementAngle)));
            TimeElpasedSinceDisplacementUpdate = NO_TIME_ELAPSED;
        }

        void HazardousComponentsInitialization()
        {
            Position = new Vector2(RandomNumberGenerator.Next(X_UNIT, RightMargin), RandomNumberGenerator.Next(Y_UNIT, BottomMargin / HALF_SIZE_DIVISOR));
            ComputeImageToDisplayRectangle();
            DisplacementAngle = RandomNumberGenerator.Next(MINIMAL_360_DEGREES_CIRCLE_FACTOR, MAXIMAL_EXCLUSIVE_360_DEGREES_CIRCLE_FACTOR) * RIGHT_ANGLE + RandomNumberGenerator.Next(STARTING_MINIMAL_DISPLACEMENT_ANGLE, STARTING_MAXIMAL_DISPLACEMENT_ANGLE);
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
                TimeElpasedSinceDisplacementUpdate = NO_TIME_ELAPSED;
                PerformDisplacementUpdate();
            }
        }

        /// <summary>
        /// Method updating sphere displacement according to time elapsed
        /// </summary>
        void PerformDisplacementUpdate()
        {
            Position += DisplacementUpdateVector;
            ComputeImageToDisplayRectangle();
            VerifyLeftRightEdgeBouncing();
            VerifyTopBottomEdgeBouncing();
        }

        /// <summary>
        /// Verifies if the sphere is bouncing on the left and right edges of the screen
        /// </summary>
        void VerifyLeftRightEdgeBouncing()
        {
            if (Position.X <= LeftMargin || Position.X >= RightMargin)
            {
                DisplacementAngle = FLAT_ANGLE - DisplacementAngle;
                DisplacementUpdateVector = new Vector2(DISPLACEMENT_VECTOR_NORM * (float)Math.Cos(MathHelper.ToRadians(DisplacementAngle)), DISPLACEMENT_VECTOR_NORM * (float)Math.Sin(MathHelper.ToRadians(DisplacementAngle)));
            }
        }

        /// <summary>
        /// Verifies if the sphere is bouncing on the top and bottom edges of the screen
        /// </summary>
        void VerifyTopBottomEdgeBouncing()
        {
            if (Position.Y >= BottomMargin || Position.Y <= TopMargin)
            {
                DisplacementAngle = FULL_ANGLE - DisplacementAngle;
                DisplacementUpdateVector = new Vector2(DISPLACEMENT_VECTOR_NORM * (float)Math.Cos(MathHelper.ToRadians(DisplacementAngle)), DISPLACEMENT_VECTOR_NORM * (float)Math.Sin(MathHelper.ToRadians(DisplacementAngle)));
            }
        }
    }
}