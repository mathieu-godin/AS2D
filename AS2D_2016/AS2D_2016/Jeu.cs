using System;
using System.Linq;
using Microsoft.Xna.Framework;


namespace AtelierXNA
{
    enum �tats { JEU, DESTRUCTION_VAISSEAU, NOUVEAU_VAISSEAU, NOUVELLE_VAGUE, FIN }
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
        //SpriteAnim� Explosion { get; set; }
        bool ExplosionActiv� { get; set; }
        float Temps�coul�DepuisMAJExplosion { get; set; }
        int PhaseExplosion { get; set; }
        Rectangle ZoneAffichage { get; set; }
        Rectangle ZoneVaisseau { get; set; }
        Rectangle ZoneSph�re { get; set; }
        Rectangle ZoneVie { get; set; }
        Random G�n�rateurAl�atoire { get; set; }
        Vector2 DescriptionExplosion { get; set; }
        Vector2 Centre { get; set; }
        //Sprite[] Vies { get; set; }
        int NbVies { get; set; }
        �tats �tatJeu { get; set; }

        public Jeu(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            G�n�rateurAl�atoire = Game.Services.GetService(typeof(Random)) as Random;
            Centre = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 4 * 3);
            ZoneAffichage = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            ZoneVaisseau = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 5, Game.Window.ClientBounds.Height / 5);
            ZoneSph�re = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 10, Game.Window.ClientBounds.Height / 10);
            ZoneVie = new Rectangle(0, 0, Game.Window.ClientBounds.Width / 20, Game.Window.ClientBounds.Height / 20);
            DescriptionExplosion = new Vector2(5, 4);
            //InitialiserVies();
            Niveau = 1;
            NbVies = 3;
            //Cr�erVaisseau();
            //Cr�erNiveau();
            �tatJeu = �tats.JEU;
        }

        /*private void Cr�erVaisseau()
        {
            Vaisseau = new VaisseauSpatial(Game, "SpaceShip", Centre, ZoneVaisseau, new Vector2(4, 2), INTERVALLE_ANIMATION_LENT, Atelier.INTERVALLE_STANDARDS);
            Game.Components.Add(Vaisseau);
        }

        private void InitialiserVies()
        {
            Vies = new Sprite[MAX_VIE];
            for (int i = 0; i < MAX_VIE; ++i)
            {
                Vies[i] = new Sprite(Game, "Ic�neVaisseau", new Vector2(Game.Window.ClientBounds.Width - (i + 1) * ZoneVie.Width, ZoneVie.Height), ZoneVie);
                Game.Components.Add(Vies[i]);
            }
        }

        private void Cr�erNiveau()
        {
            for (int i = 0; i < Niveau; ++i)
            {
                Sph�re nouvelleSph�re = new Sph�re(Game, "Sph�re", Vector2.One, ZoneSph�re, new Vector2(8, 4), INTERVALLE_ANIMATION_RAPIDE, Atelier.INTERVALLE_STANDARDS);
                Game.Components.Add(nouvelleSph�re);
            }
        }*/

        public override void Update(GameTime gameTime)
        {
            //bool vaisseauActif = !Vaisseau.AD�truire;
            //int nbEnnemis = Game.Components.Count(x => x is Sph�re && !((Sph�re)x).AD�truire);
            //G�rerTransition(vaisseauActif, nbEnnemis);
            //G�rer�tat(vaisseauActif, nbEnnemis, gameTime);

        }

        private void G�rer�tat(bool vaisseauActif, int nbEnnemis, GameTime gameTime)
        {
            switch (�tatJeu)
            {
                case �tats.JEU:
                    //G�rerCollision();
                    break;
                case �tats.DESTRUCTION_VAISSEAU:
                    //G�rerExplosion(gameTime);
                    break;
                case �tats.NOUVEAU_VAISSEAU:
                    //Cr�erVaisseau();
                    break;
                case �tats.NOUVELLE_VAGUE:
                    //Cr�erNiveau();
                    break;
                default:
                    AfficherMessageF�licitations();
                    break;
            }
        }

        private void AfficherMessageF�licitations()
        {
            //Game.Components.Add(new TexteCentr�(Game, "F�licitations, vous avez atteint le niveau " + Niveau.ToString(), "Arial", ZoneAffichage, Color.Red, 0.2f));
        }

        private void G�rerTransition(bool vaisseauActif, int nbEnnemis)
        {
            switch (�tatJeu)
            {
                case �tats.JEU:
                    G�rerTransitionJEU(vaisseauActif, nbEnnemis);
                    break;
                case �tats.DESTRUCTION_VAISSEAU:
                    //G�rerTransitionDESTRUCTION_VAISSEAU();
                    break;
                case �tats.NOUVEAU_VAISSEAU:
                    //G�rerTransitionNOUVEAU_VAISSEAU(nbEnnemis);
                    break;
                default:
                    �tatJeu = �tats.JEU;
                    break;
            }
        }

        private void G�rerTransitionJEU(bool vaisseauActif, int nbEnnemis)
        {
            if (ExplosionActiv�)
            {
                //ActiverExplosion();
                �tatJeu = �tats.DESTRUCTION_VAISSEAU;
            }
            else
            {
                if (nbEnnemis == 0)
                {
                    ++Niveau;
                    �tatJeu = �tats.NOUVELLE_VAGUE;
                }
            }
        }

        /*private void G�rerTransitionDESTRUCTION_VAISSEAU()
        {
            if (!ExplosionActiv�)
            {
                --NbVies;
                Game.Components.Remove(Vies[NbVies]);
                if (NbVies > 0)
                {
                    �tatJeu = �tats.NOUVEAU_VAISSEAU;
                }
                else
                {
                    �tatJeu = �tats.FIN;
                }
            }
        }*/

        /*private void G�rerTransitionNOUVEAU_VAISSEAU(int nbEnnemis)
        {
            if (nbEnnemis == 0)
            {
                ++Niveau;
            }
            else
            {
                foreach (Sph�re ennemi in Game.Components.Where(x => x is Sph�re))
                {
                    ennemi.AD�truire = true;
                }
            }
            �tatJeu = �tats.NOUVELLE_VAGUE;
        }*/

        /*private void G�rerExplosion(GameTime gameTime)
        {
            float Temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJExplosion += Temps�coul�;
            if (Temps�coul�DepuisMAJExplosion >= INTERVALLE_ANIMATION_LENT)
            {
                ++PhaseExplosion;
                Temps�coul�DepuisMAJExplosion = 0;
                if (PhaseExplosion >= DescriptionExplosion.X * DescriptionExplosion.Y)
                {
                    ExplosionActiv� = false;
                    Explosion.AD�truire = true;
                }
            }

        }*/

        /*private void ActiverExplosion()
        {
            Explosion = new SpriteAnim�(Game, "Explosion", Vaisseau.Position, ZoneVaisseau, DescriptionExplosion, INTERVALLE_ANIMATION_LENT);
            Game.Components.Add(Explosion);
            ExplosionActiv� = true;
            Temps�coul�DepuisMAJExplosion = 0;

        }*/

        /*private void G�rerCollision()
        {
            G�rerCollisionMissile();
            G�rerCollisionSph�re();
        }*/

        /*private void G�rerCollisionMissile()
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
        }*/

        /*private void G�rerCollisionSph�re()
        {
            foreach (Sph�re sph�re in Game.Components.Where(composant => composant is Sph�re && !((Sph�re)composant).AD�truire))
            {
                if (!Vaisseau.AD�truire && Vaisseau.EstEnCollision(sph�re))
                {
                    sph�re.AD�truire = true;
                    Vaisseau.AD�truire = true;
                    ExplosionActiv� = true;
                }
            }
        }*/
    }
}