/*
Sphère.cs
---------

Par Mathieu Godin

Rôle : Composant qui hérite de SpriteAnimé et qui
       permet d'afficher un spère reboudissant sur les
       murs de l'écran

Créé : 12 octobre 2016
*/
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
    /// <summary>
    /// Sphère rebondissante sur les murs de l'écran
    /// </summary>
    public class Sphère : SpriteAnimé
    {
        const int ANGLE_DÉPLACEMENT_DÉPART_MINIMAL = 15, ANGLE_DÉPLACEMENT_DÉPART_MAXIMAL = 75;

        float IntervalleMAJDéplacement { get; set; }
        Random GénérateurAléatoire { get; set; }
        float TempsÉcouléDepuisMAJDéplacement { get; set; }
        float AngleDéplacement { get; set; }

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
            /* Peut-être +1 au générateur car exclu*/
            Position = new Vector2(GénérateurAléatoire.Next(ABSCISSE_NULLE, MargeDroite), GénérateurAléatoire.Next(ORDONNÉE_NULLE, MargeBas / DIVISEUR_OBTENTION_DEMI_GRANDEUR));
            AngleDéplacement = GénérateurAléatoire.Next(ANGLE_DÉPLACEMENT_DÉPART_MINIMAL, ANGLE_DÉPLACEMENT_DÉPART_MAXIMAL);
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
                EffectuerMiseÀJourDéplacement();
                TempsÉcouléDepuisMAJDéplacement = AUCUN_TEMPS_ÉCOULÉ;
            }
        }

        /// <summary>
        /// Méthode qui met à jour le déplacement de la sphère selon le temps écoulé
        /// </summary>
        protected virtual void EffectuerMiseÀJourDéplacement()
        {
            Position += Vector2.UnitY;
        }
    }
}