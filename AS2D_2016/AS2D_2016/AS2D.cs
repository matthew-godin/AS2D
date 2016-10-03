using System;
using System.Linq;
using Microsoft.Xna.Framework;


namespace XNAProject
{
   /// <summary>
   /// This is a game component that implements IUpdateable.
   /// </summary>
   public class AS2D : Microsoft.Xna.Framework.GameComponent
   {
      const float SLOW_ANIMATION_INTERVAL = 6 * GameProject.STANDARD_INTERVAL;
      const float FAST_ANIMATION_INTERVAL = 1.5f * GameProject.STANDARD_INTERVAL;
      int Level { get; set; }
      Spaceship Ship { get; set; }
      Rectangle DisplayZone { get; set; }
      Rectangle ShipZone { get; set; }
      Rectangle SphereZone { get; set; }
      Random RandomNumberGenerator { get; set; }

      public AS2D(Game game)
          : base(game)
      {
      }

       public override void Initialize()
      {
         RandomNumberGenerator = Game.Services.GetService(typeof(Random)) as Random;
         Vector2 centre = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 4 * 3);
         DisplayZone = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
         ShipZone = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 5, Game.Window.ClientBounds.Height / 5);
         SphereZone = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 10, Game.Window.ClientBounds.Height / 10);
         Level = 1;
         Ship = new Spaceship(Game, "Spaceship", centre, ShipZone, new Vector2(4, 2), SLOW_ANIMATION_INTERVAL, GameProject.STANDARD_INTERVAL);
         Game.Components.Add(Ship);
         CreateLevel();
      }

      private void CreateLevel()
      {
         for (int i = 0;i<Level;++i)
         {
            Sphere newSphere = new Sphere(Game, "Sphere", Vector2.One, SphereZone, new Vector2(8, 4), FAST_ANIMATION_INTERVAL, GameProject.STANDARD_INTERVAL);
            Game.Components.Add(newSphere);
            
         }
      }

      public override void Update(GameTime gameTime)
      {
         if (Ship != null)
         {
            if (Ship.ToDestroy)
            {
               Ship = null;
               Game.Components.Add(new TexteCentre(Game, "Congratulations, you have reached level " + Level.ToString(), "Arial", DisplayZone, Color.Red, 0.2f));
            }
            else
            {
               ManageCollision();
            }
         }
      }

      private void ManageCollision()
      {
         ManageCollisionMissile();
         ManageCollisionSphere();
         ManageEndLevel();
      }

      private void ManageCollisionMissile()
      {
         foreach (Missile missile in Game.Components.Where(component => component is Missile && !((Missile)component).ExplosionActivated))
         {
            foreach (Sphere sphere in Game.Components.Where(component => component is Sphere))
            {
               if (missile.IsColliding(sphere))
               {
                  sphere.ToDestroy = true;
                  missile.ActivateExplosion();
               }
            }
         }
      }

      private void ManageCollisionSphere()
      {
         foreach (Sphere sphere in Game.Components.Where(component => component is Sphere))
         {
            if (Ship.IsColliding(sphere))
            {
               sphere.ToDestroy = true;
               Ship.ToDestroy = true;
            }
         }
      }

      private void ManageEndLevel()
      {
         int numSpheresActive = Game.Components.Count(component => component is Sphere && !((Sphere)component).ToDestroy);
         if (numSpheresActive == 0 && !Ship.ToDestroy)
         {
            ++Level;
            CreateLevel();
         }
      }
   }
}
