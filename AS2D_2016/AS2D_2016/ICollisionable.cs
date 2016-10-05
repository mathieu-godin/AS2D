﻿/* Auteur :            Raphaël Brulé
   Fichier :           ICollisionable.cs
   Date :              le 05 octobre 2016
   Description :       Cette interface représente un objet collisionable.*/

namespace AtelierXNA
{
    interface ICollisionable
    {
        //À implanter
        bool EstEnCollision(object autreObjet);
    }
}
