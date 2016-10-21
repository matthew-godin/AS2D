/*
CenteredText.cs
--------------

By Matthew Godin

Role : Component showing a centered text
       on the screen at the end of the game

Created : 5 October 2016
Modified : 11 October 2016
Description : Almost all the code making everything
              functional has been written
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
{
    /// <summary>
    /// Component showing a growing text in the middle of the screen
    /// </summary>
    public class CenteredText : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const int HALF_SIZE_DIVISOR = 2;
        const float NO_DEPTH_LAYER = 0.0F;
        const float NO_ROTATION = 0.0F;

        string TextToDisplay { get; set; }
        string FontName { get; set; }
        Rectangle DisplayZone { get; set; }
        Rectangle DisplayZoneWithMargins { get; set; }
        Color TextColor { get; set; }
        float Margin { get; set; }
        SpriteFont Font { get; set; }
        Vector2 Origin { get; set; }
        SpriteBatch SpriteMgr { get; set; }
        Vector2 CenteredPosition { get; set; }
        float Scale { get; set; }
        Vector2 TextDimensions { get; set; }

        /// <summary>
        /// Constructor saving passed settings
        /// </summary>
        /// <param name="game">Game that called this component</param>
        /// <param name="message">String representing the message to display</param>
        /// <param name="position">Message position relative to its center</param>
        /// <param name="color">Color wanted for revolving message</param>
        /// <param name="updateInterval">Interval by which the message is update to grow and revolve</param>
        public CenteredText(Game game, string textToDisplay, string fontName, Rectangle displayZonee, Color textColor, float margin) : base(game)
        {
            TextToDisplay = textToDisplay;
            FontName = fontName;
            DisplayZone = displayZonee;
            TextColor = textColor;
            Margin = margin;
        }

        /// <summary>
        /// Initializes time, angle and scale parameters of the centered text
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            DisplayZoneWithMargins = new Rectangle(DisplayZone.X, DisplayZone.Y, (int)(DisplayZone.Width - DisplayZone.Width * Margin), (int)(DisplayZone.Height - DisplayZone.Height * Margin));
            TextDimensions = Font.MeasureString(TextToDisplay);
            Scale = ComputeScale();
            InitializeOrigin();
            CenteredPosition = new Vector2(Game.Window.ClientBounds.Width / HALF_SIZE_DIVISOR, Game.Window.ClientBounds.Height / HALF_SIZE_DIVISOR);
            this.Enabled = false;
        }

        /// <summary>
        /// Loads font and sprite batch and computes origin of centered text
        /// </summary>
        protected override void LoadContent()
        {
            Font = Game.Content.Load<SpriteFont>("Fonts/" + FontName);
            SpriteMgr = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
        }

        /// <summary>
        /// Initializes the origin
        /// </summary>
        void InitializeOrigin()
        {
            Origin = new Vector2(TextDimensions.X / HALF_SIZE_DIVISOR, TextDimensions.Y / HALF_SIZE_DIVISOR);
        }

        /// <summary>
        /// Computes scale by computing horizontal and vertical scale and the taking smallest
        /// </summary>
        /// <returns>The smallest of horizontal and vertical scales</returns>
        float ComputeScale()
        {
            float horizontalScale = DisplayZoneWithMargins.Width / TextDimensions.X, verticalScale = DisplayZoneWithMargins.Height / TextDimensions.Y;

            return horizontalScale < verticalScale ? horizontalScale : verticalScale;
        }

        /// <summary>
        /// Draws centered text to the screen
        /// </summary>
        /// <param name="gameTime">Gives time information</param>
        public override void Draw(GameTime gameTime)
        {
            SpriteMgr.DrawString(Font, TextToDisplay, CenteredPosition, TextColor, NO_ROTATION, Origin, Scale, SpriteEffects.None, NO_DEPTH_LAYER);
        }
    }
}
