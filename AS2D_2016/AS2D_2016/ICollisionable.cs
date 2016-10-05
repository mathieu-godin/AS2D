/* Auteur :            Raphaël Brulé
   Fichier :           ICollisionable.cs
   Date :              le 05 octobre 2016
   Description :       Cette interface représente un objet collisionable.*/

namespace AS2D_2016
{
    interface ICollisionable
    {
        //À implanter
        bool EstEnCollision(object autreObjet);
    }
}
