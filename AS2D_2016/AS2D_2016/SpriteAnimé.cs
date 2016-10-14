/*
SpriteAnimé.cs
--------------

Par Mathieu Godin

Rôle : Composant qui hérite de Sprite et qui 
       permet d'animer le sprite qui sera
       affiché à l'écran en défilant différent
       dans la même image chargée

Créé : 5 octobre 2016
Modifié : 12 octobre 2016
Description : Très grandes modifications pour EstEnCollision pour faire ÀDétruire et autres
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

        //Propriétés initialement gérées par Initialize
        protected Rectangle RectangleSource { get; set; }
        public bool ADétruire { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        //int Rangé { get; set; }
        //int VariableÀChangerDeNom { get; set; }
        protected Vector2 Delta { get; set; }

        /// <summary>
        /// Constructeur de la classe SpriteAnimé
        /// </summary>
        /// <param name="game">Jeu dde type Game</param>
        /// <param name="nomImage">Nom du sprite tel qu'inscrit dans son dossier respectif</param>
        /// <param name="position">Position de départ du sprite</param>
        /// <param name="zoneAffichage">Zone d'affichage du sprite</param>
        /// <param name="descriptionImage">Le nombres de sprites en x et en y contenus dans l'image chargée</param>
        /// <param name="intervalleMAJAnimation">Intervalle de mise à jour de l'animation du sprite</param>
        public SpriteAnimé(Game game, string nomImage, Vector2 position, Rectangle zoneAffichage, Vector2 descriptionImage, float intervalleMAJAnimation) 
            : base(game, nomImage, position, zoneAffichage)
        {
            DescriptionImage = new Vector2(descriptionImage.X, descriptionImage.Y);
            IntervalleMAJAnnimation = intervalleMAJAnimation;
        }

        /// <summary>
        /// Initialise les composants de SpriteAnimé
        /// </summary>
        public override void Initialize()
        {
            LoadContent();
            RectangleSource = new Rectangle(ORIGINE, ORIGINE, (int)Delta.X, (int)Delta.Y);
            Delta = new Vector2(Image.Width, Image.Height) / DescriptionImage;
            ADétruire = false;
            TempsÉcouléDepuisMAJ = 0;
            //Rangé = 0;

            base.Initialize();
        }

        /// <summary>
        /// Méthode qui met à jour le SpriteAnimé selon le temps écoulé
        /// </summary>
        protected virtual void EffectuerMiseÀJour()
        {
            //if(Rangé == DescriptionImage.Y)
            //    Rangé = 0;

            //VariableÀChangerDeNom = (RectangleSource.X + (int)Delta.X) % Image.Width;

            //RectangleSource = new Rectangle(VariableÀChangerDeNom,
            //                       (int)Delta.Y * Rangé, (int)Delta.X, (int)Delta.Y);

            //if(VariableÀChangerDeNom == DescriptionImage.X - 1)
            //    ++Rangé;
            RectangleSource = new Rectangle((RectangleSource.X + (int)Delta.X) % Image.Width, RectangleSource.X >= Image.Width - (int)Delta.X ? (RectangleSource.Y >= Image.Height - (int)Delta.Y ? ORIGINE : RectangleSource.Y + (int)Delta.Y) : RectangleSource.Y, (int)Delta.X, (int)Delta.Y);
        }

        public override void Update(GameTime gameTime)
        {
            //ADétruire = EstEnCollision(this); LIGNE PAS BONNE À CHANGER

            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJAnnimation)
            {
                EffectuerMiseÀJour();
                TempsÉcouléDepuisMAJ = 0;
            }
        }

        /// <summary>
        /// Méthode qui dessine le SpriteAnimé à l'écran
        /// </summary>
        /// <param name="gameTime">Objet contenant l'information de temps de jeu de type GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Draw(Image, Position, RectangleSource, Color.White);
        }

        /// <summary>
        /// Prédicat vrai si le Sprite est en collision avec un autre objet
        /// </summary>
        /// <param name="autreObjet"></param>
        /// <returns></returns>
        public override bool EstEnCollision(object autreObjet)
        {
            SpriteAnimé autreSprite = (SpriteAnimé)autreObjet;
            Rectangle rectangleCollision = Rectangle.Intersect(RectangleDimensionsImageÀLÉchelle, autreSprite.RectangleDimensionsImageÀLÉchelle);
            bool collision = rectangleCollision.Width == LARGEUR_NULLE && rectangleCollision.Height == HAUTEUR_NULLE;

            ADétruire = collision;
            autreSprite.ADétruire = collision;

            return collision;
        }
    }
}