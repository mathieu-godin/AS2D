/* Auteur :            Rapha�l Brul�
   Fichier :           VaisseauSpatial.cs
   Date :              le 05 octobre 2016
   Description :       Ce component, enfant de SpriteAnim�, permet
                       de g�rer le vaisseau spatial.*/

// Modification : Modifications pour la descente du vaisceau au d�but
//                Mathieu Godin
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtelierXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class VaisseauSpatial : SpriteAnim�
    {
        //Constante
        const int NE_SE_D�PLACE_PAS = 0;
        const int SE_D�PLACE = 1;
        const int NB_PIXELS_DE_D�PLACEMENT = 4; // Je l'ai chang� de 5 � 4 car �a ressemblait plus � l'exemple d'ex�cution
        const int NB_DE_MISSILES_MAX = 3;

        //Propri�t� initialement g�r�e par le constructeur
        float IntervalleMAJD�placement { get; set; }

        //Propri�t� initialement g�r�e par Initialize
        float Temps�coul�DepuisMAJ { get; set; }
        int AnimationSelonLeD�placement { get; set; }
        Vector2 AnciennePosition { get; set; }
        // Ajout� par Mathieu Godin pour la descente du vaisseau
        int Ordonn�eFinaleVaisseau { get; set; }
        bool EnDescente { get; set; }
        Vector2 VecteurD�placementDescente { get; set; } // D'autres similaires pourraient �tre utilis�s dans le reste de la classe pour optimiser
        Vector2 D�placementR�sultant { get; set; }

        //Propri�t� initialement g�r�e par LoadContent
        InputManager GestionInput { get; set; }


        /// <summary>
        /// Constructeur de VaisseauSpatial
        /// </summary>
        /// <param name="jeu">Objet de classe Game</param>
        /// <param name="nomImage">Nom de l'image (string)</param>
        /// <param name="position">Position (Vector2)</param>
        /// <param name="zoneAffichage">Zone d'affichage (Rectangle)</param>
        /// <param name="descriptionImage">Description de l'image (Vector2)</param>
        /// <param name="intervalleMAJAnimation">Intervalle de mise � jour de l'animation (float)</param>
        /// <param name="intervalleMAJD�placement">Intervalle de mise � jour du d�placement (float)</param>
        public VaisseauSpatial(Game jeu, string nomImage,
                               Vector2 position, Rectangle zoneAffichage,
                               Vector2 descriptionImage, float intervalleMAJAnimation,
                               float intervalleMAJD�placement)
            : base(jeu, nomImage, position, zoneAffichage,
                  descriptionImage, intervalleMAJAnimation)
        {
            IntervalleMAJD�placement = intervalleMAJD�placement;
        }

        /// <summary>
        /// Initialise les propri�t�s du vaisseau spatial
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Temps�coul�DepuisMAJ = 0;
            AnimationSelonLeD�placement = 0;
            //� effacer avec la descente du vaisseau maintenant : Position = new Vector2(Position.X - DestinationRectangle.Width/2, Game.Window.ClientBounds.Height - DestinationRectangle.Height); 
            Position = new Vector2(Position.X - RectangleDimensionsImage�L�chelle.Width / DIVISEUR_OBTENTION_DEMI_GRANDEUR, Position.Y - RectangleDimensionsImage�L�chelle.Height / DIVISEUR_OBTENTION_DEMI_GRANDEUR); // Nouvelle ligne
            AnciennePosition = new Vector2(Position.X, Position.Y);
            Ordonn�eFinaleVaisseau = Game.Window.ClientBounds.Height - RectangleDimensionsImage�L�chelle.Height; 
            EnDescente = true;
            VecteurD�placementDescente = new Vector2(AUCUN_D�PLACEMENT, NB_PIXELS_DE_D�PLACEMENT);
            D�placementR�sultant = Position - AnciennePosition;
        }

        /// <summary>
        /// Charge le(s) composent(s) n�cessaire(s) au vaiseau spatial
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        /// <summary>
        /// Effectuer la mise � jour de l'animation du vaiseau (en lien avec le d�placement)
        /// </summary>
        protected override void EffectuerMise�JourAnimation()
        {
            RectangleSource = new Rectangle((RectangleSource.X + (int)Delta.X) % Image.Width,
                             (int)Delta.Y * AnimationSelonLeD�placement,
                             (int)Delta.X, (int)Delta.Y);
        }

        /// <summary>
        /// M�thode update du composant vaisseau
        /// </summary>
        /// <param name="gameTime">Objet de classe GameTime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Ajout pr missile
            if (GestionInput.EstNouvelleTouche(Keys.Space))
                LancerMissile();

            float Temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += Temps�coul�;
            if (Temps�coul�DepuisMAJ >= IntervalleMAJD�placement)
            {
                D�terminerSiVaisseauEnDescente();
                EffectuerMise�JourD�placement();
                Temps�coul�DepuisMAJ = AUCUN_TEMPS_�COUL�;
            }
        }

        /// <summary>
        /// D�termine si on est au d�but de la partie avec le vaisseau en descente pour aller le descendre si c'est le cas
        /// </summary>
        void D�terminerSiVaisseauEnDescente()
        {
            if (EnDescente)
            {
                G�rerDescenteDuVaisseau(); // Nouvelle m�thode
            }
        }

        /// <summary>
        /// G�re la descente du vaisseau au d�but de la partie
        /// </summary>
        void G�rerDescenteDuVaisseau()
        {
            Position += VecteurD�placementDescente;
            RectangleDimensionsImage�L�chelle = CalculerRectangleDimensionsImage�L�chelle();
            if (Position.Y >= Ordonn�eFinaleVaisseau)
            {
                Position = new Vector2(Position.X, Ordonn�eFinaleVaisseau);
                EnDescente = false;
            }
        }

        /// <summary>
        /// Effectue la mise � jour du d�placement selon les touches appuy�es sur le clavier
        /// </summary>
        void EffectuerMise�JourD�placement()
        {
            AnciennePosition = new Vector2(Position.X, Position.Y);
            G�rerClavier();
            RectangleDimensionsImage�L�chelle = CalculerRectangleDimensionsImage�L�chelle();
            D�placementR�sultant = Position - AnciennePosition;
            AnimationSelonLeD�placement = (SeD�place() ? SE_D�PLACE : NE_SE_D�PLACE_PAS);
        }

        /// <summary>
        /// G�re le d�placement horizontal du vaisseau selon les touches A et D
        /// </summary>
        void G�rerClavier()
        {
            if (GestionInput.EstClavierActiv�)
            {
                int d�placementHorizontal = G�rerTouche(Keys.D) - G�rerTouche(Keys.A);
                AjusterPosition(d�placementHorizontal);
            }
        }

        /// <summary>
        /// Renvoit le nombre de pixels de d�placement si la touche est enfonc�e, sinon renvoit un z�ro
        /// </summary>
        /// <param name="touche">Touche enfonc�e</param>
        /// <returns>Nombre de pixels de d�placement ou z�ro</returns>
        int G�rerTouche(Keys touche)
        {
            return GestionInput.EstEnfonc�e(touche) ? NB_PIXELS_DE_D�PLACEMENT : 0;
        }

        /// <summary>
        /// Ajuste la propri�t� position selon le d�placement horizontal
        /// </summary>
        /// <param name="d�placementHorizontal">D�placement horizontal</param>
        void AjusterPosition(int d�placementHorizontal)
        {
            float posX = CalculerPosition(d�placementHorizontal, Position.X, MargeGauche, MargeDroite);

            Position = new Vector2(posX, Position.Y);
        }

        /// <summary>
        /// Calcul la position horizontale selon le changement
        /// </summary>
        /// <param name="d�placement">D�placement horizontal</param>
        /// <param name="posActuelle">Postion actuelle en </param>
        /// <param name="BorneMin"></param>
        /// <param name="BorneMax"></param>
        /// <returns></returns>
        float CalculerPosition(int d�placement, float posActuelle, int BorneMin, int BorneMax)
        {
            float position = posActuelle + d�placement;
            return MathHelper.Min(MathHelper.Max(BorneMin, position), BorneMax);
        }

        bool SeD�place()
        {
            return D�placementR�sultant != Vector2.Zero;
        }

        void LancerMissile()
        {
            int nbreDeMissiles = (Game.Components.Where(composant => composant is Missile && !((Missile)composant).AD�truire && ((Missile)composant).Visible).Count());

            if (nbreDeMissiles < NB_DE_MISSILES_MAX)
            {
                Missile missile = new Missile(Game,
                                                "Missile",
                                                new Vector2(RectangleDimensionsImage�L�chelle.X + RectangleDimensionsImage�L�chelle.Width / 2 - 4, RectangleDimensionsImage�L�chelle.Y - RectangleDimensionsImage�L�chelle.Height / 4),
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
