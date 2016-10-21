/*
Sprite.cs
---------

Par Mathieu Godin

RÙle : Composant qui est un DrawableGameComponent et
       hÈrite de ICollisionnable, permet d'afficher
       un sprite ‡ l'Ècran par le biais d'un Texture2D

CrÈÈ : 5 octobre 2016
ModifiÈ : 12 octobre 2016
Description : Affiche maintenant ‡ l'Èchelle et EstEnCollision a ÈtÈ implantÈ
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
        protected const int ORDONN…E_NULLE = 0, ABSCISSE_NULLE = 0, HAUTEUR_NULLE = 0, LARGEUR_NULLE = 0, DIVISEUR_OBTENTION_DEMI_GRANDEUR = 2;

        string NomImage { get; set; }
        public Vector2 Position { get; protected set; }
        protected Rectangle ZoneAffichage { get; set; }
        protected SpriteBatch GestionSprites { get; set; }
        protected RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        /* probably private */
        protected Texture2D Image { get; private set; }
        float …chelle { get; set; }
        protected Rectangle RectangleDimensionsImage¿L…chelle { get; set; }
        protected int MargeDroite { get; set; }
        protected int MargeBas { get; set; }
        protected int MargeGauche { get; set; }
        protected int MargeHaut { get; set; }
        Vector2 DimensionsSprite¿Afficher { get; set; }

        /// <summary>
        /// Constructeur de la classe Sprite
        /// </summary>
        /// <param name="jeu">Jeu de type Game</param>
        /// <param name="nomImage">Nom de l'image tel qu'Ècrit dans son rÈpertoire</param>
        /// <param name="position">Position ‡ laquelle on veut placer le sprite</param>
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
            …chelle = Calculer…chelle();
            DimensionsSprite¿Afficher = CalculerDimensionsSprite¿Afficher();
            //Origine = new Vector2(ABSCISSE_NULLE, ORDONN…E_NULLE);
            RectangleDimensionsImage¿L…chelle = CalculerRectangleDimensionsImage¿L…chelle();
            CalculerMarges();
        }

        /// <summary>
        /// Calcule le rectangle reprÈsentant ce qui va Ítre affichÈ
        /// </summary>
        /// <returns>Le rectangle de type Rectangle reprÈsentant le pourtour de ce qui sera affichÈ</returns>
        protected Rectangle CalculerRectangleDimensionsImage¿L…chelle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)(DimensionsSprite¿Afficher.X), (int)(DimensionsSprite¿Afficher.Y));
        }

        /// <summary>
        /// Calcule les dimensions du sprite tel qu'il sera affichÈ
        /// </summary>
        /// <returns>Un vecteur de type Vector2 reprÈsentant les dimensions de ce qui sera affichÈ</returns>
        Vector2 CalculerDimensionsSprite¿Afficher()
        {
            return new Vector2(…chelle, …chelle) * CalculerDimensionsSpriteOriginal();
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
        /// Calcule l'Èchelle en calculant l'Èchelle horizontale et verticale et prenant la plus petite des deux
        /// </summary>
        /// <returns>La plus petite des Èchelles horizontales et verticales</returns>
        protected float Calculer…chelle()
        {
            float ÈchelleHorizontale = Calculer…chelleHorizontale(), ÈchelleVerticale = Calculer…chelleVerticale();

            return ÈchelleHorizontale < ÈchelleVerticale ? ÈchelleHorizontale : ÈchelleVerticale;
        }

        /// <summary>
        /// Calcule l'Èchelle horizontale du sprite pour la mÈthode Draw()
        /// </summary>
        protected virtual float Calculer…chelleHorizontale()
        {
            return ZoneAffichage.Width / (float)Image.Width;
        }

        /// <summary>
        /// Calcule l'Èchelle verticale du sprite pour la mÈthode Draw()
        /// </summary>
        protected virtual float Calculer…chelleVerticale()
        {
            return ZoneAffichage.Height / (float)Image.Height;
        }

        /// <summary>
        /// Charge le contenu nÈcessaire au fonctionnement du Sprite
        /// </summary>
        protected override void LoadContent()
        {
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Image = GestionnaireDeTextures.Find(NomImage);
        }

        /// <summary>
        /// MÈthode qui dessine le Sprite ‡ l'Ècran
        /// </summary>
        /// <param name="gameTime">Contient les informations sur le temps de jeu</param>
        public override void Draw(GameTime gameTime)
        {
            //GestionSprites.Draw(Image, Position, ZoneAffichage, Color.White, AUCUNE_ROTATION, Origine, …chelle, SpriteEffects.None, AUCUNE_COUCHE_DE_PROFONDEUR);

            GestionSprites.Draw(Image, RectangleDimensionsImage¿L…chelle, Color.White);
        }

        /// <summary>
        /// PrÈdicat vrai si le Sprite est en collision avec un autre objet
        /// </summary>
        /// <param name="autreObjet"></param>
        /// <returns></returns>
        public virtual bool EstEnCollision(object autreObjet)
        {
            //SpriteAnimÈ autreSprite = (SpriteAnimÈ)autreObjet;
            //Rectangle rectangleCollision = Rectangle.Intersect(RectangleDimensionsImage¿L…chelle, autreSprite.RectangleDimensionsImage¿L…chelle);
            //bool collision = rectangleCollision.Width == LARGEUR_NULLE && rectangleCollision.Height == HAUTEUR_NULLE;

            //autreSprite.ADÈtruire = collision;
            //return collision;

            //Rectangle autreRectangle = ((SpriteAnimÈ)autreObjet).DestinationRectangle;

            //return ZoneAffichage.Intersects(autreRectangle);

            return true;

        }

        /// <summary>
        /// Calcule les marges du sprite
        /// </summary>
        protected virtual void CalculerMarges()
        {
            MargeDroite = Game.Window.ClientBounds.Width - RectangleDimensionsImage¿L…chelle.Width;
            MargeBas = Game.Window.ClientBounds.Height - RectangleDimensionsImage¿L…chelle.Height;
            MargeHaut = HAUTEUR_NULLE;
            MargeGauche = LARGEUR_NULLE;
        }
    }
}
