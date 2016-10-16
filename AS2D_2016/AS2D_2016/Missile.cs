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
        const float INTERVALLE_ANIMATION_LENT = 6 * Atelier.INTERVALLE_STANDARDS;
        //Propri�t� initialement g�r�e par le constructeur
        float IntervalleMAJD�placement { get; set; }
        string NomImageExplosion { get; set; }
        Vector2 DescriptionImageExplosion { get; set; }
        Texture2D ImageExplosion { get; set; }
        float Temps�coul�DepuisMAJD�placement { get; set; }
        public bool ExplosionActiv�e { get; private set; }
        float Temps�coul�DepuisMAJExplosion { get; set; }
        int PhaseExplosion { get; set; }
        SpriteAnim� Explosion { get; set; }
        //bool ExplosionTermin�e { get; set; }


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
                EffectuerMise�JourD�placement(gameTime);
                Temps�coul�DepuisMAJD�placement = AUCUN_TEMPS_�COUL�;
            }
        }

        /// <summary>
        /// M�thode qui met � jour le d�placement du Missile selon le temps �coul�
        /// </summary>
        protected virtual void EffectuerMise�JourD�placement(GameTime gameTime)
        {
            Position -= Vector2.UnitY;
            if (Position.Y <= MargeHaut && !ExplosionActiv�e /*&& !ExplosionTermin�e*/)
            {
                ActiverExplosionMissile();
                G�rerExplosion(gameTime);
            }
            /*if (ExplosionTermin�e)
            {
                for (int i = Game.Components.Count - 1; i >= 0; --i)
                {
                    if (Game.Components[i] is IDestructible && ((IDestructible)Game.Components[i]).AD�truire)
                    {
                        Game.Components.RemoveAt(i);
                    }
                }
            }*/
        }

        /// <summary>
        /// Appel� quand le missile doit �tre explos�
        /// </summary>
        public void ActiverExplosion()
        {
            
        }

        /// <summary>
        /// Active l'explosion du missile
        /// </summary>
        void ActiverExplosionMissile()
        {
            Explosion = new SpriteAnim�(Game, "Explosion", Position, ZoneAffichage, DescriptionImageExplosion, INTERVALLE_ANIMATION_LENT);
            Game.Components.Add(Explosion);
            ExplosionActiv�e = true;
            Temps�coul�DepuisMAJExplosion = AUCUN_TEMPS_�COUL�;
        }

        /// <summary>
        /// G�re l'explosion du missile
        /// </summary>
        /// <param name="gameTime">Contient les informations de temps de jeu</param>
        private void G�rerExplosion(GameTime gameTime)
        {
            float Temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJExplosion += Temps�coul�;
            if (Temps�coul�DepuisMAJExplosion >= INTERVALLE_ANIMATION_LENT)
            {
                ++PhaseExplosion;
                Temps�coul�DepuisMAJExplosion = AUCUN_TEMPS_�COUL�;
                if (PhaseExplosion >= DescriptionImageExplosion.X * DescriptionImageExplosion.Y)
                {
                    ExplosionActiv�e = false;
                    Explosion.AD�truire = true;
                    //ExplosionTermin�e = true;
                }
            }
        }
    }
}