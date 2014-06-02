using System.ComponentModel.DataAnnotations;

namespace Dominio
{
    public class Usuario
    {
        public int Id { get; private set; }
       
        [Required]
        public TipodeUsuario Tipo { get; set; } //Tipo de usuario Administrador/Miembro
        [Required]
        [MaxLength(20)]
        public string NombreUsuario { get; set; } //Es el enlace con el sistema de autentificacion de "Presentación".
        [Required]
        [MaxLength(50)]
        public string  Nombre { get; set; }//Nombre*: 50 caracteres
        [Required]
        [MaxLength(50)]
        public string Apellido { get; set;}//Apellido*: 50 caracteres
        [MaxLength(9)]
        public string DNI { get; set; }//DNI*: 9 dígitos
        [MaxLength(4)]
        public string Legajo { get; set; }//Legajo: 4 dígitos
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }//Email*: 50 caracters
        [MaxLength(20)]
        public string Telefono { get; set; }//Teléfono: 20 caracteres

        public Usuario()
        { }

        
        public Usuario (string nombreusuario, string nombre, string apellido, string dni, string legajo, string email, string telefono  )
        {
            this.NombreUsuario = nombreusuario;
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.DNI = dni;
            this.Legajo = legajo;
            this.Email = email;
            this.Telefono = telefono;
        }
 
    }
}
