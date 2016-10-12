/*
Sprite.cs
---------

Par Mathieu Godin

Rôle : Composant qui est un DrawableGameComponent et
       hérite de ICollisionnable, permet d'afficher
       un sprite à l'écran par le biais d'un Texture2D

Créé : 5 octobre 2016
Modifié : 12 octobre 2016
Description : Affiche maintenant à l'échelle et EstEnCollision a été implanté
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
    /// <summary>
    /// Classe sprite qui englobe la plupart des composants de ce projet
    /// </summary>
    public class Sprite : Microsoft.Xna.Framework.DrawableGameComponent, ICollisionable
    {
        const int DIVISEUR_OBTENTION_DEMI_GRANDEUR = 2;
        const float AUCUNE_COUCHE_DE_PROFONDEUR = 0.0F;
        const float AUCUNE_ROTATION = 0.0F;
        const float ORDONNÉE_NULLE = 0.0F;
        const float ABSCISSE_NULLE = 0.0F;
        const float HAUTEUR_NULLE = 0.0F;
        const float LARGEUR_NULLE = 0.0F;

        string NomImage { get; set; }
        protected Vector2 Position { get; set; }
        Rectangle ZoneAffichage { get; set; }
        SpriteBatch GestionSprites { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        protected Texture2D Image { get; set; }
        float Échelle { get; set; }
        Vector2 Origine { get; set; }
        Rectangle RectangleDimensionsImageÀLÉchelle { get; set; }

        /// <summary>
        /// Constructeur de la classe Sprite
        /// </summary>
        /// <param name="jeu">Jeu de type Game</param>
        /// <param name="nomImage">Nom de l'image tel qu'écrit dans son répertoire</param>
        /// <param name="position">Position à laquelle on veut placer le sprite</param>
        /// <param name="zoneAffichage">Zone d'affichage dans laquelle on met le sprite</param>
        public Sprite(Game jeu, string nomImage, Vector2 position, Rectangle zoneAffichage) : base(jeu)
        {
            NomImage = nomImage;
            Position = position;
            ZoneAffichage = zoneAffichage;
        }

        /// <summary>
        /// Initialise ce qu'il faut pour le sprite
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Échelle = CalculerÉchelle();
            Origine = new Vector2(ABSCISSE_NULLE, ORDONNÉE_NULLE);
            RectangleDimensionsImageÀLÉchelle = new Rectangle((int)ABSCISSE_NULLE, (int)ORDONNÉE_NULLE, (int)(Image.Width * Échelle), (int)(Image.Height * Échelle));
        }

        /// <summary>
        /// Calcule l'échelle en calculant l'échelle horizontale et verticale et prenant la plus petite des deux
        /// </summary>
        /// <returns>La plus petite des échelles horizontales et verticales</returns>
        float CalculerÉchelle()
        {
            float échelleHorizontale = ZoneAffichage.Width / Image.Width, échelleVerticale = ZoneAffichage.Height / Image.Height;

            return échelleHorizontale < échelleVerticale ? échelleHorizontale : échelleVerticale;
        }

        /// <summary>
        /// Charge le contenu nécessaire au fonctionnement du Sprite
        /// </summary>
        protected override void LoadContent()
        {
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Image = GestionnaireDeTextures.Find(NomImage);
        }

        /// <summary>
        /// Méthode qui dessine le Sprite à l'écran
        /// </summary>
        /// <param name="gameTime">Contient les informations sur le temps de jeu</param>
        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Draw(Image, Position, ZoneAffichage, Color.White, AUCUNE_ROTATION, Origine, Échelle, SpriteEffects.None, AUCUNE_COUCHE_DE_PROFONDEUR);
        }

        /// <summary>
        /// Prédicat vrai si le Sprite est en collision avec un autre objet
        /// </summary>
        /// <param name="autreObjet"></param>
        /// <returns></returns>
        public bool EstEnCollision(object autreObjet)
        {
            Sprite autreSprite = (Sprite)autreObjet;
            Rectangle rectangleCollision = Rectangle.Intersect(RectangleDimensionsImageÀLÉchelle, autreSprite.RectangleDimensionsImageÀLÉchelle);

            return rectangleCollision.Width == LARGEUR_NULLE && rectangleCollision.Height == HAUTEUR_NULLE;
        }
    }
}
