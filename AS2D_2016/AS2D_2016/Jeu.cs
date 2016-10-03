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
      Rectangle ZoneSphère { get; set; }
      Random GénérateurAléatoire { get; set; }

      public Jeu(Game game)
          : base(game)
      {
      }

       public override void Initialize()
      {
         GénérateurAléatoire = Game.Services.GetService(typeof(Random)) as Random;
         Vector2 centre = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 4 * 3);
         ZoneAffichage = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
         ZoneVaisseau = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 5, Game.Window.ClientBounds.Height / 5);
         ZoneSphère = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 10, Game.Window.ClientBounds.Height / 10);
         Niveau = 1;
         Vaisseau = new VaisseauSpatial(Game, "SpaceShip", centre, ZoneVaisseau, new Vector2(4, 2), INTERVALLE_ANIMATION_LENT, Atelier.INTERVALLE_STANDARDS);
         Game.Components.Add(Vaisseau);
         CréerNiveau();
      }

      private void CréerNiveau()
      {
         for (int i = 0;i<Niveau;++i)
         {
            Sphère nouvelleSphère = new Sphère(Game, "Sphère", Vector2.One, ZoneSphère, new Vector2(8, 4), INTERVALLE_ANIMATION_RAPIDE, Atelier.INTERVALLE_STANDARDS);
            Game.Components.Add(nouvelleSphère);
            
         }
      }

      public override void Update(GameTime gameTime)
      {
         if (Vaisseau != null)
         {
            if (Vaisseau.ADétruire)
            {
               Vaisseau = null;
               Game.Components.Add(new TexteCentré(Game, "Félicitations, vous avez atteint le niveau " + Niveau.ToString(), "Arial", ZoneAffichage, Color.Red, 0.2f));
            }
            else
            {
               GérerCollision();
            }
         }
      }

      private void GérerCollision()
      {
         GérerCollisionMissile();
         GérerCollisionSphère();
         GérerFinDeNiveau();
      }

      private void GérerCollisionMissile()
      {
         foreach (Missile missile in Game.Components.Where(composant => composant is Missile && !((Missile)composant).ExplosionActivée))
         {
            foreach (Sphère sphère in Game.Components.Where(composant => composant is Sphère))
            {
               if (missile.EstEnCollision(sphère))
               {
                  sphère.ADétruire = true;
                  missile.ActiverExplosion();
               }
            }
         }
      }

      private void GérerCollisionSphère()
      {
         foreach (Sphère sphère in Game.Components.Where(composant => composant is Sphère))
         {
            if (Vaisseau.EstEnCollision(sphère))
            {
               sphère.ADétruire = true;
               Vaisseau.ADétruire = true;
            }
         }
      }

      private void GérerFinDeNiveau()
      {
         int nbSphèresActives = Game.Components.Count(composant => composant is Sphère && !((Sphère)composant).ADétruire);
         if (nbSphèresActives == 0 && !Vaisseau.ADétruire)
         {
            ++Niveau;
            CréerNiveau();
         }
      }
   }
}
