/*
Missile.cs
----------

Par Mathieu Godin

Rôle : Composant qui hérite de SpriteAnimé
       qui gère le missile pouvant se déplacer
       vers le haut en accélérant et son explosion

Créé : 5 octobre 2016
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


namespace AS2D_2016
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Sphère //: SpriteAnimé
    {
        //Propriété initialement gérée par le constructeur
        float IntervalleMAJDéplacement { get; set; }
        string NomImageMissile { get; set; }
        Vector2 DescriptionImageMissile { get; set; }
        string NomImageExplosion { get; set; }
        Vector2 DescriptionImageExplosion { get; set; }

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
        public Sphère(Game jeu, string nomImageMissile, Vector2 position, Rectangle zoneAffichage, Vector2 descriptionImageMissile, string nomImageExplosion, Vector2 descriptionImageExplosion, float intervalleMAJAnimation, float intervalleMAJDéplacement) : base(jeu, nomImageMissile, position, zoneAffichage, descriptionImageMissile, intervalleMAJAnimation)
        {
            IntervalleMAJDéplacement = intervalleMAJDéplacement;
            NomImageMissile = nomImageMissile;
            DescriptionImageMissile = descriptionImageMissile;
            NomImageExplosion = nomImageExplosion;
            DescriptionImageExplosion = descriptionImageExplosion;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
    }
}
