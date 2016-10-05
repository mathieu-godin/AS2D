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

//CLASSE TR�S IMCOMPL�TES!!

namespace AS2D_2016
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class VaisseauSpatial : SpriteAnim�
    {
        float IntervalleMAJD�placement { get; set; }

        public VaisseauSpatial(Game jeu, string nomImage,
                               Vector2 position, Rectangle zoneAffichage,
                               Vector2 descriptionImage, float intervalleMAJAnimation,
                               float intervalleMAJD�placement)
            : base(jeu, nomImage, position, zoneAffichage,
                  descriptionImage, intervalleMAJAnimation)
        {
            IntervalleMAJD�placement = intervalleMAJD�placement;
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
