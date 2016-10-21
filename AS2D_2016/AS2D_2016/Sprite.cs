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
        protected const int ORDONNÉE_NULLE = 0, ABSCISSE_NULLE = 0, HAUTEUR_NULLE = 0, LARGEUR_NULLE = 0, DIVISEUR_OBTENTION_DEMI_GRANDEUR = 2;

        string NomImage { get; set; }
        public Vector2 Position { get; protected set; }
        protected Rectangle ZoneAffichage { get; set; }
        protected SpriteBatch GestionSprites { get; set; }
        protected RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        /* probably private */
        Texture2D Image { get; set; }
        float Échelle { get; set; }
        protected Rectangle RectangleDimensionsImageÀLÉchelle { get; set; }
        protected int MargeDroite { get; set; }
        protected int MargeBas { get; set; }
        protected int MargeGauche { get; set; }
        protected int MargeHaut { get; set; }
        Vector2 DimensionsSpriteÀAfficher { get; set; }
        protected Vector2 DimensionsImage { get; private set; }
        protected Rectangle RectangleSource { get; set; }

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
            DimensionsImage = CalculerDimensionsImage();
            Échelle = CalculerÉchelle();
            DimensionsSpriteÀAfficher = CalculerDimensionsSpriteÀAfficher();
            //Origine = new Vector2(ABSCISSE_NULLE, ORDONNÉE_NULLE);
            RectangleDimensionsImageÀLÉchelle = CalculerRectangleDimensionsImageÀLÉchelle();
            RectangleSource = CalculerRectangleSource();
            CalculerMarges();
        }

        /// <summary>
        /// Calcule rectangle couvrant ce qui sera affiché
        /// </summary>
        /// <returns>Le rectangle source de type Rectangle</returns>
        protected virtual Rectangle CalculerRectangleSource()
        {
            return new Rectangle(ABSCISSE_NULLE, ORDONNÉE_NULLE, Image.Width, Image.Height);
        }

        /// <summary>
        /// Calcule les dimensions de l'aimge
        /// </summary>
        /// <returns>Un vecteur de type Vector2 ayant les dimensions de l'image</returns>
        Vector2 CalculerDimensionsImage()
        {
            return new Vector2(Image.Width, Image.Height);
        }

        /// <summary>
        /// Calcule le rectangle représentant ce qui va être affiché
        /// </summary>
        /// <returns>Le rectangle de type Rectangle représentant le pourtour de ce qui sera affiché</returns>
        protected Rectangle CalculerRectangleDimensionsImageÀLÉchelle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)(DimensionsSpriteÀAfficher.X), (int)(DimensionsSpriteÀAfficher.Y));
        }

        /// <summary>
        /// Calcule les dimensions du sprite tel qu'il sera affiché
        /// </summary>
        /// <returns>Un vecteur de type Vector2 représentant les dimensions de ce qui sera affiché</returns>
        Vector2 CalculerDimensionsSpriteÀAfficher()
        {
            return new Vector2(Échelle, Échelle) * CalculerDimensionsSpriteOriginal();
        }

        /// <summary>
        /// Calcule les dimensions du sprite tel qu'on le voit dans son fichier
        /// </summary>
        /// <returns>Retourne le vecteur de type Vector2 de ses dimensions</returns>
        protected virtual Vector2 CalculerDimensionsSpriteOriginal()
        {
            return new Vector2(Image.Width, Image.Height);
        }

        /// <summary>
        /// Calcule l'échelle en calculant l'échelle horizontale et verticale et prenant la plus petite des deux
        /// </summary>
        /// <returns>La plus petite des échelles horizontales et verticales</returns>
        protected float CalculerÉchelle()
        {
            float échelleHorizontale = CalculerÉchelleHorizontale(), échelleVerticale = CalculerÉchelleVerticale();

            return échelleHorizontale < échelleVerticale ? échelleHorizontale : échelleVerticale;
        }

        /// <summary>
        /// Calcule l'échelle horizontale du sprite pour la méthode Draw()
        /// </summary>
        protected virtual float CalculerÉchelleHorizontale()
        {
            return ZoneAffichage.Width / (float)Image.Width;
        }

        /// <summary>
        /// Calcule l'échelle verticale du sprite pour la méthode Draw()
        /// </summary>
        protected virtual float CalculerÉchelleVerticale()
        {
            return ZoneAffichage.Height / (float)Image.Height;
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
            //GestionSprites.Draw(Image, Position, ZoneAffichage, Color.White, AUCUNE_ROTATION, Origine, Échelle, SpriteEffects.None, AUCUNE_COUCHE_DE_PROFONDEUR);

            GestionSprites.Draw(Image, RectangleDimensionsImageÀLÉchelle, RectangleSource, Color.White);
        }

        /// <summary>
        /// Prédicat vrai si le Sprite est en collision avec un autre objet
        /// </summary>
        /// <param name="autreObjet"></param>
        /// <returns></returns>
        public virtual bool EstEnCollision(object autreObjet)
        {
            //SpriteAnimé autreSprite = (SpriteAnimé)autreObjet;
            //Rectangle rectangleCollision = Rectangle.Intersect(RectangleDimensionsImageÀLÉchelle, autreSprite.RectangleDimensionsImageÀLÉchelle);
            //bool collision = rectangleCollision.Width == LARGEUR_NULLE && rectangleCollision.Height == HAUTEUR_NULLE;

            //autreSprite.ADétruire = collision;
            //return collision;

            //Rectangle autreRectangle = ((SpriteAnimé)autreObjet).DestinationRectangle;

            //return ZoneAffichage.Intersects(autreRectangle);

            return true;

        }

        /// <summary>
        /// Calcule les marges du sprite
        /// </summary>
        protected virtual void CalculerMarges()
        {
            MargeDroite = Game.Window.ClientBounds.Width - RectangleDimensionsImageÀLÉchelle.Width;
            MargeBas = Game.Window.ClientBounds.Height - RectangleDimensionsImageÀLÉchelle.Height;
            MargeHaut = HAUTEUR_NULLE;
            MargeGauche = LARGEUR_NULLE;
        }
    }
}
