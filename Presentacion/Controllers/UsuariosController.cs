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
using Presentacion.Soporte;
using Dominio.UnitOfWork;

namespace Presentacion.Controllers
{
    [Autorizar(TipoDeUsuario.Administrador)]
    public class UsuariosController : Controller
    {
        private ReservasContext db;
        private IUsuariosRepo ur;
        private IUnitOfWorkFactory uowFactory;


        //
        // GET: /Usuario/

        public UsuariosController(ReservasContext db, IUsuariosRepo ur, IUnitOfWorkFactory uowFactory)
        {
            this.db = db;
            this.ur = ur;
            this.uowFactory = uowFactory;
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
            Usuario u = ur.getUsuario(id);
            if(u== null)
                return HttpNotFound();
            UsuarioVM usuario = ConversorUsuarioUsuarioVM.getInstance(u);
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
            using (var uow = this.uowFactory.Actual)
            { 
                var validador = new ValidadorDeUsuarios(ur);
                if (ModelState.IsValid && CrearUsuarioController(usuarioVM, validador))
            {
                    uow.Commit();
                return RedirectToAction("Index");
            }
            else
            {
                ModelStateHelper.CopyErrors(validador.Errores, ModelState);
                return View(usuarioVM);
            }
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
            Usuario u = ur.getUsuario(id);
            if (u == null)
                return HttpNotFound();
            if (u.isActive())
                return View(ConversorUsuarioUsuarioVM.getInstance(u));
            else
                return RedirectToAction("Edit");
            
        }

        //
        // POST: /Usuario/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioVM usuario){
            using (var uow = this.uowFactory.Actual){
                ValidadorDeUsuarios val = new ValidadorDeUsuarios(ur);
                Usuario user = ConversorUsuarioUsuarioVM.getInstance(new Usuario(),usuario);
                if (val.validarUsuario(user,usuario.id))
                {
                    user = ur.getUsuario(usuario.id);
                    var roles = (SimpleRoleProvider)Roles.Provider;
                    if (usuario.Tipo != user.Tipo)
                    {
                        roles.RemoveUsersFromRoles(new[] { user.NombreUsuario }, new []{user.Tipo.ToString()});
                        roles.AddUsersToRoles(new[] { user.NombreUsuario }, new[] { usuario.Tipo.ToString() });
                    }
                    user = ConversorUsuarioUsuarioVM.getInstance(user, usuario);
                    ur.Actualizar(user);
                    uow.Commit();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelStateHelper.CopyErrors(val.Errores, ModelState);
                    return View(usuario);
                }
            }
        }

        
        //bloquear usuario
        public ActionResult Lock(int id = 0)
        {
            using (var uow = this.uowFactory.Actual)
            {
                Usuario usuario = ur.getUsuario(id);
                if (usuario == null)
                    return HttpNotFound();
                if (usuario.isActive())
                {
                    usuario.setStateLocked();
                    ur.Actualizar(usuario);
                    uow.Commit();
                }
                return RedirectToAction("Index");
            }
        }
        //bloquear usuario
        public ActionResult UnLock(int id = 0)
        {
            using (var uow = this.uowFactory.Actual)
            {
                Usuario usuario = ur.getUsuario(id);
                if (usuario == null)
                    return HttpNotFound();
                if (usuario.isLocked())
                {
                    usuario.setStateActive();
                    ur.Actualizar(usuario);
                    uow.Commit();
                }
                return RedirectToAction("Index");
            }
        }
        //
        // GET: /Usuario/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Usuario usuario = ur.getUsuario(id);
            if (usuario == null)
                return HttpNotFound();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(ConversorUsuarioUsuarioVM.getInstance(usuario));
        }

        //
        // POST: /Usuario/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var uow = this.uowFactory.Actual)
            {
                Usuario usuario = ur.getUsuario(id);

                if (usuario == null)
                    return HttpNotFound();
                usuario.setStateInactive();
                //codigo sacado de http://stackoverflow.com/questions/13391166/how-to-delete-a-simplemembership-user
                if (Roles.GetRolesForUser(usuario.NombreUsuario).Length > 0)
                {
                    Roles.RemoveUserFromRoles(usuario.NombreUsuario, Roles.GetRolesForUser(usuario.NombreUsuario));
                }
                ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(usuario.NombreUsuario); // deletes record from webpages_Membership table
                ((SimpleMembershipProvider)Membership.Provider).DeleteUser(usuario.NombreUsuario, true); // deletes record from UserProfile table

                //db.Usuarios.Remove(usuario);
                ur.Actualizar(usuario);
                uow.Commit();
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}