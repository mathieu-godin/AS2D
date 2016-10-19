/*
Missile.cs
----------

Par Mathieu Godin

R�le : Composant qui h�rite de SpriteAnim�
       qui g�re le missile pouvant se d�placer
       vers le haut en acc�l�rant et son explosion

Cr�� : 5 octobre 2016
Modifi� : 15 octobre 2016
*/
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{
    /// <summary>
    /// Composant qui h�rite de SpriteAnim�
    /// </summary>
    public class Missile : SpriteAnim�
    {
        const float INTERVALLE_ANIMATION_LENT = 6 * Atelier.INTERVALLE_STANDARDS, CHANGEMENT_INTERVALLE_POUR_ACC�L�RATION = 0.00015F, D�PLACEMENT_ORDONN�E_MAJ = 4.0F;
        const int AVANT_PREMI�RE_PHASE_EXPLOSION = 0, DIMENSION_EXPLOSION = 40;
        //Propri�t� initialement g�r�e par le constructeur
        float IntervalleMAJD�placement { get; set; }
        string NomImageExplosion { get; set; }
        Vector2 DescriptionImageExplosion { get; set; }
        Texture2D ImageExplosion { get; set; }
        float Temps�coul�DepuisMAJD�placement { get; set; }
        public bool ExplosionActiv�e { get; private set; }
        float Temps�coul�DepuisMAJExplosion { get; set; }
        int PhaseExplosion { get; set; }
        public SpriteAnim� Explosion { get; private set; }
        Vector2 VecteurD�placementMAJ { get; set; }
        bool Collision { get; set; }
        Rectangle ZoneExplosion { get; set; }

        /// <summary>
        /// Constructeur de Sph�re
        /// </summary>
        /// <param name="jeu">Objet de classe Game</param>
        /// <param name="nomImage">Nom de l'image (string)</param>
        /// <param name="position">Position (Vector2)</param>
        /// <param name="zoneAffichage">Zone d'affichage (Rectangle)</param>
        /// <param name="descriptionImage">Description de l'image (Vector2)</param>
        /// <param name="intervalleMAJAnimation">Intervalle de mise � jour de l'animation (float)</param>
        /// <param name="intervalleMAJD�placement">Intervalle de mise � jour du d�placement (float)</param>
        public Missile(Game jeu, string nomImageMissile, Vector2 position, Rectangle zoneAffichage, Vector2 descriptionImageMissile, string nomImageExplosion, Vector2 descriptionImageExplosion, float intervalleMAJAnimation, float intervalleMAJD�placement) : base(jeu, nomImageMissile, position, zoneAffichage, descriptionImageMissile, intervalleMAJAnimation)
        {
            IntervalleMAJD�placement = intervalleMAJD�placement;
            NomImageExplosion = nomImageExplosion;
            DescriptionImageExplosion = descriptionImageExplosion;
        }

        /// <summary>
        /// Initialise les composants de Missile
        /// </summary>
        public override void Initialize()
        {
            LoadContent();
            base.Initialize();
            Temps�coul�DepuisMAJD�placement = AUCUN_TEMPS_�COUL�;
            ExplosionActiv�e = false;
            //ExplosionTermin�e = false;
            VecteurD�placementMAJ = new Vector2(ABSCISSE_NULLE, D�PLACEMENT_ORDONN�E_MAJ);
            Collision = false;
            ZoneExplosion = new Rectangle(ABSCISSE_NULLE, ORDONN�E_NULLE, DIMENSION_EXPLOSION, DIMENSION_EXPLOSION);
        }

        /// <summary>
        /// Charge le contenu n�cessaire au fonctionnement du Missile
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            ImageExplosion = GestionnaireDeTextures.Find(NomImageExplosion);
        }

        /// <summary>
        /// Met � jour le Missile
        /// </summary>
        /// <param name="gameTime">Contient les informations de temps</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Temps�coul�DepuisMAJD�placement += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Temps�coul�DepuisMAJD�placement >= IntervalleMAJD�placement)
            {
                EffectuerMise�JourD�placement();
                Temps�coul�DepuisMAJD�placement = AUCUN_TEMPS_�COUL�;
            }
            if (ExplosionActiv�e)
            {
                G�rerExplosion(gameTime);
            }
            if (Collision)
            {
                Collision = false;
                Game.Components.Add(Explosion);
            }
        }

        /// <summary>
        /// M�thode qui met � jour le d�placement du Missile selon le temps �coul�
        /// </summary>
        protected virtual void EffectuerMise�JourD�placement()
        {
            Position -= VecteurD�placementMAJ;
            IntervalleMAJD�placement -= CHANGEMENT_INTERVALLE_POUR_ACC�L�RATION;
            if (Position.Y <= MargeHaut && !ExplosionActiv�e)
            {
                ActiverExplosion();
                //G�rerExplosion(gameTime);
                //AD�truire = true;
            }
        }

        

        /// <summary>
        /// Appel� quand le missile doit �tre explos�
        /// </summary>
        public void ActiverExplosion()
        {
            //AD�truire = true;//LIGNE ESSENTIELLE!!!
            Visible = false;
            Explosion = new SpriteAnim�(Game, "Explosion", Position, ZoneExplosion, DescriptionImageExplosion, INTERVALLE_ANIMATION_LENT);
            ExplosionActiv�e = true;
            Temps�coul�DepuisMAJExplosion = AUCUN_TEMPS_�COUL�;
            PhaseExplosion = AVANT_PREMI�RE_PHASE_EXPLOSION;
            Collision = true;
        }

        /// <summary>
        /// G�re l'explosion du missile
        /// </summary>
        /// <param name="gameTime">Contient les informations de temps de jeu</param>
        void G�rerExplosion(GameTime gameTime)
        {
            Temps�coul�DepuisMAJExplosion += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Temps�coul�DepuisMAJExplosion >= INTERVALLE_ANIMATION_LENT)
            {
                ++PhaseExplosion;
                Temps�coul�DepuisMAJExplosion = AUCUN_TEMPS_�COUL�;
                if (PhaseExplosion >= DescriptionImageExplosion.X * DescriptionImageExplosion.Y)
                {
                    ExplosionActiv�e = false;
                    Explosion.AD�truire = true;
                    Game.Components.Remove(Explosion);
                    AD�truire = true;
                }
            }
        }
    }
}