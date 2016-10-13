/* Auteur :            Raphaël Brulé
   Fichier :           VaisseauSpatial.cs
   Date :              le 05 octobre 2016
   Description :       Ce component, enfant de SpriteAnimé, permet
                       de gérer le vaisseau spatial.*/

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

//CLASSE TRÈS IMCOMPLÈTE!!

namespace AtelierXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class VaisseauSpatial : SpriteAnimé
    {
        //Constante
        const int NE_SE_DÉPLACE_PAS = 0;
        const int SE_DÉPLACE = 1;
        const int NB_PIXELS_DE_DÉPLACEMENT = 1;

        //Propriété initialement gérée par le constructeur
        float IntervalleMAJDéplacement { get; set; }

        //Propriété initialement gérée par LoadContent
        bool SeDéplace { get; set; }
        InputManager GestionInput { get; set; }

        int VariableÀChangerDeNom { get; set; }

        /// <summary>
        /// Constructeur de VaisseauSpatial
        /// </summary>
        /// <param name="jeu">Objet de classe Game</param>
        /// <param name="nomImage">Nom de l'image (string)</param>
        /// <param name="position">Position (Vector2)</param>
        /// <param name="zoneAffichage">Zone d'affichage (Rectangle)</param>
        /// <param name="descriptionImage">Description de l'image (Vector2)</param>
        /// <param name="intervalleMAJAnimation">Intervalle de mise à jour de l'animation (float)</param>
        /// <param name="intervalleMAJDéplacement">Intervalle de mise à jour du déplacement (float)</param>
        public VaisseauSpatial(Game jeu, string nomImage,
                               Vector2 position, Rectangle zoneAffichage,
                               Vector2 descriptionImage, float intervalleMAJAnimation,
                               float intervalleMAJDéplacement)
            : base(jeu, nomImage, position, zoneAffichage,
                  descriptionImage, intervalleMAJAnimation)
        {
            IntervalleMAJDéplacement = intervalleMAJDéplacement;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;

        }

        protected override void EffectuerMiseÀJour()
        {
            GérerClavier();

            RectangleSource = new Rectangle((RectangleSource.X + (int)Delta.X) % Image.Width,
                                            (int)Delta.Y * (SeDéplace ? SE_DÉPLACE : NE_SE_DÉPLACE_PAS),
                                            (int)Delta.X, (int)Delta.Y);
        }

        void GérerClavier()
        {
            if (GestionInput.EstClavierActivé)
            {
                int déplacementHorizontal = GérerTouche(Keys.D) - GérerTouche(Keys.A);
                int déplacementVertical = GérerTouche(Keys.S) - GérerTouche(Keys.W);
                if (déplacementHorizontal != 0 || déplacementVertical != 0)
                {
                    SeDéplace = true;
                    AjusterPosition(déplacementHorizontal, déplacementVertical);
                }
                else
                {
                    SeDéplace = false;
                }
            }
        }

        int GérerTouche(Keys touche)
        {
            return GestionInput.EstEnfoncée(touche) ? NB_PIXELS_DE_DÉPLACEMENT : 0;
        }

        void AjusterPosition(int déplacementHorizontal, int déplacementVertical)
        {
            float posX = CalculerPosition(déplacementHorizontal, Position.X, MargeGauche, MargeDroite);
            float posY = CalculerPosition(déplacementVertical, Position.Y, MargeHaut, MargeBas);
            Position = new Vector2(posX, posY);
        }

        float CalculerPosition(int déplacement, float posActuelle, int BorneMin, int BorneMax)
        {
            float position = posActuelle + déplacement;
            return MathHelper.Min(MathHelper.Max(BorneMin, position), BorneMax);
        }

    }
}
