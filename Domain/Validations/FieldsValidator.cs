using System;
using System.Net.Mail;

namespace Domain.Validations {
    public static class FieldsValidator {
        /// <summary>
        /// Valida un texto verificando que no sea nulo, vacío o compuesto solo por espacios,
        /// y que su longitud esté dentro del rango permitido.
        /// </summary>
        /// <param name="text">
        /// Texto que se desea validar.
        /// </param>
        /// <param name="property">
        /// Nombre de la propiedad o campo que se utilizará en los mensajes de error.
        /// </param>
        /// <param name="minLength">
        /// Longitud mínima permitida para el texto.
        /// </param>
        /// <param name="maxLength">
        /// Longitud máxima permitida para el texto.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Se lanza cuando el texto está vacío, es menor a la longitud mínima
        /// o supera la longitud máxima permitida.
        /// </exception>
        public static void ValidateText(string text, string property, int minLength, int maxLength) {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException($"{property} no puede estar vacio", nameof(text));

            if (text.Trim().Length < minLength)
                throw new ArgumentException($"{property} no puede tener menos de {minLength} caracteres");

            if (text.Trim().Length >= maxLength)
                throw new ArgumentException($"{property} no puede superar los {maxLength} caracteres");
        }

        /// <summary>
        /// Valida que un correo electrónico no esté vacío, no exceda
        /// el límite de caracteres permitido y tenga un formato válido.
        /// </summary>
        /// <param name="email">
        /// Dirección de correo electrónico a validar.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Se lanza cuando el correo está vacío, supera la longitud máxima
        /// o no tiene un formato válido.
        /// </exception>
        public static void ValidateEmail(string email) {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El correo no puede estar vacío", nameof(email));

            if (email.Length > 100)
                throw new ArgumentException("El correo no puede superar los 100 caracteres");

            try {
                var addr = new MailAddress(email);
            } catch {
                throw new ArgumentException("El correo no tiene un formato válido");
            }
        }
        /// <summary>
        /// Valida que un valor numérico esté dentro del rango permitido.
        /// </summary>
        /// <param name="value">Valor a validar.</param>
        /// <param name="property">Nombre del campo o propiedad.</param>
        /// <param name="min">Valor mínimo permitido.</param>
        /// <param name="max">Valor máximo permitido.</param>
        /// <exception cref="ArgumentException">
        /// Se lanza cuando el valor está fuera del rango permitido.
        /// </exception>
        public static void ValidateNumber(decimal value, string property, decimal min, decimal? max = null) {
            if (value < min) {
                throw new ArgumentException($"{property} no puede ser menor a {min}");
            }

            if (max.HasValue && value > max.Value) {
                throw new ArgumentException($"{property} no puede ser mayor a {max.Value}");
            }
        }
        /// <summary>
        /// Valida una colección verificando que no sea nula
        /// y que la cantidad de elementos esté dentro
        /// del rango permitido.
        /// </summary>
        /// <typeparam name="T">
        /// Tipo de elemento contenido en la colección.
        /// </typeparam>
        /// <param name="collection">
        /// Colección a validar.
        /// </param>
        /// <param name="property">
        /// Nombre de la propiedad o campo que se utilizará
        /// en los mensajes de error.
        /// </param>
        /// <param name="min">
        /// Cantidad mínima permitida de elementos.
        /// </param>
        /// <param name="max">
        /// Cantidad máxima permitida de elementos.
        /// Si es null, no se aplicará límite máximo.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Se lanza cuando la colección es nula,
        /// tiene menos elementos que el mínimo permitido
        /// o supera el máximo permitido.
        /// </exception>
        public static void ValidateCollection<T>(ICollection<T> collection, string property, int min, int? max = null) {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection), $"{property} no puede ser nulo");

            if (min < 0)
                throw new ArgumentException("El mínimo no puede ser negativo");

            if (max is not null && min > max)
                throw new ArgumentException("El mínimo no puede ser mayor al máximo");

            if (collection.Count < min)
                throw new ArgumentException($"{property} debe tener al menos {min} elementos");

            if (max is not null && collection.Count > max)
                throw new ArgumentException($"{property} no puede superar {max} elementos");
        }
    }
}