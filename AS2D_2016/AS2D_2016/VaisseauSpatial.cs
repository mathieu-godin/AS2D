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
        const int NB_PIXELS_DE_DÉPLACEMENT = 5;

        //Propriété initialement gérée par le constructeur
        float IntervalleMAJDéplacement { get; set; }

        //Propriété initialement gérée par Initialize
        float TempsÉcouléDepuisMAJ { get; set; }
        int AnimationSelonLeDéplacement { get; set; }
        Vector2 AnciennePosition { get; set; }

        //Propriété initialement gérée par LoadContent
        InputManager GestionInput { get; set; }

        //à voir
        Vector2 DéplacementRésultant { get; set; }


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

        public override void Initialize()
        {
            base.Initialize();

            TempsÉcouléDepuisMAJ = 0;
            AnimationSelonLeDéplacement = 0;
            AnciennePosition = new Vector2(Position.X, Position.Y);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        protected override void EffectuerMiseÀJourAnimation()
        {
            RectangleSource = new Rectangle((RectangleSource.X + (int)Delta.X) % Image.Width,
                             (int)Delta.Y *AnimationSelonLeDéplacement,
                             (int)Delta.X, (int)Delta.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJDéplacement)
            {
                EffectuerMiseÀJourDéplacement();
                TempsÉcouléDepuisMAJ = 0;
            }
        }

        void EffectuerMiseÀJourDéplacement()
        {
            AnciennePosition = new Vector2(Position.X, Position.Y);
            
            GérerClavier();

            DéplacementRésultant = Position - AnciennePosition;

            AnimationSelonLeDéplacement = (SeDéplace()? SE_DÉPLACE : NE_SE_DÉPLACE_PAS);
        }

        void GérerClavier()
        {
            if (GestionInput.EstClavierActivé)
            {
                int déplacementHorizontal = GérerTouche(Keys.D) - GérerTouche(Keys.A);
                AjusterPosition(déplacementHorizontal);
            }
        }

        int GérerTouche(Keys touche)
        {
            return GestionInput.EstEnfoncée(touche) ? NB_PIXELS_DE_DÉPLACEMENT : 0;
        }

        void AjusterPosition(int déplacementHorizontal)
        {
            float posX = CalculerPosition(déplacementHorizontal, Position.X, MargeGauche, MargeDroite);

            Position = new Vector2(posX, Position.Y);
        }

        float CalculerPosition(int déplacement, float posActuelle, int BorneMin, int BorneMax)
        {
            float position = posActuelle + déplacement;
            return MathHelper.Min(MathHelper.Max(BorneMin, position), BorneMax);
        }

        bool SeDéplace()
        {
            return DéplacementRésultant != Vector2.Zero;
        }

        //public override void Draw(GameTime gameTime)
        //{
        //    //GestionSprites.Draw(Image, Position, RectangleSource, Color.White);

        //    GestionSprites.Draw(Image, new Rectangle((int)Position.X, (int)Position.Y, (int)(Delta.X*Échelle), (int)(Delta.Y*Échelle)),
        //                        RectangleSource, Color.White);
        //}
    }
}
