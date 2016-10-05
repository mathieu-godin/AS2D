/* Auteur :            Raphaël Brulé
   Fichier :           ArrièrePlanSpatial.cs
   Date :              le 5 octobre 2016
   Description :       Ce DrawableGameComponent permet à l'arrière-plan
                       de se déplacer du haut vers le bas, et ce, à répétition.*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AtelierXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ArrièrePlanSpatial : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //Propriétés initialement gérées par le constructeur
        string NomImage { get; set; }
        float IntervalleMAJ { get; set; }

        //Propriétés initialement gérées par Initialize
        float TempsÉcouléDepuisMAJ { get; set; }
        Rectangle ZoneAffichagePremièreImage { get; set; }
        Rectangle ZoneAffichageSecondeImage { get; set; }

        //Propriétés initialement gérées par LoadContent
        SpriteBatch GestionSprites { get; set; }
        Texture2D ImageDeFond { get; set; }

        /// <summary>
        /// Constructeur du DrawableGameComponent
        /// </summary>
        /// <param name="jeu">Objet de classe Game</param>
        /// <param name="nomImage">Nom de l'image (string)</param>
        /// <param name="intervalleMAJ">Intervalle de mise à jour (float)</param>
        public ArrièrePlanSpatial(Game jeu, string nomImage, float intervalleMAJ)
            : base(jeu)
        {
            NomImage = nomImage;
            IntervalleMAJ = intervalleMAJ;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            TempsÉcouléDepuisMAJ = 0;
            ZoneAffichagePremièreImage = new Rectangle(0, -Game.Window.ClientBounds.Height,
                            Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            ZoneAffichageSecondeImage = new Rectangle(0, 0, 
                            Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            base.Initialize();
        }

        /// <summary>
        /// Charge et gère davantage de contenu nécessaire
        /// </summary>
        protected override void LoadContent()
        {
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            ImageDeFond = Game.Content.Load<Texture2D>("Textures/" + NomImage);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;

            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                GérerImagesDeFond();

                TempsÉcouléDepuisMAJ = 0;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Gère les images déroulantes
        /// </summary>
        private void GérerImagesDeFond()
        {
            ZoneAffichagePremièreImage = new Rectangle(0, ZoneAffichagePremièreImage.Y + 1,
                        Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            ZoneAffichageSecondeImage = new Rectangle(0, ZoneAffichageSecondeImage.Y + 1,
                        Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);

            if (ZoneAffichagePremièreImage.Y > Game.Window.ClientBounds.Height)
            {
                ZoneAffichagePremièreImage = new Rectangle(0, -Game.Window.ClientBounds.Height + 1,
                        Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            }

            if (ZoneAffichageSecondeImage.Y > Game.Window.ClientBounds.Height)
            {
                ZoneAffichageSecondeImage = new Rectangle(0, -Game.Window.ClientBounds.Height + 1,
                        Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
            }
        }

        /// <summary>
        /// Gère l'affichage des images déroulantes
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Draw(ImageDeFond, ZoneAffichagePremièreImage, Color.White);
            GestionSprites.Draw(ImageDeFond, ZoneAffichageSecondeImage, Color.White);

            base.Draw(gameTime);
        }
    }
}
