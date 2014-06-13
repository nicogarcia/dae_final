using AccesoDatos;
using Dominio;
using Presentacion.Filters;
using System.Data;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Presentacion.Controllers
{
    [Autorizar(TipoDeUsuario.Administrador)]
    public class UsuariosController : Controller
    {
        private ReservasContext db;
        private UsuariosRepo ur;

        //
        // GET: /Usuario/

        public UsuariosController()
        {
            db = new ReservasContext();
            ur = new UsuariosRepo(this.db);

        }

        public ActionResult Index()
        {

            return View(ur.Todos());
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

            return View();
        }

        //
        // POST: /Usuario/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                WebSecurity.CreateUserAndAccount(usuario.NombreUsuario, usuario.Password);
                this.ur.AgregarUsuario(usuario);

                // Creo el usuario en el mecanismo de seguridad 
                var membership = (SimpleMembershipProvider)Membership.Provider;
                membership.CreateUserAndAccount(usuario.NombreUsuario, "123");
                
                // Le asocio el rol correspondiente.
                var roles = (SimpleRoleProvider)Roles.Provider;
                roles.AddUsersToRoles(new[] { usuario.NombreUsuario }, new[] { usuario.Tipo.ToString() });
                
                return RedirectToAction("Index");
            }

            return View(usuario);
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