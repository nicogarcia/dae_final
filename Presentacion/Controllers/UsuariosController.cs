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
    //[Autorizar(TipoDeUsuario.Administrador)]
    public class UsuariosController : Controller
    {
        private ReservasContext db;
        private IUsuariosRepo ur;

        //
        // GET: /Usuario/

        public UsuariosController()
        {
            db = new ReservasContext();
            ur = new UsuariosRepo(this.db);

        }

        public ActionResult Index()
        {

            return View(ur.ListarUsuarios("","Todos"));
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
        [Autorizar(TipoDeUsuario.Administrador)]
        public ActionResult Create()
        {

            return View(new UsuarioVM());
        }

        //
        // POST: /Usuario/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizar(TipoDeUsuario.Administrador)]
        
        public ActionResult Create(UsuarioVM usuarioVM)
        {
            ValidadorDeUsuarios validador = new ValidadorDeUsuarios(ur);

            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario(usuarioVM.NombreUsuario, usuarioVM.Nombre, usuarioVM.Apellido, usuarioVM.DNI, usuarioVM.Legajo, usuarioVM.Email, usuarioVM.Telefono, usuarioVM.Tipo);
                if (validador.Validar(usuario))
                {
                    WebSecurity.CreateUserAndAccount(usuarioVM.NombreUsuario, usuarioVM.Password);
                    
                    this.ur.AgregarUsuario(usuario);

                    // Creo el usuario en el mecanismo de seguridad 
                    //var membership = (SimpleMembershipProvider)Membership.Provider;
                    //membership.CreateUserAndAccount(usuarioVM.NombreUsuario, "123");

                    // Le asocio el rol correspondiente.
                    var roles = (SimpleRoleProvider)Roles.Provider;
                    roles.AddUsersToRoles(new[] { usuarioVM.NombreUsuario }, new[] { usuarioVM.Tipo.ToString() });
                  }
                else
                {
                    IDictionary<string, string> errores = validador.Errores;
                    ICollection<string> keys = errores.Keys;
                    string mensajedeerror;
                    foreach(string key in keys)
                    {
                        errores.TryGetValue(key, out mensajedeerror);
                        ModelState.AddModelError(key, mensajedeerror);
                    }
                    return View(usuarioVM);
                }
                
                return RedirectToAction("Index");
            }

            return View(usuarioVM);
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
            return View(usuario);
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
            db.Usuarios.Remove(usuario);
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