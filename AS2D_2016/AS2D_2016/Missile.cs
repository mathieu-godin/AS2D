/*
Missile.cs
----------

Par Mathieu Godin

Rôle : Composant qui hérite de SpriteAnimé
       qui gère le missile pouvant se déplacer
       vers le haut en accélérant et son explosion

Créé : 5 octobre 2016
Co-auteur : Raphaël Brûlé
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
    /// <summary>
    /// Composant qui hérite de SpriteAnimé
    /// </summary>
    public class Missile : SpriteAnimé
    {
        const float INTERVALLE_ANIMATION_LENT = 6 * Atelier.INTERVALLE_STANDARDS, CHANGEMENT_INTERVALLE_POUR_ACCÉLÉRATION = 1 / 4000F, DÉPLACEMENT_ORDONNÉE_MAJ = 4.0F;
        const int AVANT_PREMIÈRE_PHASE_EXPLOSION = 0, DIMENSION_EXPLOSION = 40;
        const string CHAÎNE_IMAGE_EXPLOSION = "Explosion";
        
        float IntervalleMAJDéplacement { get; set; }
        string NomImageExplosion { get; set; }
        Vector2 DescriptionImageExplosion { get; set; }
        Texture2D ImageExplosion { get; set; }
        float TempsÉcouléDepuisMAJDéplacement { get; set; }
        int PhaseExplosion { get; set; }
        Vector2 VecteurDéplacementMAJ { get; set; }
        bool Collision { get; set; }
        Rectangle ZoneExplosion { get; set; }
        SpriteAnimé Explosion { get; set; }
        int NombreDePhasesExplosion { get; set; }
        public bool ExplosionActivée { get; private set; }

        /// <summary>
        /// Constructeur de Sphère
        /// </summary>
        /// <param name="jeu">Objet de classe Game</param>
        /// <param name="nomImage">Nom de l'image (string)</param>
        /// <param name="position">Position (Vector2)</param>
        /// <param name="zoneAffichage">Zone d'affichage (Rectangle)</param>
        /// <param name="descriptionImage">Description de l'image (Vector2)</param>
        /// <param name="intervalleMAJAnimation">Intervalle de mise à jour de l'animation (float)</param>
        /// <param name="intervalleMAJDéplacement">Intervalle de mise à jour du déplacement (float)</param>
        public Missile(Game jeu, string nomImageMissile, Vector2 position, Rectangle zoneAffichage, Vector2 descriptionImageMissile, string nomImageExplosion, Vector2 descriptionImageExplosion, float intervalleMAJAnimation, float intervalleMAJDéplacement) : base(jeu, nomImageMissile, position, zoneAffichage, descriptionImageMissile, intervalleMAJAnimation)
        {
            IntervalleMAJDéplacement = intervalleMAJDéplacement;
            NomImageExplosion = nomImageExplosion;
            DescriptionImageExplosion = descriptionImageExplosion;
        }

        /// <summary>
        /// Initialise les composants de Missile
        /// </summary>
        public override void Initialize()
        {
            LoadContent();
            base.Initialize();
            TempsÉcouléDepuisMAJDéplacement = AUCUN_TEMPS_ÉCOULÉ;
            ExplosionActivée = false;
            VecteurDéplacementMAJ = new Vector2(ABSCISSE_NULLE, DÉPLACEMENT_ORDONNÉE_MAJ);
            Collision = false;
            ZoneExplosion = new Rectangle(ABSCISSE_NULLE, ORDONNÉE_NULLE, DIMENSION_EXPLOSION, DIMENSION_EXPLOSION);
        }

        /// <summary>
        /// Met à jour le Missile
        /// </summary>
        /// <param name="gameTime">Contient les informations de temps de jeu</param>
        public override void Update(GameTime gameTime)
        {
            VérifierExplosionActivée(gameTime);
            VérifierCollision();
        }

        /// <summary>
        /// Choisit le bon fonctionnement de la structure de temps en fonction de si l'explosion est activée, soit ExplosionActivée == true
        /// </summary>
        /// <param name="gameTime">Contient les informations de temps de jeu</param>
        void VérifierExplosionActivée(GameTime gameTime)
        {
            if (ExplosionActivée)
            {
                MettreÀJourExplosion(gameTime);
            }
            else
            {
                MettreÀJourMissile(gameTime);   
            }
        }

        /// <summary>
        /// Appelé si le Missile est en explosion, soit ExplosionActivée == true
        /// </summary>
        /// <param name="gameTime">Contient les informations de temps de jeu</param>
        void MettreÀJourExplosion(GameTime gameTime)
        {
            TempsÉcouléDepuisMAJDéplacement += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléDepuisMAJDéplacement >= INTERVALLE_ANIMATION_LENT)
            {
                TempsÉcouléDepuisMAJDéplacement = AUCUN_TEMPS_ÉCOULÉ;
                GérerExplosion();
            }
        }

        /// <summary>
        /// Appelé si le Missile est à son état normal, soit ExplosionActivée == false
        /// </summary>
        /// <param name="gameTime">Contient les informations de temps de jeu</param>
        void MettreÀJourMissile(GameTime gameTime)
        {
            TempsÉcouléDepuisMAJDéplacement += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléDepuisMAJDéplacement >= IntervalleMAJDéplacement)
            {
                TempsÉcouléDepuisMAJDéplacement = AUCUN_TEMPS_ÉCOULÉ;
                base.Update(gameTime);
                EffectuerMiseÀJourDéplacement();
            }
        }

        /// <summary>
        /// Vérifie si Collision == true et crée l'explosion si c'est le cas en remettant Collison = false
        /// </summary>
        void VérifierCollision()
        {
            if (Collision)
            {
                Collision = false;
                Game.Components.Add(Explosion);
            }
        }

        /// <summary>
        /// Méthode qui met à jour le déplacement du Missile selon le temps écoulé
        /// </summary>
        protected virtual void EffectuerMiseÀJourDéplacement()
        {
            Position -= VecteurDéplacementMAJ;
            IntervalleMAJDéplacement -= CHANGEMENT_INTERVALLE_POUR_ACCÉLÉRATION;
            CalculerRectangleImageÀAfficher();
            VérifierCollisionPlafond();
        }

        /// <summary>
        /// Vérifie si il y a collison avec le plafond et active l'explosion si c'est le cas
        /// </summary>
        void VérifierCollisionPlafond()
        {
            if (Position.Y <= MargeHaut && !ExplosionActivée)
            {
                ActiverExplosion();
            }
        }
        
        /// <summary>
        /// Appelé quand le missile doit être explosé
        /// </summary>
        public void ActiverExplosion()
        {
            Visible = false;
            Explosion = new SpriteAnimé(Game, CHAÎNE_IMAGE_EXPLOSION, Position, ZoneExplosion, DescriptionImageExplosion, INTERVALLE_ANIMATION_LENT);
            ExplosionActivée = true;
            PhaseExplosion = AVANT_PREMIÈRE_PHASE_EXPLOSION;
            Collision = true;
            NombreDePhasesExplosion = (int)(DescriptionImageExplosion.X * DescriptionImageExplosion.Y);
        }

        /// <summary>
        /// Gère l'explosion du missile
        /// </summary>
        /// <param name="gameTime">Contient les informations de temps de jeu</param>
        void GérerExplosion()
        {
            ++PhaseExplosion;
            if (PhaseExplosion >= NombreDePhasesExplosion)
            {
                ExplosionActivée = false;
                Explosion.ADétruire = true;
                ADétruire = true;
            }
        }
    }
}