using Capsap.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando un usuario no tiene permisos
    /// </summary>
    public class PermisoDenegadoException : DomainException
    {
        public int UsuarioId { get; }
        public string Accion { get; }
        public RolUsuario RolUsuario { get; }

        public PermisoDenegadoException(int usuarioId, string accion)
            : base(
                $"El usuario {usuarioId} no tiene permisos para realizar la acción: {accion}",
                "PERMISO_DENEGADO")
        {
            UsuarioId = usuarioId;
            Accion = accion;
            AddData("UsuarioId", usuarioId);
            AddData("Accion", accion);
        }

        public PermisoDenegadoException(int usuarioId, RolUsuario rol, string accion)
            : base(
                $"El usuario {usuarioId} con rol '{rol}' no tiene permisos para realizar la acción: {accion}",
                "PERMISO_DENEGADO")
        {
            UsuarioId = usuarioId;
            RolUsuario = rol;
            Accion = accion;
            AddData("UsuarioId", usuarioId);
            AddData("RolUsuario", rol);
            AddData("Accion", accion);
        }
    }

}
