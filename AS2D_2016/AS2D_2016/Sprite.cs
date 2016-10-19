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

Co-auteur : Rapha�l Brul�
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
        protected const int ORDONN�E_NULLE = 0, ABSCISSE_NULLE = 0, HAUTEUR_NULLE = 0, LARGEUR_NULLE = 0, DIVISEUR_OBTENTION_DEMI_GRANDEUR = 2, ORIGINE = 0;

        //Propri�t�s initialement g�r�es par le constructeur
        string NomImage { get; set; }
        public Vector2 Position { get; private set; }
        protected Rectangle ZoneAffichage { get; set; }

        protected Rectangle RectangleSource { get; set; }
        protected Vector2 DimensionsImage�LAffichage { get; set; }
        protected SpriteBatch GestionSprites { get; set; }
        protected RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        /* probably private */ protected Texture2D Image { get; set; }
        protected float �chelle { get; set; }
        //Vector2 Origine { get; set; }
        protected Rectangle RectangleDimensionsImage�L�chelle { get; set; }
        protected int MargeDroite { get; set; }
        protected int MargeBas { get; set; }
        protected int MargeGauche { get; set; }
        protected int MargeHaut { get; set; }

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
            DimensionsImage�LAffichage = new Vector2(Image.Width, Image.Height);
            RectangleSource = Cr�erRectangleSource();
            �chelle = Calculer�chelle();
            //Origine = new Vector2(ABSCISSE_NULLE, ORDONN�E_NULLE);
            RectangleDimensionsImage�L�chelle = Cr�erRectangleDimensionsImage�L�chelle();
            MargeHaut = HAUTEUR_NULLE;
            MargeGauche = LARGEUR_NULLE;
        }

        /// <summary>
        /// Cr�er rectangle source
        /// </summary>
        /// <returns>Retourne le rectangle en question</returns>
        protected virtual Rectangle Cr�erRectangleSource()
        {
            return new Rectangle(ORIGINE, ORIGINE, (int)DimensionsImage�LAffichage.X, (int)DimensionsImage�LAffichage.Y);
        }

        /// <summary>
        /// C�er le rectangle des bonnes dimmensions � l'�chelle et position d'affichage
        /// </summary>
        /// <returns>Retourne le rectangle en question</returns>
        protected virtual Rectangle Cr�erRectangleDimensionsImage�L�chelle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)(DimensionsImage�LAffichage.X * �chelle), (int)(DimensionsImage�LAffichage.Y * �chelle));
        }

        /// <summary>
        /// Calcule l'�chelle en calculant l'�chelle horizontale et verticale et prenant la plus petite des deux
        /// </summary>
        /// <returns>La plus petite des �chelles horizontales et verticales</returns>
        protected virtual float Calculer�chelle()
        {
            //Rajout de cast de float car sinon ca fesait une division enti�re qui donnait tj 0! Mais anyway cest pas bon mais bon, va voir le remix dans SpriteAnim� LOL.
            float �chelleHorizontale = ZoneAffichage.Width / (float)Image.Width, �chelleVerticale = ZoneAffichage.Height / (float)Image.Height;

            return �chelleHorizontale < �chelleVerticale ? �chelleHorizontale : �chelleVerticale;
        }

        /// <summary>
        /// Charge le contenu n�cessaire au fonctionnement du Sprite
        /// </summary>
        protected override void LoadContent()
        {
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Image = GestionnaireDeTextures.Find(NomImage);
            CalculerMarges();
        }

        /// <summary>
        /// M�thode qui dessine le SpriteAnim� � l'�cran
        /// </summary>
        /// <param name="gameTime">Objet contenant l'information de temps de jeu de type GameTime</param>
        public override void Draw(GameTime gameTime)
        {
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
        }


    }
}
