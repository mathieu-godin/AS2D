using System;
using System.Linq;
using Microsoft.Xna.Framework;


namespace AtelierXNA
{
   /// <summary>
   /// This is a game component that implements IUpdateable.
   /// </summary>
   public class Jeu : Microsoft.Xna.Framework.GameComponent
   {
      const float INTERVALLE_ANIMATION_LENT = 6 * Atelier.INTERVALLE_STANDARDS;
      const float INTERVALLE_ANIMATION_RAPIDE = 1.5f * Atelier.INTERVALLE_STANDARDS;
      int Niveau { get; set; }
      VaisseauSpatial Vaisseau { get; set; }
      Rectangle ZoneAffichage { get; set; }
      Rectangle ZoneVaisseau { get; set; }
      Rectangle ZoneSph�re { get; set; }
      Random G�n�rateurAl�atoire { get; set; }

      public Jeu(Game game)
          : base(game)
      {
      }

       public override void Initialize()
      {
         G�n�rateurAl�atoire = Game.Services.GetService(typeof(Random)) as Random;
         Vector2 centre = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 4 * 3);
         ZoneAffichage = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
         ZoneVaisseau = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 5, Game.Window.ClientBounds.Height / 5);
         ZoneSph�re = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 10, Game.Window.ClientBounds.Height / 10);
         Niveau = 1;
         Vaisseau = new VaisseauSpatial(Game, "SpaceShip", centre, ZoneVaisseau, new Vector2(4, 2), INTERVALLE_ANIMATION_LENT, Atelier.INTERVALLE_STANDARDS);
         Game.Components.Add(Vaisseau);
         Cr�erNiveau();
      }

      private void Cr�erNiveau()
      {
         for (int i = 0;i<Niveau;++i)
         {
            Sph�re nouvelleSph�re = new Sph�re(Game, "Sph�re", Vector2.One, ZoneSph�re, new Vector2(8, 4), INTERVALLE_ANIMATION_RAPIDE, Atelier.INTERVALLE_STANDARDS);
            Game.Components.Add(nouvelleSph�re);
            
         }
      }

      public override void Update(GameTime gameTime)
      {
         if (Vaisseau != null)
         {
            if (Vaisseau.AD�truire)
            {
               Vaisseau = null;
               Game.Components.Add(new TexteCentr�(Game, "F�licitations, vous avez atteint le niveau " + Niveau.ToString(), "Arial", ZoneAffichage, Color.Red, 0.2f));
            }
            else
            {
               G�rerCollision();
            }
         }
      }

      private void G�rerCollision()
      {
         G�rerCollisionMissile();
         G�rerCollisionSph�re();
         G�rerFinDeNiveau();
      }

      private void G�rerCollisionMissile()
      {
         foreach (Missile missile in Game.Components.Where(composant => composant is Missile && !((Missile)composant).ExplosionActiv�e))
         {
            foreach (Sph�re sph�re in Game.Components.Where(composant => composant is Sph�re))
            {
               if (missile.EstEnCollision(sph�re))
               {
                  sph�re.AD�truire = true;
                  missile.ActiverExplosion();
               }
            }
         }
      }

      private void G�rerCollisionSph�re()
      {
         foreach (Sph�re sph�re in Game.Components.Where(composant => composant is Sph�re))
         {
            if (Vaisseau.EstEnCollision(sph�re))
            {
               sph�re.AD�truire = true;
               Vaisseau.AD�truire = true;
            }
         }
      }

      private void G�rerFinDeNiveau()
      {
         int nbSph�resActives = Game.Components.Count(composant => composant is Sph�re && !((Sph�re)composant).AD�truire);
         if (nbSph�resActives == 0 && !Vaisseau.AD�truire)
         {
            ++Niveau;
            Cr�erNiveau();
         }
      }
   }
}
