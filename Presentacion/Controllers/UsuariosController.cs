using Dominio;
using Dominio.Entidades;
using Dominio.Queries;
using Dominio.Repos;
using Dominio.Validacion;
using Presentacion.Models;
using Presentacion.Filters;
using System.Web.Mvc;
using System.Web.Security;
using Presentacion.Models.Conversores;
using WebMatrix.WebData;
using Presentacion.Soporte;
using Dominio.UnitOfWork;

namespace Presentacion.Controllers
{
    [Autorizar(TipoDeUsuario.Administrador)]
    public class UsuariosController : Controller
    {
        private IUsuariosRepo UsuariosRepo;
        private IUnitOfWorkFactory UowFactory;
        private IUsuariosQueriesTS UsuariosQueriesTS;

        //
        // GET: /Usuario/

        public UsuariosController(
            IUsuariosRepo usuariosRepo, 
            IUnitOfWorkFactory uowFactory,
            IUsuariosQueriesTS usuariosQueriesTs)
        {
            UsuariosRepo = usuariosRepo;
            UowFactory = uowFactory;
            UsuariosQueriesTS = usuariosQueriesTs;
        }

        public ActionResult Index(ListaUsuariosVM busquedaVM)
        {
            var listado = UsuariosQueriesTS.ListarUsuarios(busquedaVM.Nombre, busquedaVM.Apellido, busquedaVM.Legajo);
            busquedaVM.ListaUsuario = listado;
            return View(busquedaVM);
        }

        //
        // GET: /Usuario/Details/5

        public ActionResult Details(int id = 0)
        {
            var usuario = UsuariosRepo.getUsuario(id);

            if (usuario == null)
                return HttpNotFound();

            UsuarioVM usuarioVM = ConversorUsuario.getInstance(usuario);
            return View(usuarioVM);
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
            using (var uow = UowFactory.Actual)
            {
                var validador = new ValidadorDeUsuarios(UsuariosRepo);

                if (ModelState.IsValid && CrearUsuarioController(usuarioVM, validador))
                {
                    uow.Commit();
                    return RedirectToAction("Index");
                }

                ModelStateHelper.CopyErrors(validador.Errores, ModelState);
                return View(usuarioVM);
            }
        }

        //Valida y crea un usuario.
        private bool CrearUsuarioController(UsuarioVM usuarioVM, ValidadorDeUsuarios validador)
        {
            var usuario = new Usuario(usuarioVM.NombreUsuario, usuarioVM.Nombre, usuarioVM.Apellido, usuarioVM.DNI, usuarioVM.Legajo, usuarioVM.Email, usuarioVM.Telefono, usuarioVM.Tipo);
            if (validador.Validar(usuario))
            {
                WebSecurity.CreateUserAndAccount(usuarioVM.NombreUsuario, usuarioVM.Password);

                UsuariosRepo.Agregar(usuario);

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
            var usuario = UsuariosRepo.getUsuario(id);

            if (usuario == null)
                return HttpNotFound();

            if (usuario.IsActive())
                return View(ConversorUsuario.getInstance(usuario));

            return RedirectToAction("Edit");
        }

        //
        // POST: /Usuario/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioVM usuario)
        {
            using (var uow = UowFactory.Actual)
            {
                var validador = new ValidadorDeUsuarios(UsuariosRepo);
                Usuario user = ConversorUsuario.getInstance(new Usuario(), usuario);
                if (validador.validarUsuario(user, usuario.id))
                {
                    user = UsuariosRepo.getUsuario(usuario.id);
                    var roles = (SimpleRoleProvider)Roles.Provider;
                    if (usuario.Tipo != user.Tipo)
                    {
                        roles.RemoveUsersFromRoles(new[] { user.NombreUsuario }, new[] { user.Tipo.ToString() });
                        roles.AddUsersToRoles(new[] { user.NombreUsuario }, new[] { usuario.Tipo.ToString() });
                    }
                    user = ConversorUsuario.getInstance(user, usuario);
                    UsuariosRepo.Actualizar(user);
                    uow.Commit();
                    return RedirectToAction("Index");
                }
                
                ModelStateHelper.CopyErrors(validador.Errores, ModelState);
                return View(usuario);
            }
        }

        //bloquear usuario
        public ActionResult Lock(int id = 0)
        {
            using (var uow = UowFactory.Actual)
            {
                var usuario = UsuariosRepo.getUsuario(id);

                if (usuario == null)
                    return HttpNotFound();

                if (usuario.IsInactive())
                    return RedirectToAction("Index");

                usuario.ToggleState();
                UsuariosRepo.Actualizar(usuario);
                uow.Commit();

                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Usuario/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Usuario usuario = UsuariosRepo.getUsuario(id);

            if (usuario == null)
                return HttpNotFound();

            return View(ConversorUsuario.getInstance(usuario));
        }

        //
        // POST: /Usuario/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var uow = UowFactory.Actual)
            {
                var usuario = UsuariosRepo.getUsuario(id);

                if (usuario == null)
                    return HttpNotFound();

                usuario.SetStateInactive();
                
                //codigo sacado de http://stackoverflow.com/questions/13391166/how-to-delete-a-simplemembership-user
                if (Roles.GetRolesForUser(usuario.NombreUsuario).Length > 0)
                    Roles.RemoveUserFromRoles(usuario.NombreUsuario, Roles.GetRolesForUser(usuario.NombreUsuario));

                // deletes record from webpages_Membership table
                ((SimpleMembershipProvider) Membership.Provider).DeleteAccount(usuario.NombreUsuario);

                // deletes record from UserProfile table
                Membership.Provider.DeleteUser(usuario.NombreUsuario, true);

                UsuariosRepo.Actualizar(usuario);
                uow.Commit();

                return RedirectToAction("Index");
            }
        }

    }
}