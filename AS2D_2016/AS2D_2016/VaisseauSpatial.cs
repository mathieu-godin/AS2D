/* Auteur :            Raphaël Brulé
   Fichier :           VaisseauSpatial.cs
   Date :              le 05 octobre 2016
   Description :       Ce component, enfant de SpriteAnimé, permet
                       de gérer le vaisseau spatial.*/

// Modification : Modifications pour la descente du vaisceau au début
//                Mathieu Godin
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
        const int NB_PIXELS_DE_DÉPLACEMENT = 4; // Je l'ai changé de 5 à 4 car ça ressemblait plus à l'exemple d'exécution

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
        // Ajouté par Mathieu Godin pour la descente du vaisseau
        int OrdonnéeFinaleVaisseau { get; set; }
        bool EnDescente { get; set; }
        Vector2 VecteurDéplacementDescente { get; set; } // D'autres similaires pourraient être utilisés dans le reste de la classe pour optimiser


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

            //À effacer avec la descente du vaisseau maintenant : Position = new Vector2(Position.X - DestinationRectangle.Width/2, Game.Window.ClientBounds.Height - DestinationRectangle.Height); 
            Position = new Vector2(Position.X - DestinationRectangle.Width / DIVISEUR_OBTENTION_DEMI_GRANDEUR, Position.Y - DestinationRectangle.Height / DIVISEUR_OBTENTION_DEMI_GRANDEUR); // Nouvelle ligne
            TempsÉcouléDepuisMAJ = 0;
            AnimationSelonLeDéplacement = 0;
            AnciennePosition = new Vector2(Position.X, Position.Y);
            OrdonnéeFinaleVaisseau = Game.Window.ClientBounds.Height - DestinationRectangle.Height; // Nouvelle ligne
            EnDescente = true; // Nouvelle ligne
            VecteurDéplacementDescente = new Vector2(AUCUN_DÉPLACEMENT, NB_PIXELS_DE_DÉPLACEMENT);
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
            //Ajout pr missile
            if (GestionInput.EstNouvelleTouche(Keys.Space))
                LancerMissile();

            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJDéplacement)
            {
                DéterminerSiVaisseauEnDescente(); // Nouvelle méthode
                EffectuerMiseÀJourDéplacement();
                TempsÉcouléDepuisMAJ = AUCUN_TEMPS_ÉCOULÉ;
            }
        }

        /// <summary>
        /// Détermine si on est au début de la partie avec le vaisseau en descente pour aller le descendre si c'est le cas
        /// </summary>
        void DéterminerSiVaisseauEnDescente()
        {
            if (EnDescente)
            {
                GérerDescenteDuVaisseau(); // Nouvelle méthode
            }
        }

        /// <summary>
        /// Gère la descente du vaisseau au début de la partie
        /// </summary>
        /// <param name="gameTime">Contient les informations de temps de jeu</param>
        void GérerDescenteDuVaisseau()
        {
                Position += VecteurDéplacementDescente;
                if (Position.Y >= OrdonnéeFinaleVaisseau)
                {
                    Position = new Vector2(Position.X, OrdonnéeFinaleVaisseau);
                    EnDescente = false;
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

        void LancerMissile()
        {
            int nbreDeMissiles = (Game.Components.Where(composant => composant is Missile && !((Missile)composant).ADétruire && ((Missile)composant).Visible).Count());

            if(nbreDeMissiles < 3)
            {
                Missile missile = new Missile(Game,
                                                "Missile",
                                                new Vector2(DestinationRectangle.X + DestinationRectangle.Width/2 - 4, DestinationRectangle.Y - DestinationRectangle.Height/4),
                                                new Rectangle(0, 0, 30, 40),
                                                new Vector2(25, 1),
                                                "Explosion",
                                                new Vector2(5, 4),
                                                1.5f * Atelier.INTERVALLE_STANDARDS,
                                                Atelier.INTERVALLE_STANDARDS);
                Game.Components.Add(missile);
            }
        }

    }
}
