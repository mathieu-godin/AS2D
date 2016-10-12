using System;
using System.Linq;
using Microsoft.Xna.Framework;


namespace AtelierXNA
{
    enum États { JEU, DESTRUCTION_VAISSEAU, NOUVEAU_VAISSEAU, NOUVELLE_VAGUE, FIN }
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Jeu : Microsoft.Xna.Framework.GameComponent
    {
        const int MAX_VIE = 3;
        const float INTERVALLE_ANIMATION_LENT = 6 * Atelier.INTERVALLE_STANDARDS;
        const float INTERVALLE_ANIMATION_RAPIDE = 1.5f * Atelier.INTERVALLE_STANDARDS;
        int Niveau { get; set; }
        //VaisseauSpatial Vaisseau { get; set; }
        //SpriteAnimé Explosion { get; set; }
        bool ExplosionActivé { get; set; }
        float TempsÉcouléDepuisMAJExplosion { get; set; }
        int PhaseExplosion { get; set; }
        Rectangle ZoneAffichage { get; set; }
        Rectangle ZoneVaisseau { get; set; }
        Rectangle ZoneSphère { get; set; }
        Rectangle ZoneVie { get; set; }
        Random GénérateurAléatoire { get; set; }
        Vector2 DescriptionExplosion { get; set; }
        Vector2 Centre { get; set; }
        //Sprite[] Vies { get; set; }
        int NbVies { get; set; }
        États ÉtatJeu { get; set; }

        public Jeu(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            GénérateurAléatoire = Game.Services.GetService(typeof(Random)) as Random;
            Centre = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 4 * 3);
            ZoneAffichage = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            ZoneVaisseau = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 5, Game.Window.ClientBounds.Height / 5);
            ZoneSphère = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 10, Game.Window.ClientBounds.Height / 10);
            ZoneVie = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 20, Game.Window.ClientBounds.Height / 20);
            DescriptionExplosion = new Vector2(5, 4);
            //InitialiserVies();
            Niveau = 1;
            NbVies = 3;
            //CréerVaisseau();
            //CréerNiveau();
            ÉtatJeu = États.JEU;
        }

        /*private void CréerVaisseau()
        {
            Vaisseau = new VaisseauSpatial(Game, "SpaceShip", Centre, ZoneVaisseau, new Vector2(4, 2), INTERVALLE_ANIMATION_LENT, Atelier.INTERVALLE_STANDARDS);
            Game.Components.Add(Vaisseau);
        }

        private void InitialiserVies()
        {
            Vies = new Sprite[MAX_VIE];
            for (int i = 0; i < MAX_VIE; ++i)
            {
                Vies[i] = new Sprite(Game, "IcôneVaisseau", new Vector2(Game.Window.ClientBounds.Width - (i + 1) * ZoneVie.Width, ZoneVie.Height), ZoneVie);
                Game.Components.Add(Vies[i]);
            }
        }

        private void CréerNiveau()
        {
            for (int i = 0; i < Niveau; ++i)
            {
                Sphère nouvelleSphère = new Sphère(Game, "Sphère", Vector2.One, ZoneSphère, new Vector2(8, 4), INTERVALLE_ANIMATION_RAPIDE, Atelier.INTERVALLE_STANDARDS);
                Game.Components.Add(nouvelleSphère);
            }
        }*/

        public override void Update(GameTime gameTime)
        {
            //bool vaisseauActif = !Vaisseau.ADétruire;
            //int nbEnnemis = Game.Components.Count(x => x is Sphère && !((Sphère)x).ADétruire);
            //GérerTransition(vaisseauActif, nbEnnemis);
            //GérerÉtat(vaisseauActif, nbEnnemis, gameTime);

        }

        private void GérerÉtat(bool vaisseauActif, int nbEnnemis, GameTime gameTime)
        {
            switch (ÉtatJeu)
            {
                case États.JEU:
                    //GérerCollision();
                    break;
                case États.DESTRUCTION_VAISSEAU:
                    //GérerExplosion(gameTime);
                    break;
                case États.NOUVEAU_VAISSEAU:
                    //CréerVaisseau();
                    break;
                case États.NOUVELLE_VAGUE:
                    //CréerNiveau();
                    break;
                default:
                    AfficherMessageFélicitations();
                    break;
            }
        }

        private void AfficherMessageFélicitations()
        {
            //Game.Components.Add(new TexteCentré(Game, "Félicitations, vous avez atteint le niveau " + Niveau.ToString(), "Arial", ZoneAffichage, Color.Red, 0.2f));
        }

        private void GérerTransition(bool vaisseauActif, int nbEnnemis)
        {
            switch (ÉtatJeu)
            {
                case États.JEU:
                    GérerTransitionJEU(vaisseauActif, nbEnnemis);
                    break;
                case États.DESTRUCTION_VAISSEAU:
                    //GérerTransitionDESTRUCTION_VAISSEAU();
                    break;
                case États.NOUVEAU_VAISSEAU:
                    //GérerTransitionNOUVEAU_VAISSEAU(nbEnnemis);
                    break;
                default:
                    ÉtatJeu = États.JEU;
                    break;
            }
        }

        private void GérerTransitionJEU(bool vaisseauActif, int nbEnnemis)
        {
            if (ExplosionActivé)
            {
                //ActiverExplosion();
                ÉtatJeu = États.DESTRUCTION_VAISSEAU;
            }
            else
            {
                if (nbEnnemis == 0)
                {
                    ++Niveau;
                    ÉtatJeu = États.NOUVELLE_VAGUE;
                }
            }
        }

        /*private void GérerTransitionDESTRUCTION_VAISSEAU()
        {
            if (!ExplosionActivé)
            {
                --NbVies;
                Game.Components.Remove(Vies[NbVies]);
                if (NbVies > 0)
                {
                    ÉtatJeu = États.NOUVEAU_VAISSEAU;
                }
                else
                {
                    ÉtatJeu = États.FIN;
                }
            }
        }*/

        /*private void GérerTransitionNOUVEAU_VAISSEAU(int nbEnnemis)
        {
            if (nbEnnemis == 0)
            {
                ++Niveau;
            }
            else
            {
                foreach (Sphère ennemi in Game.Components.Where(x => x is Sphère))
                {
                    ennemi.ADétruire = true;
                }
            }
            ÉtatJeu = États.NOUVELLE_VAGUE;
        }*/

        /*private void GérerExplosion(GameTime gameTime)
        {
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJExplosion += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJExplosion >= INTERVALLE_ANIMATION_LENT)
            {
                ++PhaseExplosion;
                TempsÉcouléDepuisMAJExplosion = 0;
                if (PhaseExplosion >= DescriptionExplosion.X * DescriptionExplosion.Y)
                {
                    ExplosionActivé = false;
                    Explosion.ADétruire = true;
                }
            }

        }*/

        /*private void ActiverExplosion()
        {
            Explosion = new SpriteAnimé(Game, "Explosion", Vaisseau.Position, ZoneVaisseau, DescriptionExplosion, INTERVALLE_ANIMATION_LENT);
            Game.Components.Add(Explosion);
            ExplosionActivé = true;
            TempsÉcouléDepuisMAJExplosion = 0;

        }*/

        /*private void GérerCollision()
        {
            GérerCollisionMissile();
            GérerCollisionSphère();
        }*/

        /*private void GérerCollisionMissile()
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
        }*/

        /*private void GérerCollisionSphère()
        {
            foreach (Sphère sphère in Game.Components.Where(composant => composant is Sphère && !((Sphère)composant).ADétruire))
            {
                if (!Vaisseau.ADétruire && Vaisseau.EstEnCollision(sphère))
                {
                    sphère.ADétruire = true;
                    Vaisseau.ADétruire = true;
                    ExplosionActivé = true;
                }
            }
        }*/
    }
}