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
        const int NE_SE_D�PLACE_PAS = 0;
        const int SE_D�PLACE = 1;
        const int NB_PIXELS_DE_D�PLACEMENT = 1;

        //Propri�t� initialement g�r�e par le constructeur
        float IntervalleMAJD�placement { get; set; }

        //Propri�t� initialement g�r�e par LoadContent
        bool SeD�place { get; set; }
        InputManager GestionInput { get; set; }

        int Variable�ChangerDeNom { get; set; }

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

        protected override void EffectuerMise�Jour()
        {
            G�rerClavier();

            RectangleSource = new Rectangle((RectangleSource.X + (int)Delta.X) % Image.Width,
                                            (int)Delta.Y * (SeD�place ? SE_D�PLACE : NE_SE_D�PLACE_PAS),
                                            (int)Delta.X, (int)Delta.Y);
        }

        void G�rerClavier()
        {
            if (GestionInput.EstClavierActiv�)
            {
                int d�placementHorizontal = G�rerTouche(Keys.D) - G�rerTouche(Keys.A);
                int d�placementVertical = G�rerTouche(Keys.S) - G�rerTouche(Keys.W);
                if (d�placementHorizontal != 0 || d�placementVertical != 0)
                {
                    SeD�place = true;
                    AjusterPosition(d�placementHorizontal, d�placementVertical);
                }
                else
                {
                    SeD�place = false;
                }
            }
        }

        int G�rerTouche(Keys touche)
        {
            return GestionInput.EstEnfonc�e(touche) ? NB_PIXELS_DE_D�PLACEMENT : 0;
        }

        void AjusterPosition(int d�placementHorizontal, int d�placementVertical)
        {
            float posX = CalculerPosition(d�placementHorizontal, Position.X, MargeGauche, MargeDroite);
            float posY = CalculerPosition(d�placementVertical, Position.Y, MargeHaut, MargeBas);
            Position = new Vector2(posX, posY);
        }

        float CalculerPosition(int d�placement, float posActuelle, int BorneMin, int BorneMax)
        {
            float position = posActuelle + d�placement;
            return MathHelper.Min(MathHelper.Max(BorneMin, position), BorneMax);
        }

    }
}
