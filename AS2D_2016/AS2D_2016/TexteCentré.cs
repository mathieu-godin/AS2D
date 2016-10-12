/*
TexteCentré.cs
--------------

Par Mathieu Godin

Rôle : Composant qui affiche un texte centré
       à l'écran à la fin de la partie

Créé : 5 octobre 2016
Modifié : 11 octobre 2016
Description : Presque tout le code rendant le tout
              fonctionnel a été écrit
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
    /// Composant qui permet d'afficher un texte grossissant au centre de l'écran
    /// </summary>
    public class TexteCentré : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const int DIVISEUR_OBTENTION_DEMI_GRANDEUR = 2;
        const float AUCUNE_COUCHE_DE_PROFONDEUR = 0.0F;
        const float AUCUNE_ROTATION = 0.0F;

        string TexteÀAfficher { get; set; }
        string NomFont { get; set; }
        Rectangle ZoneAffichage { get; set; }
        Rectangle ZoneAffichageMarginée { get; set; }
        Color CouleurTexte { get; set; }
        float Marge { get; set; }
        SpriteFont Font { get; set; }
        Vector2 Origine { get; set; }
        SpriteBatch GestionSprites { get; set; }
        Vector2 PositionCentrée { get; set; }
        float Échelle { get; set; }
        Vector2 DimensionsTexte { get; set; }

        /// <summary>
        /// Constructeur qui permet de sauvegarder les personnalisations passées en paramètres
        /// </summary>
        /// <param name="game">Jeu qui a appelé ce composant</param>
        /// <param name="message">Chaîne rerpésentant le message que l'on veut afficher</param>
        /// <param name="position">Position du message dans l'écran par rapport à son centre</param>
        /// <param name="couleur">Couleur voulue pour le message tournoyant en question</param>
        /// <param name="intervalleMAJ">Intervalle de mise à jour de l'angle et de l'échelle du message, une grande valeur fera tourner et agrandir le message plus rapidement</param>
        public TexteCentré(Game game, string texteÀAfficher, string nomFont, Rectangle zoneAffichage, Color couleurTexte, float marge) : base(game)
        {
            TexteÀAfficher = texteÀAfficher;
            NomFont = nomFont;
            ZoneAffichage = zoneAffichage;
            CouleurTexte = couleurTexte;
            Marge = marge;
        }

        /// <summary>
        /// Initialise les mécaniques de temps, d'angle et d'échelle du message centré
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            ZoneAffichageMarginée = new Rectangle(ZoneAffichage.X, ZoneAffichage.Y, (int)(ZoneAffichage.Width - ZoneAffichage.Width * Marge), (int)(ZoneAffichage.Height - ZoneAffichage.Height * Marge));
            DimensionsTexte = Font.MeasureString(TexteÀAfficher);
            Échelle = CalculerÉchelle();
            InitialiserOrigine();
            PositionCentrée = new Vector2(Game.Window.ClientBounds.Width / DIVISEUR_OBTENTION_DEMI_GRANDEUR, Game.Window.ClientBounds.Height / DIVISEUR_OBTENTION_DEMI_GRANDEUR);
            this.Enabled = false;
        }

        /// <summary>
        /// Charge la police et la gestion des sprites et fait le calcul de l'origine du message centré
        /// </summary>
        protected override void LoadContent()
        {
            Font = Game.Content.Load<SpriteFont>("Fonts/" + NomFont);
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
        }

        /// <summary>
        /// Initialise l'origine
        /// </summary>
        void InitialiserOrigine()
        {
            Origine = new Vector2(DimensionsTexte.X / DIVISEUR_OBTENTION_DEMI_GRANDEUR, DimensionsTexte.Y / DIVISEUR_OBTENTION_DEMI_GRANDEUR);
        }

        /// <summary>
        /// Calcule l'échelle en calculant l'échelle horizontale et verticale et prenant la plus petite des deux
        /// </summary>
        /// <returns>La plus petite des échelles horizontales et verticales</returns>
        float CalculerÉchelle()
        {
            float échelleHorizontale = ZoneAffichageMarginée.Width / DimensionsTexte.X, échelleVerticale = ZoneAffichageMarginée.Height / DimensionsTexte.Y;

            return échelleHorizontale < échelleVerticale ? échelleHorizontale : échelleVerticale;
        }

        /// <summary>
        /// Dessine le message centré à l'écran
        /// </summary>
        /// <param name="gameTime">Donne des informations sur le temps du jeu</param>
        public override void Draw(GameTime gameTime)
        {
            GestionSprites.DrawString(Font, TexteÀAfficher, PositionCentrée, CouleurTexte, AUCUNE_ROTATION, Origine, Échelle, SpriteEffects.None, AUCUNE_COUCHE_DE_PROFONDEUR);
        }
    }
}
