﻿/*
SpriteAnimé.cs
--------------

Par Mathieu Godin

Rôle : Composant qui hérite de Sprite et qui
       permet d'animer le sprite qui sera
       affiché à l'écran en défilant différent
       dans la même image chargée

Créé : 5 octobre 2016
*/
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
    /// <summary>
    /// Composant qui peut afficher un sprite animé par un défilement de sprites présents sur la même image chargée
    /// </summary>
    public class SpriteAnimé : Sprite, IDestructible
    {
        //Constantes
        const float AUCUN_DÉPLACEMENT = 0.0F;
        const int ORIGINE = 0;

        //Propriétés initialement gérées par le constructeur
        Vector2 DescriptionImage { get; set; }
        float IntervalleMAJAnnimation { get; set; }

        //Propriétés initialement gérées par LoadContent
        Rectangle RectangleSource { get; set; }
        public bool ADétruire { get; set; }

        //Propriétés initialement gérées par CalculerMarges
        protected Vector2 Delta { get; set; }
        protected int MargeDroite { get; set; }
        protected int MargeBas { get; set; }
        protected int MargeGauche { get; set; }
        protected int MargeHaut { get; set; }

        /// <summary>
        /// Constructeur de la classe SpriteAnimé
        /// </summary>
        /// <param name="game">Jeu dde type Game</param>
        /// <param name="nomImage">Nom du sprite tel qu'inscrit dans son dossier respectif</param>
        /// <param name="position">Position de départ du sprite</param>
        /// <param name="zoneAffichage">Zone d'affichage du sprite</param>
        /// <param name="descriptionImage">Le nombres de sprites en x et en y contenus dans l'image chargée</param>
        /// <param name="intervalleMAJAnimation">Intervalle de mise à jour de l'animation du sprite</param>
        public SpriteAnimé(Game game, string nomImage, Vector2 position, Rectangle zoneAffichage, Vector2 descriptionImage, float intervalleMAJAnimation) : base(game, nomImage, position, zoneAffichage)
        {
            DescriptionImage = new Vector2(descriptionImage.X, descriptionImage.Y);
            IntervalleMAJAnnimation = intervalleMAJAnimation;
        }

        /// <summary>
        /// Méthode de chargement de contenu nécessaire au SpriteAnimé
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            RectangleSource = new Rectangle(ORIGINE, ORIGINE, (int)Delta.X, (int)Delta.Y);
            ADétruire = false;
        }

        /// <summary>
        /// Calcule les marges du sprite
        /// </summary>
        protected void CalculerMarges()
        {
            Delta = new Vector2(Image.Width, Image.Height) / DescriptionImage;
            MargeDroite = Game.Window.ClientBounds.Width - (int)Delta.X;
            MargeBas = Game.Window.ClientBounds.Height - (int)Delta.Y;
            MargeGauche = 0; MargeHaut = 0;
        }

        /// <summary>
        /// Méthode qui met à jour le SpriteAnimé selon le temps écoulé
        /// </summary>
        protected virtual void EffectuerMiseÀJour()
        {
            ADétruire = EstEnCollision(this);
        }

        //protected virtual void SpriteAniméSurUneLigne()//#ligneàmathieu
        //{
        //    RectangleSource = new Rectangle((RectangleSource.X + (int)Delta.X) % Image.Width, RectangleSource.X > Image.Width - (int)Delta.X ? (RectangleSource.Y > Image.Height - (int)Delta.Y ? ORIGINE : RectangleSource.Y + (int)Delta.Y) : RectangleSource.Y, (int)Delta.X, (int)Delta.Y);
        //}

        /// <summary>
        /// Méthode qui dessine le SpriteAnimé à l'écran
        /// </summary>
        /// <param name="gameTime">Objet contenant l'information de temps de jeu de type GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Draw(Image, Position, RectangleSource, Color.White);
        }
    }
}