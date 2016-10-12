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
        const int DIVISEUR_OBTENTION_DEMI_GRANDEUR = 2;
        const float AUCUNE_COUCHE_DE_PROFONDEUR = 0.0F;
        const float AUCUNE_ROTATION = 0.0F;
        const float ORDONN�E_NULLE = 0.0F;
        const float ABSCISSE_NULLE = 0.0F;
        const float HAUTEUR_NULLE = 0.0F;
        const float LARGEUR_NULLE = 0.0F;

        string NomImage { get; set; }
        protected Vector2 Position { get; set; }
        Rectangle ZoneAffichage { get; set; }
        SpriteBatch GestionSprites { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        protected Texture2D Image { get; set; }
        float �chelle { get; set; }
        Vector2 Origine { get; set; }
        Rectangle RectangleDimensionsImage�L�chelle { get; set; }

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
            �chelle = Calculer�chelle();
            Origine = new Vector2(ABSCISSE_NULLE, ORDONN�E_NULLE);
            RectangleDimensionsImage�L�chelle = new Rectangle((int)ABSCISSE_NULLE, (int)ORDONN�E_NULLE, (int)(Image.Width * �chelle), (int)(Image.Height * �chelle));
        }

        /// <summary>
        /// Calcule l'�chelle en calculant l'�chelle horizontale et verticale et prenant la plus petite des deux
        /// </summary>
        /// <returns>La plus petite des �chelles horizontales et verticales</returns>
        float Calculer�chelle()
        {
            float �chelleHorizontale = ZoneAffichage.Width / Image.Width, �chelleVerticale = ZoneAffichage.Height / Image.Height;

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
        }

        /// <summary>
        /// M�thode qui dessine le Sprite � l'�cran
        /// </summary>
        /// <param name="gameTime">Contient les informations sur le temps de jeu</param>
        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Draw(Image, Position, ZoneAffichage, Color.White, AUCUNE_ROTATION, Origine, �chelle, SpriteEffects.None, AUCUNE_COUCHE_DE_PROFONDEUR);
        }

        /// <summary>
        /// Pr�dicat vrai si le Sprite est en collision avec un autre objet
        /// </summary>
        /// <param name="autreObjet"></param>
        /// <returns></returns>
        public bool EstEnCollision(object autreObjet)
        {
            Sprite autreSprite = (Sprite)autreObjet;
            Rectangle rectangleCollision = Rectangle.Intersect(RectangleDimensionsImage�L�chelle, autreSprite.RectangleDimensionsImage�L�chelle);

            return rectangleCollision.Width == LARGEUR_NULLE && rectangleCollision.Height == HAUTEUR_NULLE;
        }
    }
}
