using System;
using UnityEngine;

// Görkem Küpeli Kütüphanesi'ne hoşgeldiniz!

namespace Core.ToolBox
{
    public static class Ext
    {
        public static int Sign(float sign)
        {
            //Mathf.Sign gibi, ama 0 da var
            if (sign > 0f) return 1;
            if (sign < 0f) return -1;
            return 0;
        }

        public static float Clamp(float value, float float1, float float2)
        {
            //Mathf.Clamp, ama hangi değerin min, hangisinin max olduğunu kendi buluyor
            return Mathf.Clamp(value, Mathf.Min(float1, float2), Mathf.Max(float1, float2));
        }

        public static Vector3 Lengthdir(float length, float angle)
        {
            // Belirli bir yöne belirli bir mesafe ilerlersen nasıl bir vector3 elde edersin?
            return Quaternion.Euler(0, 0, angle) * (Vector3.right * length);
        }

        public static void CircularMovement(this Rigidbody2D rb, Vector3 pos, Vector3 center, float moveSpeed)
        {
            // Rigidbody2D'ye çembersel hareket ekler
            var q = Quaternion.AngleAxis(moveSpeed, Vector3.forward);
            rb.MovePosition(q * (pos - center) + center);
        }

        public static float Format(float number, int digits)
        {
            // Diyelim ki 0,12345 var elimizde. Bunu kullanarak onu mesela 0,12 yapabiliriz. Ya da 0,123.
            var divide = 1;
            Fnd.Repeat(digits, () => divide = divide * 10);
            return Mathf.Round(number * divide) / divide;
        }

        public static float TargetAngle(Vector2 second, Vector2 first)
        {
            // İki vektör arasındaki açıyı döndürür. 
            return Vector2.SignedAngle(Vector2.right, second - first);
        }
    }

    public static class Fnd
    {
        public static void Repeat(int times, Action executeActions)
        {
            // Belirli bir aksiyonu "times" kadar tekrarlamayı sağlayan fonksiyon
            for (var i = 0; i < times; i++) executeActions();
        }
    }
}