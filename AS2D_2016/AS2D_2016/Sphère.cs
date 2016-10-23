/*
Sphère.cs
---------

Par Mathieu Godin

Rôle : Composant qui hérite de SpriteAnimé et qui
       permet d'afficher un spère reboudissant sur les
       murs de l'écran

Créé : 12 octobre 2016
Co-auteur : Raphaël Brûlé
*/
using System;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
    /// <summary>
    /// Sphère rebondissante sur les murs de l'écran
    /// </summary>
    public class Sphère : SpriteAnimé
    {
        const int ANGLE_DÉPLACEMENT_DÉPART_MINIMAL = 15, ANGLE_DÉPLACEMENT_DÉPART_MAXIMAL = 75, ANGLE_DROIT = 90, FACTEUR_MINIMAL_CERCLE_360_DEGRÉS = 0, FACTEUR_MAXIMAL_CERCLE_360_DEGRÉS_EXCLU = 4, ANGLE_PLAT = 180, ANGLE_PLEIN = 360;
        const float NORME_VECTEUR_DÉPLACEMENT = 3.5F;

        float IntervalleMAJDéplacement { get; set; }
        Random GénérateurAléatoire { get; set; }
        float TempsÉcouléDepuisMAJDéplacement { get; set; }
        float AngleDéplacement { get; set; }
        Vector2 VecteurDéplacementMAJ { get; set; }

        /// <summary>
        /// Constructeur de la classe Sphère
        /// </summary>
        /// <param name="game">Jeu de type Game</param>
        /// <param name="nomImage">Nom de la sphère tel qu'inscrit dans son dossier respectif</param>
        /// <param name="position">Position de départ de la sphère</param>
        /// <param name="zoneAffichage">Zone d'affichage de la sphère</param>
        /// <param name="descriptionImage">Le nombres de sprites de sphère en x et en y contenus dans l'image chargée</param>
        /// <param name="intervalleMAJAnimation">Intervalle de mise à jour de l'animation de la sphère</param>
        public Sphère(Game jeu, string nomImage, Vector2 position, Rectangle zoneAffichage, Vector2 descriptionImage, float intervalleMAJAnimation, float intervalleMAJDéplacement) : base(jeu, nomImage, position, zoneAffichage, descriptionImage, intervalleMAJAnimation)
        {
            IntervalleMAJDéplacement = intervalleMAJDéplacement;
        }

        /// <summary>
        /// Initialise les composants de la sphère
        /// </summary>
        public override void Initialize()
        {
            LoadContent();
            base.Initialize();
            InitialisationComposantsHazardeux();
            VecteurDéplacementMAJ = new Vector2(NORME_VECTEUR_DÉPLACEMENT * (float)Math.Cos(MathHelper.ToRadians(AngleDéplacement)), NORME_VECTEUR_DÉPLACEMENT * (float)Math.Sin(MathHelper.ToRadians(AngleDéplacement)));
            TempsÉcouléDepuisMAJDéplacement = AUCUN_TEMPS_ÉCOULÉ;
        }

        void InitialisationComposantsHazardeux()
        {
            Position = new Vector2(GénérateurAléatoire.Next(ABSCISSE_NULLE, MargeDroite), GénérateurAléatoire.Next(ORDONNÉE_NULLE, MargeBas / DIVISEUR_OBTENTION_DEMI_GRANDEUR));
            CalculerRectangleImageÀAfficher();
            AngleDéplacement = GénérateurAléatoire.Next(FACTEUR_MINIMAL_CERCLE_360_DEGRÉS, FACTEUR_MAXIMAL_CERCLE_360_DEGRÉS_EXCLU) * ANGLE_DROIT + GénérateurAléatoire.Next(ANGLE_DÉPLACEMENT_DÉPART_MINIMAL, ANGLE_DÉPLACEMENT_DÉPART_MAXIMAL);
        }

        /// <summary>
        /// Charge le contenu nécessaire au fonctionnement de la sphère
        /// </summary>
        protected override void LoadContent()
        {
            GénérateurAléatoire = Game.Services.GetService(typeof(Random)) as Random;
            base.LoadContent();
        }

        /// <summary>
        /// Met à jour la sphère
        /// </summary>
        /// <param name="gameTime">Contient les informations sur le temps de jeu</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            TempsÉcouléDepuisMAJDéplacement += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléDepuisMAJDéplacement >= IntervalleMAJDéplacement)
            {
                TempsÉcouléDepuisMAJDéplacement = AUCUN_TEMPS_ÉCOULÉ;
                EffectuerMiseÀJourDéplacement();
            }
        }

        /// <summary>
        /// Méthode qui met à jour le déplacement de la sphère selon le temps écoulé
        /// </summary>
        void EffectuerMiseÀJourDéplacement()
        {
            Position += VecteurDéplacementMAJ;
            CalculerRectangleImageÀAfficher();
            VérifierRebondissementMurs();
            VérifierRebondissementPlafondPlancher();
        }

        /// <summary>
        /// Vérifie si la balle rebondit sur les murs sur les côtés de la fenêtre
        /// </summary>
        void VérifierRebondissementMurs()
        {
            if (Position.X <= MargeGauche || Position.X >= MargeDroite)
            {
                AngleDéplacement = ANGLE_PLAT - AngleDéplacement;
                VecteurDéplacementMAJ = new Vector2(NORME_VECTEUR_DÉPLACEMENT * (float)Math.Cos(MathHelper.ToRadians(AngleDéplacement)), NORME_VECTEUR_DÉPLACEMENT * (float)Math.Sin(MathHelper.ToRadians(AngleDéplacement)));
            }
        }

        /// <summary>
        /// Vérifie si la balle rebondit sur le plancher ou le plafond de la fenêtre
        /// </summary>
        void VérifierRebondissementPlafondPlancher()
        {
            if (Position.Y >= MargeBas || Position.Y <= MargeHaut)
            {
                AngleDéplacement = ANGLE_PLEIN - AngleDéplacement;
                VecteurDéplacementMAJ = new Vector2(NORME_VECTEUR_DÉPLACEMENT * (float)Math.Cos(MathHelper.ToRadians(AngleDéplacement)), NORME_VECTEUR_DÉPLACEMENT * (float)Math.Sin(MathHelper.ToRadians(AngleDéplacement)));
            }
        }
    }
}