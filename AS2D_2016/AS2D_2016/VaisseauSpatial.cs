/* Auteur :            Raphaël Brulé
   Fichier :           VaisseauSpatial.cs
   Date :              le 05 octobre 2016
   Description :       Ce component, enfant de SpriteAnimé, permet
                       de gérer le vaisseau spatial.*/

// Co-auteur : Mathieu Godin
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtelierXNA
{
    /// <summary>
    /// Ce component, enfant de SpriteAnimé, permet de gérer le vaisseau spatial
    /// </summary>
    public class VaisseauSpatial : SpriteAnimé
    {
        const int NE_SE_DÉPLACE_PAS = 0, SE_DÉPLACE = 1, NB_PIXELS_DE_DÉPLACEMENT = 4, NB_DE_MISSILES_MAX = 3, HAUTEUR_MISSILE_MAX = 40, NB_DE_MISSILE_DANSLIMAGE = 25, ANIMATION_DE_BASE = 0, DEMI_LARGEUR_CANON_VAISSEAU = 4, UNITÉ_ANIMATION = 1, LARGEUR_ANIMATION = 5, HAUTEUR_ANIMATION = 4, AUCUNE_ACCÉLÉRATION = 0;
        const string CHAÎNE_IMAGE_MISSILE = "Missile", CHAÎNE_IMAGE_EXPLOSION = "Explosion";
        const float FACTEUR_ACCÉLÉRATION = 1f / 600F, INTERVALLE_MIN = 0.01F, INTERVALLE_MAX = 1;

        float IntervalleMAJDéplacement { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        int AnimationSelonLeDéplacement { get; set; }
        Vector2 AnciennePosition { get; set; }
        int OrdonnéeFinaleVaisseau { get; set; }
        bool EnDescente { get; set; }
        Vector2 VecteurDéplacementDescente { get; set; }
        Vector2 DéplacementRésultant { get; set; }
        InputManager GestionInput { get; set; }
        Vector2 PositionSupplémentaireMissile { get; set; }

        /// <summary>
        /// Constructeur de VaisseauSpatial
        /// </summary>
        /// <param name="jeu">Objet de classe Game</param>
        /// <param name="nomImage">Nom de l'image (string)</param>
        /// <param name="position">Position (Vector2)</param>
        /// <param name="zoneAffichage">Zone d'affichage (Rectangle)</param>
        /// <param name="descriptionImage">Description de l'image (Vector2)</param>
        /// <param name="intervalleMAJAnimation">Intervalle de mise à jour de l'animation (float)</param>
        /// <param name="intervalleMAJDéplacement">Intervalle de mise à jour du déplacement (float)</param>
        public VaisseauSpatial(Game jeu, string nomImage, Vector2 position, Rectangle zoneAffichage, Vector2 descriptionImage, float intervalleMAJAnimation, float intervalleMAJDéplacement) : base(jeu, nomImage, position, zoneAffichage, descriptionImage, intervalleMAJAnimation)
        {
            IntervalleMAJDéplacement = intervalleMAJDéplacement;
        }

        /// <summary>
        /// Initialise les propriétés du vaisseau spatial
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            TempsÉcouléDepuisMAJ = AUCUN_TEMPS_ÉCOULÉ;
            AnimationSelonLeDéplacement = ANIMATION_DE_BASE; 
            Position = new Vector2(Position.X - DimensionsSpriteÀAfficher.X / DIVISEUR_OBTENTION_DEMI_GRANDEUR, Position.Y - DimensionsSpriteÀAfficher.Y / DIVISEUR_OBTENTION_DEMI_GRANDEUR);
            AnciennePosition = new Vector2(Position.X, Position.Y);
            OrdonnéeFinaleVaisseau = Game.Window.ClientBounds.Height - (int)DimensionsSpriteÀAfficher.Y; 
            EnDescente = true;
            VecteurDéplacementDescente = new Vector2(AUCUN_DÉPLACEMENT, NB_PIXELS_DE_DÉPLACEMENT);
            DéplacementRésultant = Position - AnciennePosition;
            PositionSupplémentaireMissile = new Vector2(DimensionsSpriteÀAfficher.X / DIVISEUR_OBTENTION_DEMI_GRANDEUR - DEMI_LARGEUR_CANON_VAISSEAU, DimensionsSpriteÀAfficher.Y / DEMI_LARGEUR_CANON_VAISSEAU);
        }

        /// <summary>
        /// Charge le(s) composent(s) nécessaire(s) au vaiseau spatial
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        /// <summary>
        /// Méthode qui met à jour le SpriteAnimé selon le temps écoulé
        /// </summary>
        protected override void EffectuerMiseÀJourAnimation()
        {
            RectangleSource = new Rectangle((RectangleSource.X + (int)Delta.X) % (int)DimensionsImage.X, (int)Delta.Y * AnimationSelonLeDéplacement, (int)Delta.X, (int)Delta.Y);
        }

        /// <summary>
        /// Méthode update du composant vaisseau
        /// </summary>
        /// <param name="gameTime">Objet de classe GameTime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            VérifierLancementMissile();
            MettreÀJourVaisseau(gameTime);
        }

        /// <summary>
        /// Vérifie si la touche Espace a été appuyée afin de lancer un missile si c'est le cas
        /// </summary>
        void VérifierLancementMissile()
        {
            if (GestionInput.EstNouvelleTouche(Keys.Space))
            {
                LancerMissile();
            }
        }

        /// <summary>
        /// Gère la structure de temps du vaisseau
        /// </summary>
        void MettreÀJourVaisseau(GameTime gameTime)
        {
            TempsÉcouléDepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJDéplacement)
            {
                TempsÉcouléDepuisMAJ = AUCUN_TEMPS_ÉCOULÉ;
                DéterminerSiVaisseauEnDescente();
                EffectuerMiseÀJourDéplacement();
            }
        }

        /// <summary>
        /// Détermine si on est au début de la partie avec le vaisseau en descente pour aller le descendre si c'est le cas
        /// </summary>
        void DéterminerSiVaisseauEnDescente()
        {
            if (EnDescente)
            {
                GérerDescenteDuVaisseau();
            }
        }

        /// <summary>
        /// Gère la descente du vaisseau au début de la partie
        /// </summary>
        void GérerDescenteDuVaisseau()
        {
            Position += VecteurDéplacementDescente;
            CalculerRectangleImageÀAfficher();
            if (Position.Y >= OrdonnéeFinaleVaisseau)
            {
                Position = new Vector2(Position.X, OrdonnéeFinaleVaisseau);
                EnDescente = false;
            }
        }

        /// <summary>
        /// Effectue la mise à jour du déplacement selon les touches appuyées sur le clavier
        /// </summary>
        void EffectuerMiseÀJourDéplacement()
        {
            AnciennePosition = new Vector2(Position.X, Position.Y);
            GérerClavier();
            CalculerRectangleImageÀAfficher();
            DéplacementRésultant = Position - AnciennePosition;
            AnimationSelonLeDéplacement = (SeDéplace() ? SE_DÉPLACE : NE_SE_DÉPLACE_PAS);
        }

        /// <summary>
        /// Gère le déplacement horizontal du vaisseau selon les touches A et D
        /// </summary>
        void GérerClavier()
        {
            if (GestionInput.EstClavierActivé)
            {
                int déplacementHorizontal = GérerTouche(Keys.D) - GérerTouche(Keys.A);
                GérerAccélération();
                VérifierSiAjustementPositionNécessaire(déplacementHorizontal);
            }
        }

        /// <summary>
        /// Vérifie si on a bougé et si on doit ainsi ajuster la Position du VaisseauSpatial
        /// </summary>
        /// <param name="déplacementHorizontal">Déplacement horizontal effectué par le VaisseauSpatial</param>
        void VérifierSiAjustementPositionNécessaire(int déplacementHorizontal)
        {
            if (déplacementHorizontal != AUCUN_DÉPLACEMENT)
            {
                AjusterPosition(déplacementHorizontal);
            }
        }

        void GérerAccélération()
        {
            int modificateurAccélération = GérerTouche(Keys.PageDown) - GérerTouche(Keys.PageUp);
            if (modificateurAccélération != AUCUNE_ACCÉLÉRATION)
            {
                IntervalleMAJDéplacement += modificateurAccélération * FACTEUR_ACCÉLÉRATION;
                IntervalleMAJDéplacement = MathHelper.Max(MathHelper.Min(IntervalleMAJDéplacement, INTERVALLE_MAX), INTERVALLE_MIN);
            }
        }

        /// <summary>
        /// Renvoit le nombre de pixels de déplacement si la touche est enfoncée, sinon renvoit un zéro
        /// </summary>
        /// <param name="touche">Touche enfoncée</param>
        /// <returns>Nombre de pixels de déplacement ou zéro</returns>
        int GérerTouche(Keys touche)
        {
            return GestionInput.EstEnfoncée(touche) ? NB_PIXELS_DE_DÉPLACEMENT : AUCUN_DÉPLACEMENT;
        }

        /// <summary>
        /// Ajuste la propriété position selon le déplacement horizontal
        /// </summary>
        /// <param name="déplacementHorizontal">Déplacement horizontal</param>
        void AjusterPosition(int déplacementHorizontal)
        {
            float abscisse = CalculerPosition(déplacementHorizontal, Position.X, MargeGauche, MargeDroite);

            Position = new Vector2(abscisse, Position.Y);
        }

        /// <summary>
        /// Calcul la position horizontale selon le changement
        /// </summary>
        /// <param name="déplacement">Déplacement horizontal</param>
        /// <param name="posActuelle">Postion actuelle en </param>
        /// <param name="BorneMin">Borne minimale de la position</param>
        /// <param name="BorneMax">Borne maximale de la position</param>
        /// <returns>La position horizontale</returns>
        float CalculerPosition(int déplacement, float posActuelle, int BorneMin, int BorneMax)
        {
            float position = posActuelle + déplacement;
            return MathHelper.Min(MathHelper.Max(BorneMin, position), BorneMax);
        }

        /// <summary>
        /// Vérifie si le vaisseau se déplace
        /// </summary>
        /// <returns>Vrai ou faux</returns>
        bool SeDéplace()
        {
            return DéplacementRésultant != Vector2.Zero;
        }

        /// <summary>
        /// S'occuppe de lancer les missiles selon un maximum défini
        /// </summary>
        void LancerMissile()
        {
            int nbreDeMissiles = (Game.Components.Where(composant => composant is Missile && !((Missile)composant).ADétruire && ((Missile)composant).Visible).Count());

            if (nbreDeMissiles < NB_DE_MISSILES_MAX)
            {
                Missile missile = new Missile(Game, CHAÎNE_IMAGE_MISSILE, new Vector2(Position.X + PositionSupplémentaireMissile.X, Position.Y - PositionSupplémentaireMissile.Y), new Rectangle(ABSCISSE_NULLE, ORDONNÉE_NULLE, HAUTEUR_MISSILE_MAX, HAUTEUR_MISSILE_MAX), new Vector2(NB_DE_MISSILE_DANSLIMAGE, UNITÉ_ANIMATION), CHAÎNE_IMAGE_EXPLOSION, new Vector2(LARGEUR_ANIMATION, HAUTEUR_ANIMATION), IntervalleMAJAnnimation, IntervalleMAJDéplacement);
                Game.Components.Add(missile);
            }
        }
    }
}
