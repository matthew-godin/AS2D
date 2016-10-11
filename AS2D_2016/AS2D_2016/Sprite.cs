/*
Sprite.cs
---------

By Matthew Godin

Role : DrawableGameComponent
       inheriting from ICollidable that displays
       a sprite using Texture2D

Created : 5 October 2016
*/
/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
{
    public class Sprite : Microsoft.Xna.Framework.DrawableGameComponent, ICollidable
    {
        //Properties initially managed by the constructor
        string ImageName { get; set; }
        protected Vector2 Position { get; set; }
        protected Rectangle DisplayZone { get; set; }

        //Properties initially managed by LoadContent
        protected SpriteBatch SpriteMgr { get; private set; }
        RessourcesManager<Texture2D> TexturesMgr { get; set; }
        protected Texture2D Image { get; private set; }

        public Sprite(Game game, string imageName, Vector2 position, Rectangle displayZone) : base(game)
        {
            ImageName = imageName;
            Position = position;
            DisplayZone = displayZone;
        }

        protected override void LoadContent()
        {
            SpriteMgr = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            TexturesMgr = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Image = TexturesMgr.Find(ImageName);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteMgr.Draw(Image, Position, Color.White);
        }
        public bool IsColliding(object otherObject)
        {
            //To implement
        }
    }
}
*/