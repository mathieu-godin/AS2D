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

namespace AtelierXNA
{
    public class Atelier : Microsoft.Xna.Framework.Game
    {
        public const float INTERVALLE_CALCUL_FPS = 1f;
        public const float INTERVALLE_STANDARDS = 1f / 60f;

        GraphicsDeviceManager PériphériqueGraphique { get; set; }
        SpriteBatch GestionSprites { get; set; }
        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        InputManager GestionInput { get; set; }

        public Atelier()
        {
            PériphériqueGraphique = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            PériphériqueGraphique.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            GestionnaireDeFonts = new RessourcesManager<SpriteFont>(this, "Fonts");
            GestionnaireDeTextures = new RessourcesManager<Texture2D>(this, "Textures");
            GestionInput = new InputManager(this);
            GestionSprites = new SpriteBatch(GraphicsDevice);

            Services.AddService(typeof(RessourcesManager<SpriteFont>), GestionnaireDeFonts);
            Services.AddService(typeof(RessourcesManager<Texture2D>), GestionnaireDeTextures);
            Services.AddService(typeof(InputManager), GestionInput);
            Services.AddService(typeof(SpriteBatch), GestionSprites);
            Services.AddService(typeof(Random), new Random());

            Components.Add(GestionInput);
            Components.Add(new ArrièrePlanSpatial(this, "CielÉtoilé", INTERVALLE_STANDARDS));
            Components.Add(new Jeu(this));
            Components.Add(new AfficheurFPS(this, "Arial", Color.Tomato, INTERVALLE_CALCUL_FPS));
            /* TEMPORAIRE */ Components.Add(new TexteCentré(this, "Félicitations, vous avez atteint le niveau " + "4", "Arial", new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.Red, 0.2f));
            /* TEMPORAIRE */ Components.Add(new Sphère(this, "Sphère", Vector2.One, new Rectangle(0, 0, Window.ClientBounds.Width / 10, Window.ClientBounds.Height / 10), new Vector2(8, 4), 1.5f * Atelier.INTERVALLE_STANDARDS, Atelier.INTERVALLE_STANDARDS));
            /*Temporaire*/              Components.Add(new VaisseauSpatial(this,
            /*Temporaire*/                                                 "SpaceShip",
            /*Temporaire*/                                                  new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 4 * 3),
            /*Temporaire*/                                                  new Rectangle(0, 0, Window.ClientBounds.Width / 5, Window.ClientBounds.Height / 5),
            /*Temporaire*/                                                  new Vector2(4, 2),
            /*Temporaire*/                                                  6 * INTERVALLE_STANDARDS,
            /*Temporaire*/                                                  INTERVALLE_STANDARDS));
            Components.Add(new Missile(this, "Missile", new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 4 * 3), new Rectangle(0, 0, 34, 40), new Vector2(25, 1), "Explosion", new Vector2(5, 4), 1.5f * Atelier.INTERVALLE_STANDARDS, Atelier.INTERVALLE_STANDARDS));
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            NettoyerListeComponents();
            GérerClavier();
            base.Update(gameTime);
        }

        void NettoyerListeComponents()
        {
            for (int i = Components.Count - 1; i >= 0; --i)
            {
                if (Components[i] is IDestructible && ((IDestructible)Components[i]).ADétruire)
                {
                    Components.RemoveAt(i);
                }
            }
        }

        private void GérerClavier()
        {
            if (GestionInput.EstEnfoncée(Keys.Escape))
            {
                Exit();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GestionSprites.Begin();
            base.Draw(gameTime);
            GestionSprites.End();
        }
    }
}
