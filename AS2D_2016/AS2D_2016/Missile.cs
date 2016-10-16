/*
Missile.cs
----------

Par Mathieu Godin

Rôle : Composant qui hérite de SpriteAnimé
       qui gère le missile pouvant se déplacer
       vers le haut en accélérant et son explosion

Créé : 5 octobre 2016
Modifié : 15 octobre 2016
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
    /// Composant qui hérite de SpriteAnimé
    /// </summary>
    public class Missile : SpriteAnimé
    {
        //Propriété initialement gérée par le constructeur
        float IntervalleMAJDéplacement { get; set; }
        string NomImageExplosion { get; set; }
        Vector2 DescriptionImageExplosion { get; set; }
        Texture2D ImageExplosion { get; set; }
        float TempsÉcouléDepuisMAJDéplacement { get; set; }

        public bool ExplosionActivée { get; set; }

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
        }

        /// <summary>
        /// Charge le contenu nécessaire au fonctionnement du Missile
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            ImageExplosion = GestionnaireDeTextures.Find(NomImageExplosion);
        }

        /// <summary>
        /// Met à jour le Missile
        /// </summary>
        /// <param name="gameTime">Contient les informations de temps</param>
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
        /// Méthode qui met à jour le déplacement du Missile selon le temps écoulé
        /// </summary>
        protected virtual void EffectuerMiseÀJourDéplacement()
        {
            Position -= Vector2.UnitY;
        }

        public void ActiverExplosion()
        {

        }
    }
}
