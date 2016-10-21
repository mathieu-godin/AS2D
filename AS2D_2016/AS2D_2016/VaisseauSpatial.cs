/* Auteur :            Raphaël Brulé
   Fichier :           VaisseauSpatial.cs
   Date :              le 05 octobre 2016
   Description :       Ce component, enfant de SpriteAnimé, permet
                       de gérer le vaisseau spatial.*/

// Modification : Modifications pour la descente du vaisceau au début
//                Mathieu Godin
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtelierXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class VaisseauSpatial : SpriteAnimé
    {
        //Constante
        const int NE_SE_DÉPLACE_PAS = 0;
        const int SE_DÉPLACE = 1;
        const int NB_PIXELS_DE_DÉPLACEMENT = 4; // Je l'ai changé de 5 à 4 car ça ressemblait plus à l'exemple d'exécution
        const int NB_DE_MISSILES_MAX = 3;

        //Propriété initialement gérée par le constructeur
        float IntervalleMAJDéplacement { get; set; }

        //Propriété initialement gérée par Initialize
        float TempsÉcouléDepuisMAJ { get; set; }
        int AnimationSelonLeDéplacement { get; set; }
        Vector2 AnciennePosition { get; set; }
        // Ajouté par Mathieu Godin pour la descente du vaisseau
        int OrdonnéeFinaleVaisseau { get; set; }
        bool EnDescente { get; set; }
        Vector2 VecteurDéplacementDescente { get; set; } // D'autres similaires pourraient être utilisés dans le reste de la classe pour optimiser
        Vector2 DéplacementRésultant { get; set; }

        //Propriété initialement gérée par LoadContent
        InputManager GestionInput { get; set; }


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
        public VaisseauSpatial(Game jeu, string nomImage,
                               Vector2 position, Rectangle zoneAffichage,
                               Vector2 descriptionImage, float intervalleMAJAnimation,
                               float intervalleMAJDéplacement)
            : base(jeu, nomImage, position, zoneAffichage,
                  descriptionImage, intervalleMAJAnimation)
        {
            IntervalleMAJDéplacement = intervalleMAJDéplacement;
        }

        /// <summary>
        /// Initialise les propriétés du vaisseau spatial
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            TempsÉcouléDepuisMAJ = 0;
            AnimationSelonLeDéplacement = 0;
            //À effacer avec la descente du vaisseau maintenant : Position = new Vector2(Position.X - DestinationRectangle.Width/2, Game.Window.ClientBounds.Height - DestinationRectangle.Height); 
            Position = new Vector2(Position.X - RectangleDimensionsImageÀLÉchelle.Width / DIVISEUR_OBTENTION_DEMI_GRANDEUR, Position.Y - RectangleDimensionsImageÀLÉchelle.Height / DIVISEUR_OBTENTION_DEMI_GRANDEUR); // Nouvelle ligne
            AnciennePosition = new Vector2(Position.X, Position.Y);
            OrdonnéeFinaleVaisseau = Game.Window.ClientBounds.Height - RectangleDimensionsImageÀLÉchelle.Height; 
            EnDescente = true;
            VecteurDéplacementDescente = new Vector2(AUCUN_DÉPLACEMENT, NB_PIXELS_DE_DÉPLACEMENT);
            DéplacementRésultant = Position - AnciennePosition;
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
        /// Effectuer la mise à jour de l'animation du vaiseau (en lien avec le déplacement)
        /// </summary>
        protected override void EffectuerMiseÀJourAnimation()
        {
            RectangleSource = new Rectangle((RectangleSource.X + (int)Delta.X) % Image.Width,
                             (int)Delta.Y * AnimationSelonLeDéplacement,
                             (int)Delta.X, (int)Delta.Y);
        }

        /// <summary>
        /// Méthode update du composant vaisseau
        /// </summary>
        /// <param name="gameTime">Objet de classe GameTime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Ajout pr missile
            if (GestionInput.EstNouvelleTouche(Keys.Space))
                LancerMissile();

            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJDéplacement)
            {
                DéterminerSiVaisseauEnDescente();
                EffectuerMiseÀJourDéplacement();
                TempsÉcouléDepuisMAJ = AUCUN_TEMPS_ÉCOULÉ;
            }
        }

        /// <summary>
        /// Détermine si on est au début de la partie avec le vaisseau en descente pour aller le descendre si c'est le cas
        /// </summary>
        void DéterminerSiVaisseauEnDescente()
        {
            if (EnDescente)
            {
                GérerDescenteDuVaisseau(); // Nouvelle méthode
            }
        }

        /// <summary>
        /// Gère la descente du vaisseau au début de la partie
        /// </summary>
        void GérerDescenteDuVaisseau()
        {
            Position += VecteurDéplacementDescente;
            RectangleDimensionsImageÀLÉchelle = CalculerRectangleDimensionsImageÀLÉchelle();
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
            RectangleDimensionsImageÀLÉchelle = CalculerRectangleDimensionsImageÀLÉchelle();
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
                AjusterPosition(déplacementHorizontal);
            }
        }

        /// <summary>
        /// Renvoit le nombre de pixels de déplacement si la touche est enfoncée, sinon renvoit un zéro
        /// </summary>
        /// <param name="touche">Touche enfoncée</param>
        /// <returns>Nombre de pixels de déplacement ou zéro</returns>
        int GérerTouche(Keys touche)
        {
            return GestionInput.EstEnfoncée(touche) ? NB_PIXELS_DE_DÉPLACEMENT : 0;
        }

        /// <summary>
        /// Ajuste la propriété position selon le déplacement horizontal
        /// </summary>
        /// <param name="déplacementHorizontal">Déplacement horizontal</param>
        void AjusterPosition(int déplacementHorizontal)
        {
            float posX = CalculerPosition(déplacementHorizontal, Position.X, MargeGauche, MargeDroite);

            Position = new Vector2(posX, Position.Y);
        }

        /// <summary>
        /// Calcul la position horizontale selon le changement
        /// </summary>
        /// <param name="déplacement">Déplacement horizontal</param>
        /// <param name="posActuelle">Postion actuelle en </param>
        /// <param name="BorneMin"></param>
        /// <param name="BorneMax"></param>
        /// <returns></returns>
        float CalculerPosition(int déplacement, float posActuelle, int BorneMin, int BorneMax)
        {
            float position = posActuelle + déplacement;
            return MathHelper.Min(MathHelper.Max(BorneMin, position), BorneMax);
        }

        bool SeDéplace()
        {
            return DéplacementRésultant != Vector2.Zero;
        }

        void LancerMissile()
        {
            int nbreDeMissiles = (Game.Components.Where(composant => composant is Missile && !((Missile)composant).ADétruire && ((Missile)composant).Visible).Count());

            if (nbreDeMissiles < NB_DE_MISSILES_MAX)
            {
                Missile missile = new Missile(Game,
                                                "Missile",
                                                new Vector2(RectangleDimensionsImageÀLÉchelle.X + RectangleDimensionsImageÀLÉchelle.Width / 2 - 4, RectangleDimensionsImageÀLÉchelle.Y - RectangleDimensionsImageÀLÉchelle.Height / 4),
                                                new Rectangle(0, 0, 30, 40),
                                                new Vector2(25, 1),
                                                "Explosion",
                                                new Vector2(5, 4),
                                                1.5f * Atelier.INTERVALLE_STANDARDS,
                                                Atelier.INTERVALLE_STANDARDS);
                Game.Components.Add(missile);
            }
        }

    }
}
