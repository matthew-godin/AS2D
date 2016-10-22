/* Author :            Raphael Brule
   File :           Spaceship.cs
   Date :              05 October 2016
   Description :       This component, child of AnimatedSprite,
                       manages the spaceship.*/

// Co-author : Matthew Godin
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNAProject
{
    /// <summary>
    /// This component, child of AnimatedSprite, manages the spaceship
    /// </summary>
    public class Spaceship : AnimatedSprite
    {
        const int NOT_MOVING = 0, MOVING = 1, NUM_PIXELS_MOVING = 4, MAX_NUM_MISSILES = 3, MAX_MISSILE_HEIGHT = 40, NUM_MISSILES_IN_FRAME = 25, BASE_ANIMATION = 0, HALF_WIDTH_SHIP_CANON = 4, ANIMATION_UNIT = 1, ANIMATION_WIDTH = 5, ANIMATION_HEIGHT = 4;
        const string MISSILE_IMAGE_STRING = "Missile", EXPLOSION_IMAGE_STRING = "Explosion";
        const float FAST_ANIMATION_INTERVAL = 1.5f * GameProject.STANDARD_INTERVAL;

        float DisplacementUpdateInterval { get; set; }
        float TimeSpentSinceUpdate { get; set; }
        int AnimationAccordingToMove { get; set; }
        Vector2 PreviousPosition { get; set; }
        int ShipFinalY { get; set; }
        bool IsDescending { get; set; }
        Vector2 DescentDisplacementVector { get; set; }
        Vector2 ResultingDisplacement { get; set; }
        InputManager InputMgr { get; set; }
        Vector2 MissileSupplementaryPosition { get; set; }

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
        public Spaceship(Game game, string imageName, Vector2 position, Rectangle displayZonee, Vector2 imageDescription, float animationUpdateInterval, float displacementUpdateInterval) : base(game, imageName, position, displayZonee, imageDescription, animationUpdateInterval)
        {
            DisplacementUpdateInterval = displacementUpdateInterval;
        }

        /// <summary>
        /// Initializes the spaceship's properties
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            TimeSpentSinceUpdate = NO_TIME_ELAPSED;
            AnimationAccordingToMove = BASE_ANIMATION; 
            Position = new Vector2(Position.X - SpriteDimensions.X / HALF_SIZE_DIVISOR, Position.Y - SpriteDimensions.Y / HALF_SIZE_DIVISOR);
            PreviousPosition = new Vector2(Position.X, Position.Y);
            ShipFinalY = Game.Window.ClientBounds.Height - (int)SpriteDimensions.Y; 
            IsDescending = true;
            DescentDisplacementVector = new Vector2(NO_DISPLACEMENT, NUM_PIXELS_MOVING);
            ResultingDisplacement = Position - PreviousPosition;
            MissileSupplementaryPosition = new Vector2(SpriteDimensions.X / HALF_SIZE_DIVISOR - HALF_WIDTH_SHIP_CANON, SpriteDimensions.Y / HALF_WIDTH_SHIP_CANON);
        }

        /// <summary>
        /// Loads components required by spaceship
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            InputMgr = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        /// <summary>
        /// Creates the rectangle representing the perimeter of what is selected by the original image
        /// </summary>
        /// <returns>A rectangle of type Rectangle</returns>
        protected new Rectangle CreateSourceRectangle()
        {
            return new Rectangle((SourceRectangle.X + (int)Delta.X) % (int)ImageDimensions.X, (int)Delta.Y * AnimationAccordingToMove, (int)Delta.X, (int)Delta.Y);
        }

        /// <summary>
        /// Ship component update method
        /// </summary>
        /// <param name="gameTime">Game objectTime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            VerifyMissileLaunch();
            UpdateShip(gameTime);
        }

        /// <summary>
        /// Verifies if Escape key has been pressed and launches a missile if so
        /// </summary>
        void VerifyMissileLaunch()
        {
            if (InputMgr.IsNewKey(Keys.Space))
            {
                LaunchMissile();
            }
        }

        /// <summary>
        /// Manages ship's movement based on time elapsed
        /// </summary>
        void UpdateShip(GameTime gameTime)
        {
            TimeSpentSinceUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TimeSpentSinceUpdate >= DisplacementUpdateInterval)
            {
                TimeSpentSinceUpdate = NO_TIME_ELAPSED;
                DetermineIfShipIsDescending();
                PerformDisplacementUpdate();
            }
        }

        /// <summary>
        /// Determine if at beginning of game and move ship down if that is the case
        /// </summary>
        void DetermineIfShipIsDescending()
        {
            if (IsDescending)
            {
                ManageShipDescent();
            }
        }

        /// <summary>
        /// Manages the descent of the ship at the beginning of the game
        /// </summary>
        void ManageShipDescent()
        {
            Position += DescentDisplacementVector;
            ComputeImageRectangleToDisplay();
            if (Position.Y >= ShipFinalY)
            {
                Position = new Vector2(Position.X, ShipFinalY);
                IsDescending = false;
            }
        }

        /// <summary>
        /// Perform displacement update according to keys pressed
        /// </summary>
        void PerformDisplacementUpdate()
        {
            PreviousPosition = new Vector2(Position.X, Position.Y);
            ManageKeyboard();
            ComputeImageRectangleToDisplay();
            ResultingDisplacement = Position - PreviousPosition;
            AnimationAccordingToMove = (IsMoving() ? MOVING : NOT_MOVING);
        }

        /// <summary>
        /// Manages the horizontal displacement of the ship according to keys A and D
        /// </summary>
        void ManageKeyboard()
        {
            if (InputMgr.IsKeyboardActive)
            {
                int horizontalDisplacement = ManageKey(Keys.D) - ManageKey(Keys.A);
                AdjustPosition(horizontalDisplacement);
            }
        }

        /// <summary>
        /// Returns number of pixels by which the ship must move if key is pressed, otherwise return zero
        /// </summary>
        /// <param name="key">Key pressed</param>
        /// <returns>Number of pixels of displacement or zero</returns>
        int ManageKey(Keys key)
        {
            return InputMgr.IsPressed(key) ? NUM_PIXELS_MOVING : NO_DISPLACEMENT;
        }

        /// <summary>
        /// Adjusts the position's property according to horizontal displacement
        /// </summary>
        /// <param name="horizontalDisplacement">Horizontal displacement</param>
        void AdjustPosition(int horizontalDisplacement)
        {
            float x = ComputePosition(horizontalDisplacement, Position.X, LeftMargin, RightMargin);

            Position = new Vector2(x, Position.Y);
        }

        /// <summary>
        /// Computes horizontal position according to displacement
        /// </summary>
        /// <param name="displacement">Horizontal displacement</param>
        /// <param name="currentPosition">Current position</param>
        /// <param name="MinThreshold">Positions's minimum threshold</param>
        /// <param name="MaxThreshold">Positions's maximum threshold</param>
        /// <returns>Horizontal position</returns>
        float ComputePosition(int displacement, float currentPosition, int MinThreshold, int MaxThreshold)
        {
            float position = currentPosition + displacement;
            return MathHelper.Min(MathHelper.Max(MinThreshold, position), MaxThreshold);
        }

        /// <summary>
        /// Verifies if ship is moving
        /// </summary>
        /// <returns>True or false</returns>
        bool IsMoving()
        {
            return ResultingDisplacement != Vector2.Zero;
        }

        /// <summary>
        /// Launches missiles according to defined maximum
        /// </summary>
        void LaunchMissile()
        {
            int numMissiles = (Game.Components.Where(component => component is Missile && !((Missile)component).ToDestroy && ((Missile)component).Visible).Count());

            if (numMissiles < MAX_NUM_MISSILES)
            {
                Missile missile = new Missile(Game, MISSILE_IMAGE_STRING, new Vector2(Position.X + MissileSupplementaryPosition.X, Position.Y - MissileSupplementaryPosition.Y), new Rectangle(NULL_X, NULL_Y, MAX_MISSILE_HEIGHT, MAX_MISSILE_HEIGHT), new Vector2(NUM_MISSILES_IN_FRAME, ANIMATION_UNIT), EXPLOSION_IMAGE_STRING, new Vector2(ANIMATION_WIDTH, ANIMATION_HEIGHT), FAST_ANIMATION_INTERVAL, GameProject.STANDARD_INTERVAL);
                Game.Components.Add(missile);
            }
        }
    }
}
