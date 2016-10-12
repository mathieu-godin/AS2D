/*
Sprite.cs
---------

Par Mathieu Godin

Rôle : Composant qui est un DrawableGameComponent et
       hérite de ICollisionnable, permet d'afficher
       un sprite à l'écran par le biais d'un Texture2D

Créé : 5 octobre 2016
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
    public class Sprite : Microsoft.Xna.Framework.DrawableGameComponent, ICollisionable
    {
        string NomImage { get; set; }
        protected Vector2 Position { get; set; }
        protected Rectangle ZoneAffichage { get; set; }
        protected SpriteBatch GestionSprites { get; private set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        protected Texture2D Image { get; private set; }

        public Sprite(Game jeu, string nomImage, Vector2 position, Rectangle zoneAffichage) : base(jeu)
        {
            NomImage = nomImage;
            Position = position;
            ZoneAffichage = zoneAffichage;
        }

        protected override void LoadContent()
        {
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            Image = GestionnaireDeTextures.Find(NomImage);
        }

        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Draw(Image, Position, Color.White);
        }
        public bool EstEnCollision(object autreObjet)
        {
            //À implémenter
        }
    }
}
