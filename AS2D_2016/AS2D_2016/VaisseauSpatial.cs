/* Auteur :            Rapha�l Brul�
   Fichier :           VaisseauSpatial.cs
   Date :              le 05 octobre 2016
   Description :       Ce component, enfant de SpriteAnim�, permet
                       de g�rer le vaisseau spatial.*/

// Co-auteur : Mathieu Godin
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtelierXNA
{
    /// <summary>
    /// Ce component, enfant de SpriteAnim�, permet de g�rer le vaisseau spatial
    /// </summary>
    public class VaisseauSpatial : SpriteAnim�
    {
        const int NE_SE_D�PLACE_PAS = 0, SE_D�PLACE = 1, NB_PIXELS_DE_D�PLACEMENT = 4, NB_DE_MISSILES_MAX = 3, HAUTEUR_MISSILE_MAX = 40, NB_DE_MISSILE_DANSLIMAGE = 25, ANIMATION_DE_BASE = 0, DEMI_LARGEUR_CANON_VAISSEAU = 4, UNIT�_ANIMATION = 1, LARGEUR_ANIMATION = 5, HAUTEUR_ANIMATION = 4, AUCUNE_ACC�L�RATION = 0;
        const string CHA�NE_IMAGE_MISSILE = "Missile", CHA�NE_IMAGE_EXPLOSION = "Explosion";
        const float FACTEUR_ACC�L�RATION = 1f / 600F, INTERVALLE_MIN = 0.01F, INTERVALLE_MAX = 1;

        float IntervalleMAJD�placement { get; set; }
        float Temps�coul�DepuisMAJ { get; set; }
        int AnimationSelonLeD�placement { get; set; }
        Vector2 AnciennePosition { get; set; }
        int Ordonn�eFinaleVaisseau { get; set; }
        bool EnDescente { get; set; }
        Vector2 VecteurD�placementDescente { get; set; }
        Vector2 D�placementR�sultant { get; set; }
        InputManager GestionInput { get; set; }
        Vector2 PositionSuppl�mentaireMissile { get; set; }

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
        public VaisseauSpatial(Game jeu, string nomImage, Vector2 position, Rectangle zoneAffichage, Vector2 descriptionImage, float intervalleMAJAnimation, float intervalleMAJD�placement) : base(jeu, nomImage, position, zoneAffichage, descriptionImage, intervalleMAJAnimation)
        {
            IntervalleMAJD�placement = intervalleMAJD�placement;
        }

        /// <summary>
        /// Initialise les propri�t�s du vaisseau spatial
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Temps�coul�DepuisMAJ = AUCUN_TEMPS_�COUL�;
            AnimationSelonLeD�placement = ANIMATION_DE_BASE; 
            Position = new Vector2(Position.X - DimensionsSprite�Afficher.X / DIVISEUR_OBTENTION_DEMI_GRANDEUR, Position.Y - DimensionsSprite�Afficher.Y / DIVISEUR_OBTENTION_DEMI_GRANDEUR);
            AnciennePosition = new Vector2(Position.X, Position.Y);
            Ordonn�eFinaleVaisseau = Game.Window.ClientBounds.Height - (int)DimensionsSprite�Afficher.Y; 
            EnDescente = true;
            VecteurD�placementDescente = new Vector2(AUCUN_D�PLACEMENT, NB_PIXELS_DE_D�PLACEMENT);
            D�placementR�sultant = Position - AnciennePosition;
            PositionSuppl�mentaireMissile = new Vector2(DimensionsSprite�Afficher.X / DIVISEUR_OBTENTION_DEMI_GRANDEUR - DEMI_LARGEUR_CANON_VAISSEAU, DimensionsSprite�Afficher.Y / DEMI_LARGEUR_CANON_VAISSEAU);
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
        /// M�thode qui met � jour le SpriteAnim� selon le temps �coul�
        /// </summary>
        protected override void EffectuerMise�JourAnimation()
        {
            RectangleSource = new Rectangle((RectangleSource.X + (int)Delta.X) % (int)DimensionsImage.X, (int)Delta.Y * AnimationSelonLeD�placement, (int)Delta.X, (int)Delta.Y);
        }

        /// <summary>
        /// M�thode update du composant vaisseau
        /// </summary>
        /// <param name="gameTime">Objet de classe GameTime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            V�rifierLancementMissile();
            Mettre�JourVaisseau(gameTime);
        }

        /// <summary>
        /// V�rifie si la touche Espace a �t� appuy�e afin de lancer un missile si c'est le cas
        /// </summary>
        void V�rifierLancementMissile()
        {
            if (GestionInput.EstNouvelleTouche(Keys.Space))
            {
                LancerMissile();
            }
        }

        /// <summary>
        /// G�re la structure de temps du vaisseau
        /// </summary>
        void Mettre�JourVaisseau(GameTime gameTime)
        {
            Temps�coul�DepuisMAJ += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Temps�coul�DepuisMAJ >= IntervalleMAJD�placement)
            {
                Temps�coul�DepuisMAJ = AUCUN_TEMPS_�COUL�;
                D�terminerSiVaisseauEnDescente();
                EffectuerMise�JourD�placement();
            }
        }

        /// <summary>
        /// D�termine si on est au d�but de la partie avec le vaisseau en descente pour aller le descendre si c'est le cas
        /// </summary>
        void D�terminerSiVaisseauEnDescente()
        {
            if (EnDescente)
            {
                G�rerDescenteDuVaisseau();
            }
        }

        /// <summary>
        /// G�re la descente du vaisseau au d�but de la partie
        /// </summary>
        void G�rerDescenteDuVaisseau()
        {
            Position += VecteurD�placementDescente;
            CalculerRectangleImage�Afficher();
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
            CalculerRectangleImage�Afficher();
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
                G�rerAcc�l�ration();
                V�rifierSiAjustementPositionN�cessaire(d�placementHorizontal);
            }
        }

        /// <summary>
        /// V�rifie si on a boug� et si on doit ainsi ajuster la Position du VaisseauSpatial
        /// </summary>
        /// <param name="d�placementHorizontal">D�placement horizontal effectu� par le VaisseauSpatial</param>
        void V�rifierSiAjustementPositionN�cessaire(int d�placementHorizontal)
        {
            if (d�placementHorizontal != AUCUN_D�PLACEMENT)
            {
                AjusterPosition(d�placementHorizontal);
            }
        }

        void G�rerAcc�l�ration()
        {
            int modificateurAcc�l�ration = G�rerTouche(Keys.PageDown) - G�rerTouche(Keys.PageUp);
            if (modificateurAcc�l�ration != AUCUNE_ACC�L�RATION)
            {
                IntervalleMAJD�placement += modificateurAcc�l�ration * FACTEUR_ACC�L�RATION;
                IntervalleMAJD�placement = MathHelper.Max(MathHelper.Min(IntervalleMAJD�placement, INTERVALLE_MAX), INTERVALLE_MIN);
            }
        }

        /// <summary>
        /// Renvoit le nombre de pixels de d�placement si la touche est enfonc�e, sinon renvoit un z�ro
        /// </summary>
        /// <param name="touche">Touche enfonc�e</param>
        /// <returns>Nombre de pixels de d�placement ou z�ro</returns>
        int G�rerTouche(Keys touche)
        {
            return GestionInput.EstEnfonc�e(touche) ? NB_PIXELS_DE_D�PLACEMENT : AUCUN_D�PLACEMENT;
        }

        /// <summary>
        /// Ajuste la propri�t� position selon le d�placement horizontal
        /// </summary>
        /// <param name="d�placementHorizontal">D�placement horizontal</param>
        void AjusterPosition(int d�placementHorizontal)
        {
            float abscisse = CalculerPosition(d�placementHorizontal, Position.X, MargeGauche, MargeDroite);

            Position = new Vector2(abscisse, Position.Y);
        }

        /// <summary>
        /// Calcul la position horizontale selon le changement
        /// </summary>
        /// <param name="d�placement">D�placement horizontal</param>
        /// <param name="posActuelle">Postion actuelle en </param>
        /// <param name="BorneMin">Borne minimale de la position</param>
        /// <param name="BorneMax">Borne maximale de la position</param>
        /// <returns>La position horizontale</returns>
        float CalculerPosition(int d�placement, float posActuelle, int BorneMin, int BorneMax)
        {
            float position = posActuelle + d�placement;
            return MathHelper.Min(MathHelper.Max(BorneMin, position), BorneMax);
        }

        /// <summary>
        /// V�rifie si le vaisseau se d�place
        /// </summary>
        /// <returns>Vrai ou faux</returns>
        bool SeD�place()
        {
            return D�placementR�sultant != Vector2.Zero;
        }

        /// <summary>
        /// S'occuppe de lancer les missiles selon un maximum d�fini
        /// </summary>
        void LancerMissile()
        {
            int nbreDeMissiles = (Game.Components.Where(composant => composant is Missile && !((Missile)composant).AD�truire && ((Missile)composant).Visible).Count());

            if (nbreDeMissiles < NB_DE_MISSILES_MAX)
            {
                Missile missile = new Missile(Game, CHA�NE_IMAGE_MISSILE, new Vector2(Position.X + PositionSuppl�mentaireMissile.X, Position.Y - PositionSuppl�mentaireMissile.Y), new Rectangle(ABSCISSE_NULLE, ORDONN�E_NULLE, HAUTEUR_MISSILE_MAX, HAUTEUR_MISSILE_MAX), new Vector2(NB_DE_MISSILE_DANSLIMAGE, UNIT�_ANIMATION), CHA�NE_IMAGE_EXPLOSION, new Vector2(LARGEUR_ANIMATION, HAUTEUR_ANIMATION), IntervalleMAJAnnimation, IntervalleMAJD�placement);
                Game.Components.Add(missile);
            }
        }
    }
}
