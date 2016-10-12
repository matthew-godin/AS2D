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
    public class GameProject : Microsoft.Xna.Framework.Game
    {
        public const float FPS_INTERVAL = 1f;
        public const float STANDARD_INTERVAL = 1f / 60f;

        GraphicsDeviceManager GraphicsMgr { get; set; }
        SpriteBatch SpriteMgr { get; set; }
        RessourcesManager<SpriteFont> FontsMgr { get; set; }
        RessourcesManager<Texture2D> TexturesMgr { get; set; }
        InputManager InputMgr { get; set; }

        public GameProject()
        {
            GraphicsMgr = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GraphicsMgr.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            FontsMgr = new RessourcesManager<SpriteFont>(this, "Fonts");
            TexturesMgr = new RessourcesManager<Texture2D>(this, "Textures");
            InputMgr = new InputManager(this);
            SpriteMgr = new SpriteBatch(GraphicsDevice);

            Services.AddService(typeof(RessourcesManager<SpriteFont>), FontsMgr);
            Services.AddService(typeof(RessourcesManager<Texture2D>), TexturesMgr);
            Services.AddService(typeof(InputManager), InputMgr);
            Services.AddService(typeof(SpriteBatch), SpriteMgr);
            Services.AddService(typeof(Random), new Random());

            Components.Add(InputMgr);
            Components.Add(new NightSkyBackground(this, "NightSky", STANDARD_INTERVAL));
            Components.Add(new AS2D(this));
            Components.Add(new FPSDisplay(this, "Arial", Color.Tomato, FPS_INTERVAL));
            /* TEMPORARY */ Components.Add(new CenteredText(this, "Congratulations, you have reached level " + "4", "Arial", new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.Red, 0.2f));
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            CleanUpComponentList();
            ManageKeyboard();
            base.Update(gameTime);
        }

        void CleanUpComponentList()
        {
            for (int i = Components.Count - 1; i >= 0; --i)
            {
                if (Components[i] is IDestructible && ((IDestructible)Components[i]).ToDestroy)
                {
                    Components.RemoveAt(i);
                }
            }
        }

        private void ManageKeyboard()
        {
            if (InputMgr.IsPressed(Keys.Escape))
            {
                Exit();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SpriteMgr.Begin();
            base.Draw(gameTime);
            SpriteMgr.End();
        }
    }
}
