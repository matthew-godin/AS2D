using System;
using System.Linq;
using Microsoft.Xna.Framework;


namespace XNAProject
{
    enum States { GAME, SHIP_DESTRUCTION, NEW_SHIP, NEW_SWARM, END }
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AS2D : Microsoft.Xna.Framework.GameComponent
    {
        const int MAX_LIVES = 3;
        const float SLOW_ANIMATION_INTERVAL = 6 * GameProject.STANDARD_INTERVAL;
        const float FAST_ANIMATION_INTERVAL = 1.5f * GameProject.STANDARD_INTERVAL;
        int Level { get; set; }
        Spaceship Ship { get; set; }
        AnimatedSprite Explosion { get; set; }
        bool ExplosionActivated { get; set; }
        float TimeSpentSinceUpdateExplosion { get; set; }
        int ExplosionPhase { get; set; }
        Rectangle DisplayZone { get; set; }
        Rectangle ShipZone { get; set; }
        Rectangle SphereZone { get; set; }
        Rectangle LivesZone { get; set; }
        Random RandomNumberGenerator { get; set; }
        Vector2 ExplosionDescription { get; set; }
        Vector2 Center { get; set; }
        Sprite[] Lives { get; set; }
        int NumLives { get; set; }
        States GameState { get; set; }

        public AS2D(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            RandomNumberGenerator = Game.Services.GetService(typeof(Random)) as Random;
            Center = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 4 * 3);
            DisplayZone = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            ShipZone = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 5, Game.Window.ClientBounds.Height / 5);
            SphereZone = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 10, Game.Window.ClientBounds.Height / 10);
            LivesZone = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 20, Game.Window.ClientBounds.Height / 20);
            ExplosionDescription = new Vector2(5, 4);
            InitializeLives();
            Level = 1;
            NumLives = 3;
            CreateShip();
            CreateLevel();
            GameState = States.GAME;
        }

        private void CreateShip()
        {
            Ship = new Spaceship(Game, "Spaceship", Center, ShipZone, new Vector2(4, 2), SLOW_ANIMATION_INTERVAL, GameProject.STANDARD_INTERVAL);
            Game.Components.Add(Ship);
        }

        private void InitializeLives()
        {
            Lives = new Sprite[MAX_LIVES];
            for (int i = 0; i < MAX_LIVES; ++i)
            {
                Lives[i] = new Sprite(Game, "ShipIcon", new Vector2(Game.Window.ClientBounds.Width - (i + 1) * LivesZone.Width, LivesZone.Height), LivesZone);
                Game.Components.Add(Lives[i]);
            }
        }

        private void CreateLevel()
        {
            for (int i = 0; i < Level; ++i)
            {
                Sphere newSphere = new Sphere(Game, "Sphere", Vector2.One, SphereZone, new Vector2(8, 4), FAST_ANIMATION_INTERVAL, GameProject.STANDARD_INTERVAL);
                Game.Components.Add(newSphere);
            }
        }

        public override void Update(GameTime gameTime)
        {
            bool shipActive = !Ship.ToDestroy;
            int numEnemies = Game.Components.Count(x => x is Sphere && !((Sphere)x).ToDestroy);
            ManageTransition(shipActive, numEnemies);
            ManageState(shipActive, numEnemies, gameTime);

        }

        private void ManageState(bool shipActive, int numEnemies, GameTime gameTime)
        {
            switch (GameState)
            {
                case States.GAME:
                    ManageCollision();
                    break;
                case States.SHIP_DESTRUCTION:
                    ManageExplosion(gameTime);
                    break;
                case States.NEW_SHIP:
                    CreateShip();
                    break;
                case States.NEW_SWARM:
                    CreateLevel();
                    break;
                default:
                    DisplayCongratulationsMessage();
                    break;
            }
        }

        private void DisplayCongratulationsMessage()
        {
            Game.Components.Add(new TexteCentre(Game, "Congratulations, you have reached level " + Level.ToString(), "Arial", DisplayZone, Color.Red, 0.2f));
        }

        private void ManageTransition(bool shipActive, int numEnemies)
        {
            switch (GameState)
            {
                case States.GAME:
                    ManageTransitionGAME(shipActive, numEnemies);
                    break;
                case States.SHIP_DESTRUCTION:
                    ManageTransitionSHIP_DESTRUCTION();
                    break;
                case States.NEW_SHIP:
                    ManageTransitionNEW_SHIP(numEnemies);
                    break;
                default:
                    GameState = States.GAME;
                    break;
            }
        }

        private void ManageTransitionGAME(bool shipActive, int numEnemies)
        {
            if (ExplosionActivated)
            {
                ActivateExplosion();
                GameState = States.SHIP_DESTRUCTION;
            }
            else
            {
                if (numEnemies == 0)
                {
                    ++Level;
                    GameState = States.NEW_SWARM;
                }
            }
        }

        private void ManageTransitionSHIP_DESTRUCTION()
        {
            if (!ExplosionActivated)
            {
                --NumLives;
                Game.Components.Remove(Lives[NumLives]);
                if (NumLives > 0)
                {
                    GameState = States.NEW_SHIP;
                }
                else
                {
                    GameState = States.END;
                }
            }
        }

        private void ManageTransitionNEW_SHIP(int numEnemies)
        {
            if (numEnemies == 0)
            {
                ++Level;
            }
            else
            {
                foreach (Sphere enemy in Game.Components.Where(x => x is Sphere))
                {
                    enemy.ToDestroy = true;
                }
            }
            GameState = States.NEW_SWARM;
        }

        private void ManageExplosion(GameTime gameTime)
        {
            float TimeElapased = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeSpentSinceUpdateExplosion += TimeElapased;
            if (TimeSpentSinceUpdateExplosion >= SLOW_ANIMATION_INTERVAL)
            {
                ++ExplosionPhase;
                TimeSpentSinceUpdateExplosion = 0;
                if (ExplosionPhase >= ExplosionDescription.X * ExplosionDescription.Y)
                {
                    ExplosionActivated = false;
                    Explosion.ToDestroy = true;
                }
            }

        }

        private void ActivateExplosion()
        {
            Explosion = new AnimatedSprite(Game, "Explosion", Ship.Position, ShipZone, ExplosionDescription, SLOW_ANIMATION_INTERVAL);
            Game.Components.Add(Explosion);
            ExplosionActivated = true;
            TimeSpentSinceUpdateExplosion = 0;

        }

        private void ManageCollision()
        {
            ManageCollisionMissile();
            ManageCollisionSphere();
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
            foreach (Sphere sphere in Game.Components.Where(component => component is Sphere && !((Sphere)component).ToDestroy))
            {
                if (!Ship.ToDestroy && Ship.IsColliding(sphere))
                {
                    sphere.ToDestroy = true;
                    Ship.ToDestroy = true;
                    ExplosionActivated = true;
                }
            }
        }
    }
}