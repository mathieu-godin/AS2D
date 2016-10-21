/*
Sprite.cs
---------

Par Mathieu Godin

R�le : Composant qui est un DrawableGameComponent et
       h�rite de ICollisionnable, permet d'afficher
       un sprite � l'�cran par le biais d'un Texture2D

Cr�� : 5 octobre 2016
Modifi� : 12 octobre 2016
Description : Affiche maintenant � l'�chelle et EstEnCollision a �t� implant�
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
        protected const int ORDONN�E_NULLE = 0, ABSCISSE_NULLE = 0, HAUTEUR_NULLE = 0, LARGEUR_NULLE = 0, DIVISEUR_OBTENTION_DEMI_GRANDEUR = 2;

        string NomImage { get; set; }
        public Vector2 Position { get; protected set; }
        protected Rectangle ZoneAffichage { get; set; }
        protected SpriteBatch GestionSprites { get; set; }
        protected RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        /* probably private */
        Texture2D Image { get; set; }
        float �chelle { get; set; }
        protected Rectangle RectangleDimensionsImage�L�chelle { get; set; }
        protected int MargeDroite { get; set; }
        protected int MargeBas { get; set; }
        protected int MargeGauche { get; set; }
        protected int MargeHaut { get; set; }
        Vector2 DimensionsSprite�Afficher { get; set; }
        protected Vector2 DimensionsImage { get; private set; }
        protected Rectangle RectangleSource { get; set; }

        /// <summary>
        /// Constructeur de la classe Sprite
        /// </summary>
        /// <param name="jeu">Jeu de type Game</param>
        /// <param name="nomImage">Nom de l'image tel qu'�crit dans son r�pertoire</param>
        /// <param name="position">Position � laquelle on veut placer le sprite</param>
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
            �chelle = Calculer�chelle();
            DimensionsSprite�Afficher = CalculerDimensionsSprite�Afficher();
            //Origine = new Vector2(ABSCISSE_NULLE, ORDONN�E_NULLE);
            RectangleDimensionsImage�L�chelle = CalculerRectangleDimensionsImage�L�chelle();
            RectangleSource = CalculerRectangleSource();
            CalculerMarges();
        }

        /// <summary>
        /// Calcule rectangle couvrant ce qui sera affich�
        /// </summary>
        /// <returns>Le rectangle source de type Rectangle</returns>
        protected virtual Rectangle CalculerRectangleSource()
        {
            return new Rectangle(ABSCISSE_NULLE, ORDONN�E_NULLE, Image.Width, Image.Height);
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
        /// Calcule le rectangle repr�sentant ce qui va �tre affich�
        /// </summary>
        /// <returns>Le rectangle de type Rectangle repr�sentant le pourtour de ce qui sera affich�</returns>
        protected Rectangle CalculerRectangleDimensionsImage�L�chelle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)(DimensionsSprite�Afficher.X), (int)(DimensionsSprite�Afficher.Y));
        }

        /// <summary>
        /// Calcule les dimensions du sprite tel qu'il sera affich�
        /// </summary>
        /// <returns>Un vecteur de type Vector2 repr�sentant les dimensions de ce qui sera affich�</returns>
        Vector2 CalculerDimensionsSprite�Afficher()
        {
            return new Vector2(�chelle, �chelle) * CalculerDimensionsSpriteOriginal();
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
        /// Calcule l'�chelle en calculant l'�chelle horizontale et verticale et prenant la plus petite des deux
        /// </summary>
        /// <returns>La plus petite des �chelles horizontales et verticales</returns>
        protected float Calculer�chelle()
        {
            float �chelleHorizontale = Calculer�chelleHorizontale(), �chelleVerticale = Calculer�chelleVerticale();

            return �chelleHorizontale < �chelleVerticale ? �chelleHorizontale : �chelleVerticale;
        }

        /// <summary>
        /// Calcule l'�chelle horizontale du sprite pour la m�thode Draw()
        /// </summary>
        protected virtual float Calculer�chelleHorizontale()
        {
            return ZoneAffichage.Width / (float)Image.Width;
        }

        /// <summary>
        /// Calcule l'�chelle verticale du sprite pour la m�thode Draw()
        /// </summary>
        protected virtual float Calculer�chelleVerticale()
        {
            return ZoneAffichage.Height / (float)Image.Height;
        }

        /// <summary>
        /// Charge le contenu n�cessaire au fonctionnement du Sprite
        /// </summary>
        protected override void LoadContent()
        {
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Image = GestionnaireDeTextures.Find(NomImage);
        }

        /// <summary>
        /// M�thode qui dessine le Sprite � l'�cran
        /// </summary>
        /// <param name="gameTime">Contient les informations sur le temps de jeu</param>
        public override void Draw(GameTime gameTime)
        {
            //GestionSprites.Draw(Image, Position, ZoneAffichage, Color.White, AUCUNE_ROTATION, Origine, �chelle, SpriteEffects.None, AUCUNE_COUCHE_DE_PROFONDEUR);

            GestionSprites.Draw(Image, RectangleDimensionsImage�L�chelle, RectangleSource, Color.White);
        }

        /// <summary>
        /// Pr�dicat vrai si le Sprite est en collision avec un autre objet
        /// </summary>
        /// <param name="autreObjet"></param>
        /// <returns></returns>
        public virtual bool EstEnCollision(object autreObjet)
        {
            //SpriteAnim� autreSprite = (SpriteAnim�)autreObjet;
            //Rectangle rectangleCollision = Rectangle.Intersect(RectangleDimensionsImage�L�chelle, autreSprite.RectangleDimensionsImage�L�chelle);
            //bool collision = rectangleCollision.Width == LARGEUR_NULLE && rectangleCollision.Height == HAUTEUR_NULLE;

            //autreSprite.AD�truire = collision;
            //return collision;

            //Rectangle autreRectangle = ((SpriteAnim�)autreObjet).DestinationRectangle;

            //return ZoneAffichage.Intersects(autreRectangle);

            return true;

        }

        /// <summary>
        /// Calcule les marges du sprite
        /// </summary>
        protected virtual void CalculerMarges()
        {
            MargeDroite = Game.Window.ClientBounds.Width - RectangleDimensionsImage�L�chelle.Width;
            MargeBas = Game.Window.ClientBounds.Height - RectangleDimensionsImage�L�chelle.Height;
            MargeHaut = HAUTEUR_NULLE;
            MargeGauche = LARGEUR_NULLE;
        }
    }
}
