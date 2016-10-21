/*
SpriteAnimé.cs
--------------

Par Mathieu Godin et Raphaël Brûlé

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
        protected const int AUCUN_TEMPS_ÉCOULÉ = 0, AUCUN_DÉPLACEMENT = 0;

        //Propriétés initialement gérées par le constructeur
        Vector2 DescriptionImage { get; set; }
        protected float IntervalleMAJAnnimation { get; set; }

        //Propriétés initialement gérées par Initialize
        public bool ADétruire { get; set; }
        float TempsÉcouléDepuisMAJAnimation { get; set; }
        //int Rangé { get; set; }
        //int VariableÀChangerDeNom { get; set; }
        protected Vector2 Delta { get; set; }
        int ÉtalementAnimationsAbscisses { get; set; }
        int ÉtalementAnimationsOrdonnées { get; set; }

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
            Delta = CalculerDimensionsSpriteOriginal();
            ADétruire = false;
            TempsÉcouléDepuisMAJAnimation = AUCUN_TEMPS_ÉCOULÉ;
            base.Initialize();
            ÉtalementAnimationsAbscisses = (int)DimensionsImage.X - (int)Delta.X;
            ÉtalementAnimationsOrdonnées = (int)DimensionsImage.Y - (int)Delta.Y;
        }

        /// <summary>
        /// Calcule les dimensions du sprite tel qu'on le voit dans son fichier
        /// </summary>
        /// <returns>Retourne le vecteur de type Vector2 de ses dimensions</returns>
        protected override Vector2 CalculerDimensionsSpriteOriginal()
        {
            return base.CalculerDimensionsSpriteOriginal() / DescriptionImage;
        }

        /// <summary>
        /// Calcule rectangle couvrant ce qui sera affiché
        /// </summary>
        /// <returns>Le rectangle source de type Rectangle</returns>
        protected override Rectangle CalculerRectangleSource()
        {
            return new Rectangle(ABSCISSE_NULLE, ORDONNÉE_NULLE, (int)Delta.X, (int)Delta.Y);
        }

        /// <summary>
        /// Calcule l'échelle horizontale du sprite pour la méthode Draw()
        /// </summary>
        protected override float CalculerÉchelleHorizontale()
        {
            return ZoneAffichage.Width / Delta.X;
        }

        /// <summary>
        /// Calcule l'échelle verticale du sprite pour la méthode Draw()
        /// </summary>
        protected override float CalculerÉchelleVerticale()
        {
            return ZoneAffichage.Height / Delta.Y;
        }

        /// <summary>
        /// Méthode qui met à jour le SpriteAnimé selon le temps écoulé
        /// </summary>
        protected virtual void EffectuerMiseÀJourAnimation()
        {
            RectangleSource = new Rectangle((RectangleSource.X + (int)Delta.X) % (int)DimensionsImage.X, RectangleSource.X >= ÉtalementAnimationsAbscisses ? (RectangleSource.Y >= ÉtalementAnimationsOrdonnées ? ORDONNÉE_NULLE : RectangleSource.Y + (int)Delta.Y) : RectangleSource.Y, (int)Delta.X, (int)Delta.Y);
        }

        /// <summary>
        /// Met à jour le sprite animé
        /// </summary>
        /// <param name="gameTime">Contient les informations sur le temps de jeu</param>
        public override void Update(GameTime gameTime)
        {
            TempsÉcouléDepuisMAJAnimation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléDepuisMAJAnimation >= IntervalleMAJAnnimation)
            {
                EffectuerMiseÀJourAnimation();
                TempsÉcouléDepuisMAJAnimation = AUCUN_TEMPS_ÉCOULÉ;
            }
        }

        /// <summary>
        /// Prédicat vrai si le Sprite est en collision avec un autre objet
        /// </summary>
        /// <param name="autreObjet">L'autre objet qui pourrait être en collision</param>
        /// <returns></returns>
        public override bool EstEnCollision(object autreObjet)
        {
            Rectangle autreRectangle = ((SpriteAnimé)autreObjet).RectangleDimensionsImageÀLÉchelle;

            return RectangleDimensionsImageÀLÉchelle.Intersects(autreRectangle);
        }
    }
}