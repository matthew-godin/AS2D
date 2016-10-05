
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XNAProject
{
    public class Sprite : Microsoft.Xna.Framework.DrawableGameComponent, ICollidable
    {
        string ImageName { get; set; }
        protected Vector2 Position { get; set; }
        protected Texture2D Image { get; private set; }
        protected SpriteBatch SpriteMgr { get; private set; }
        RessourcesManager<Texture2D> TexturesMgr { get; set; }
        protected Rectangle DisplayZone { get; set; }

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
    }
}
