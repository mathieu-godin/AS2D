/* Auteur :            Rapha�l Brul�
   Fichier :           VaisseauSpatial.cs
   Date :              le 05 octobre 2016
   Description :       Ce component, enfant de SpriteAnim�, permet
                       de g�rer le vaisseau spatial.*/

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

//CLASSE TR�S IMCOMPL�TE!!

namespace AtelierXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class VaisseauSpatial : SpriteAnim�
    {
        //Constante
        const int SE_D�PLACE = 1;

        //Propri�t� initialement g�r�e par le constructeur
        float IntervalleMAJD�placement { get; set; }

        int DeltaX { get; set; }
        int DeltaY { get; set; }
        InputManager GestionInput { get; set; }

        /// <summary>
        /// Constructeur de VaisseauSpatial
        /// </summary>
        /// <param name="jeu">Objet de classe Game</param>
        /// <param name="nomImage">Nom de l'image (string)</param>
        /// <param name="position">Position (Vector2)</param>
        /// <param name="zoneAffichage">Zone d'affichage (Rectangle)</param>
        /// <param name="descriptionImage">Description de l'image (Vector2)</param>
        /// <param name="intervalleMAJAnimation">Intervalle de mise � jour de l'animation (float)</param>
        /// <param name="intervalleMAJD�placement">Intervalle de mise � jour du d�placement (float)</param>
        public VaisseauSpatial(Game jeu, string nomImage,
                               Vector2 position, Rectangle zoneAffichage,
                               Vector2 descriptionImage, float intervalleMAJAnimation,
                               float intervalleMAJD�placement)
            : base(jeu, nomImage, position, zoneAffichage,
                  descriptionImage, intervalleMAJAnimation)
        {
            IntervalleMAJD�placement = intervalleMAJD�placement;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;

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
