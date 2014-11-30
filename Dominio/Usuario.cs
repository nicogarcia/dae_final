using System.ComponentModel.DataAnnotations;

namespace Dominio
{
    public class Usuario
    {
        public int Id { get; private set; }
       
        [Required]
        public TipoDeUsuario Tipo { get; set; } //Tipo de usuario Administrador/Miembro

        [Required]
        public EstadoUsuario EstadoUsuario { get; set; } //Estado de usuario

        [Required]
        [MaxLength(20)]
        public string NombreUsuario { get; set; } //Es el enlace con el sistema de autentificacion de "Presentación".

        [Required]
        [MaxLength(50)]
        public string  Nombre { get; set; }//Nombre*: 50 caracteres

        [Required]
        [MaxLength(50)]
        public string Apellido { get; set;}//Apellido*: 50 caracteres

        [Required]
        [MaxLength(9)]
        public string DNI { get; set; }//DNI*: 9 dígitos

        [MaxLength(4)]
        public string Legajo { get; set; }//Legajo: 4 dígitos

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }//Email*: 50 caracters

        [MaxLength(20)]
        public string Telefono { get; set; }//Teléfono: 20 caracteres

        public Usuario()
        { }
        
        public Usuario (string nombreusuario, string nombre, string apellido, string dni, string legajo, string email, string telefono, TipoDeUsuario tipo)
        {
            NombreUsuario = nombreusuario;
            Nombre = nombre;
            Apellido = apellido;
            DNI = dni;
            Legajo = legajo;
            Email = email;
            Telefono = telefono;
            Tipo = tipo;
            EstadoUsuario = EstadoUsuario.Activo;
        }

        public bool IsActive()
        {
            return EstadoUsuario == EstadoUsuario.Activo;
        }

        public bool IsLocked()
        {
            return EstadoUsuario == EstadoUsuario.Bloqueado;
        }

        public bool IsInactive()
        {
            return EstadoUsuario == EstadoUsuario.Inactivo;
        }
        
        public void SetStateActive()
        {
            EstadoUsuario = EstadoUsuario.Activo;
        }
        
        public void SetStateInactive()
        {
            EstadoUsuario = EstadoUsuario.Inactivo;
        }

        public void SetStateLocked()
        {
            EstadoUsuario = EstadoUsuario.Bloqueado;
        }

        public void ToggleState()
        {
            if (EstadoUsuario != EstadoUsuario.Inactivo)
            {
                EstadoUsuario = (EstadoUsuario == EstadoUsuario.Bloqueado) ? EstadoUsuario.Activo : EstadoUsuario.Bloqueado;
            }
        }
    }
}
