/*
ArrièrePlanSpatial.cs
---------------------

Par Mathieu Godin

Rôle : Composant qui un arrière-plan
       déroulant de haut en bas de
       ciel étoilé
Créé : 5 octobre 2016
Co-auteur : Raphaël Brûlé
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtelierXNA
{
    /// <summary>
    /// Composant qui permet d'afficher un arrière plan déroulant à la verticale
    /// </summary>
    public class ArrièrePlanSpatial : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const float AUCUN_TEMPS_ÉCOULÉ = 0.0F;
        const float ORDONNÉE_NULLE = 0.0F;
        const float ABSCISSE_NULLE = 0.0F;
        const float INCRÉMENT_ORDONNÉE = 0.3F;
        const float ÉCHELLE = 4.0F / 7.0F;
        const float AUCUN_ANGLE = 0.0F;
        const float AUCUNE_ÉPAISSEUR_DE_COUCHE = 0.0F;

        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        float HauteurImageMiseÀLÉchelle { get; set; }
        Vector2 VecteurIncrément { get; set; }
        Vector2 PositionPremierFond { get; set; }
        Vector2 PositionDeuxièmeFond { get; set; }
        SpriteBatch GestionSprites { get; set; }
        string NomImage { get; set; }
        Texture2D ImageDeFond { get; set; }

        /// <summary>
        /// Constructeur prenant en charge le jeu, le nom de l'image que l'on veut dérouler en arrière-plan et l'intervalle de mise à jour auquelle celle-ci sera déroulée
        /// </summary>
        /// <param name="jeu">Jeu de type Game</param>
        /// <param name="nomImage">Chaîne de caractères représentant l'image que l'on veut afficher comme arrière-plan déroulant</param>
        /// <param name="intervalleMAJ">Intervalle de mise à jour en secondes auquel on veut défiler l'arrière-plan</param>
        public ArrièrePlanSpatial(Game jeu, string nomImage, float intervalleMAJ) : base(jeu)
        {
            NomImage = nomImage;
            IntervalleMAJ = intervalleMAJ;
        }

        /// <summary>
        /// Méthode qui initialise les différentes propriétés de l'objet à leur valeur propre au commencement
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            TempsÉcouléDepuisMAJ = AUCUN_TEMPS_ÉCOULÉ;
            VecteurIncrément = new Vector2(ABSCISSE_NULLE, INCRÉMENT_ORDONNÉE);
            HauteurImageMiseÀLÉchelle = ImageDeFond.Height * ÉCHELLE;
            ReplacerFonds();
        }

        /// <summary>
        /// Replace les deux arrière-plans à leur position initiale afin de continuer le défilement de l'arrière-paln indéfinément
        /// </summary>
        void ReplacerFonds()
        {
            PositionPremierFond = new Vector2(ABSCISSE_NULLE, ORDONNÉE_NULLE);
            PositionDeuxièmeFond = new Vector2(ABSCISSE_NULLE, -HauteurImageMiseÀLÉchelle);
        }

        /// <summary>
        /// Charge du contenu nécessaire à l'objet
        /// </summary>
        protected override void LoadContent()
        {
            ImageDeFond = (Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>).Find(NomImage);
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
        }

        /// <summary>
        /// Méthode qui met à jour le contenu de l'objet et que s'occupe de la gestion du temps
        /// </summary>
        /// <param name="gameTime">Objet contenant les informations sur le temps lié au jeu</param>
        public override void Update(GameTime gameTime)
        {
            TempsÉcouléDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                TempsÉcouléDepuisMAJ = AUCUN_TEMPS_ÉCOULÉ;
                PositionPremierFond += VecteurIncrément;
                PositionDeuxièmeFond += VecteurIncrément;
                VérifierSiInversementNécessaire();
            }
        }

        /// <summary>
        /// Vérifie si les deux arrière-plans ont parcouru la largeur de la fenêtre afin de les replacer à leurs positions initiales si c'est le cas
        /// </summary>
        void VérifierSiInversementNécessaire()
        {
            if (PositionPremierFond.Y >= HauteurImageMiseÀLÉchelle)
            {
                ReplacerFonds();
            }
        }

        /// <summary>
        /// Dessine les deux arrière-plans à l'écran
        /// </summary>
        /// <param name="gameTime">Informations sur le temps de jeu de type GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Draw(ImageDeFond, PositionPremierFond, null, Color.White, AUCUN_ANGLE, Vector2.Zero, ÉCHELLE, SpriteEffects.None, AUCUNE_ÉPAISSEUR_DE_COUCHE);
            GestionSprites.Draw(ImageDeFond, PositionDeuxièmeFond, null, Color.White, AUCUN_ANGLE, Vector2.Zero, ÉCHELLE, SpriteEffects.None, AUCUNE_ÉPAISSEUR_DE_COUCHE);
        }
    }
}
