using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    // ==========================================
    // EXCEPCIÓN BASE DEL DOMINIO
    // ==========================================
    /// <summary>
    /// Excepción base para todas las excepciones del dominio
    /// </summary>
    public class DomainException : Exception
    {
        public string ErrorCode { get; protected set; }
        public Dictionary<string, object> AdditionalData { get; protected set; }

        public DomainException()
        {
            AdditionalData = new Dictionary<string, object>();
        }

        public DomainException(string message) : base(message)
        {
            AdditionalData = new Dictionary<string, object>();
        }

        public DomainException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
            AdditionalData = new Dictionary<string, object>();
        }

        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
            AdditionalData = new Dictionary<string, object>();
        }

        public DomainException(string message, string errorCode, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
            AdditionalData = new Dictionary<string, object>();
        }

        /// <summary>
        /// Agrega datos adicionales a la excepción
        /// </summary>
        public void AddData(string key, object value)
        {
            AdditionalData[key] = value;
        }
    }

}
