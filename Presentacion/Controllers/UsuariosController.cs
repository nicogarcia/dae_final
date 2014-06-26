using AccesoDatos;
using Dominio;
using Dominio.Repos;
using Presentacion.Models;
using Presentacion.Filters;
using System.Data;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using System.Collections.Generic;

namespace Presentacion.Controllers
{
    [Autorizar(TipoDeUsuario.Administrador, EstadoUsuario.Activo)]
    public class UsuariosController : Controller
    {
        private ReservasContext db;
        private IUsuariosRepo ur;


        //
        // GET: /Usuario/

        public UsuariosController(ReservasContext db, IUsuariosRepo ur)
        {
            this.db = db;
            this.ur = ur;

        }

        public ActionResult Index(ListaUsuariosVM busquedaVM)
        {
            var listado = ur.ListarUsuarios(busquedaVM.Nombre, busquedaVM.Apellido, busquedaVM.Legajo);
            busquedaVM.ListaUsuario = listado;
            return View(busquedaVM);
        }

        //
        // GET: /Usuario/Details/5

        public ActionResult Details(int id = 0)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        //
        // GET: /Usuario/Create
        public ActionResult Create()
        {

            return View(new UsuarioVM());
        }

        //
        // POST: /Usuario/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioVM usuarioVM)
        {
            var validador = new ValidadorDeUsuarios(ur);
            if (ModelState.IsValid && CrearUsuarioController(usuarioVM, validador))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelStateHelper.CopyErrors(validador.Errores, ModelState);
                return View(usuarioVM);
            }

           
        } 

            
   
        //Valida y crea un usuario.
        private bool CrearUsuarioController (UsuarioVM usuarioVM, ValidadorDeUsuarios validador)
        {
            Usuario usuario = new Usuario(usuarioVM.NombreUsuario, usuarioVM.Nombre, usuarioVM.Apellido, usuarioVM.DNI, usuarioVM.Legajo, usuarioVM.Email, usuarioVM.Telefono, usuarioVM.Tipo);
            if (validador.Validar(usuario))
            {
                WebSecurity.CreateUserAndAccount(usuarioVM.NombreUsuario, usuarioVM.Password);
                this.ur.Agregar(usuario);
                // Le asocio el rol correspondiente.
                var roles = (SimpleRoleProvider)Roles.Provider;
                roles.AddUsersToRoles(new[] { usuarioVM.NombreUsuario }, new[] { usuarioVM.Tipo.ToString() });
                return true;
            }

            return false;


        }

        //
        // GET: /Usuario/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            if (usuario.EstadoUsuario ==EstadoUsuario.Activo)
                return View(usuario);
            else
                return RedirectToAction("Edit");
        }

        //
        // POST: /Usuario/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        //bloquear usuario
        public ActionResult Lock(int id = 0)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            if (usuario.EstadoUsuario == EstadoUsuario.Activo){
                usuario.EstadoUsuario = EstadoUsuario.Bloqueado;
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
            //return View(ur.Todos());
        }
        //bloquear usuario
        public ActionResult UnLock(int id = 0)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            if (usuario.EstadoUsuario == EstadoUsuario.Bloqueado)
            {
                usuario.EstadoUsuario = EstadoUsuario.Activo;
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        //
        // GET: /Usuario/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        //
        // POST: /Usuario/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            usuario.EstadoUsuario = EstadoUsuario.Inactivo;
            db.Entry(usuario).State = EntityState.Modified;
            //codigo sacado de http://stackoverflow.com/questions/13391166/how-to-delete-a-simplemembership-user
            if (Roles.GetRolesForUser(usuario.NombreUsuario).Length > 0)
            {
                Roles.RemoveUserFromRoles(usuario.NombreUsuario, Roles.GetRolesForUser(usuario.NombreUsuario));
            }
            ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(usuario.NombreUsuario); // deletes record from webpages_Membership table
            ((SimpleMembershipProvider)Membership.Provider).DeleteUser(usuario.NombreUsuario, true); // deletes record from UserProfile table

            //db.Usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}