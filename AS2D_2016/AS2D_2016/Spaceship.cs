/* Author :            Raphael Brule
   File :           Spaceship.cs
   Date :              05 October 2016
   Description :       This component, child of AnimatedSprite,
                       manages the spaceship.*/

// Modification : Modifications for the descent of the ship at the beginning
//                Matthew Godin
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNAProject
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Spaceship : AnimatedSprite
    {
        //Constant
        const int NOT_MOVING = 0,
                  MOVING = 1,
                  NUM_PIXELS_MOVING = 4, // Changed it from 5 to 4
                  MAX_NUM_MISSILES = 3,
                  MAX_MISSILE_HEIGHT = 40,
                  NUM_MISSILES_IN_FRAME = 25;


        //Property initially managed by the constructor
        float DisplacementUpdateInterval { get; set; }

        //Property initially managed by Initialize
        float TimeSpentSinceUpdate { get; set; }
        int AnimationAccordingToMove { get; set; }
        Vector2 PreviousPosition { get; set; }
        // Added for ship descent
        int ShipFinalY { get; set; }
        bool IsDescending { get; set; }
        Vector2 DescentDisplacementVector { get; set; } // Other similar things could be done in the rest of the class for optimization
        Vector2 ResultingDisplacement { get; set; }

        //Property initially managed by LoadContent
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

        /// <summary>
        /// Initializes the spaceship's properties
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            TimeSpentSinceUpdate = 0;
            AnimationAccordingToMove = 0;
            //To erase with the descent of the ship now : Position = new Vector2(Position.X - DestinationRectangle.Width/2, Game.Window.ClientBounds.Height - DestinationRectangle.Height); 
            Position = new Vector2(Position.X - SpriteDimensions.X / HALF_SIZE_DIVISOR, Position.Y - SpriteDimensions.Y / HALF_SIZE_DIVISOR); // Nouvelle ligne
            PreviousPosition = new Vector2(Position.X, Position.Y);
            ShipFinalY = Game.Window.ClientBounds.Height - (int)SpriteDimensions.Y; 
            IsDescending = true;
            DescentDisplacementVector = new Vector2(NO_DISPLACEMENT, NUM_PIXELS_MOVING);
            ResultingDisplacement = Position - PreviousPosition;
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
            
            if (InputMgr.IsNewKey(Keys.Space))
                LaunchMissile();

            float TimeElapased = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeSpentSinceUpdate += TimeElapased;
            if (TimeSpentSinceUpdate >= DisplacementUpdateInterval)
            {
                DetermineIfShipIsDescending();
                PerformDisplacementUpdate();
                TimeSpentSinceUpdate = NO_TIME_ELAPSED;
            }
        }

        /// <summary>
        /// Determine if at beginning of game and move ship down if that is the case
        /// </summary>
        void DetermineIfShipIsDescending()
        {
            if (IsDescending)
            {
                ManageShipDescent(); // New method
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
            return InputMgr.IsPressed(key) ? NUM_PIXELS_MOVING : 0;
        }

        /// <summary>
        /// Adjusts the position's property according to horizontal displacement
        /// </summary>
        /// <param name="horizontalDisplacement">Horizontal displacement</param>
        void AdjustPosition(int horizontalDisplacement)
        {
            float posX = ComputePosition(horizontalDisplacement, Position.X, LeftMargin, RightMargin);

            Position = new Vector2(posX, Position.Y);
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
                Missile missile = new Missile(Game, "Missile",
                                                new Vector2(Position.X + SpriteDimensions.X / HALF_SIZE_DIVISOR - 4, Position.Y - SpriteDimensions.Y / 4),
                                                new Rectangle(NULL_X, NULL_Y, MAX_MISSILE_HEIGHT, MAX_MISSILE_HEIGHT),
                                                new Vector2(NUM_MISSILES_IN_FRAME, 1),
                                                "Explosion",
                                                new Vector2(5, 4),
                                                1.5f * GameProject.STANDARD_INTERVAL,
                                                GameProject.STANDARD_INTERVAL);
                Game.Components.Add(missile);
            }
        }

    }
}
