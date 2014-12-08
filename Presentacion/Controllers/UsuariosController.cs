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
        // Repos
        private IUsuariosRepo UsuariosRepo;

        // Queries
        private IUsuariosQueriesTS UsuariosQueriesTS;

        // UoW
        private IUnitOfWorkFactory UowFactory;

        // Util
        private IValidadorDeUsuarios ValidadorDeUsuarios;
        private IConversorUsuario ConversorUsuario;

        //
        // GET: /Usuario/

        public UsuariosController(
            IUsuariosRepo usuariosRepo,
            IUnitOfWorkFactory uowFactory,
            IUsuariosQueriesTS usuariosQueriesTs,
            IValidadorDeUsuarios validadorDeUsuarios,
            IConversorUsuario conversorUsuario)
        {
            UsuariosRepo = usuariosRepo;
            UowFactory = uowFactory;
            UsuariosQueriesTS = usuariosQueriesTs;
            ValidadorDeUsuarios = validadorDeUsuarios;
            ConversorUsuario = conversorUsuario;
        }

        public ActionResult Index(ListaUsuariosVM busquedaVM)
        {
            busquedaVM.ListaUsuario = UsuariosQueriesTS.ListarUsuarios(
                busquedaVM.Nombre,
                busquedaVM.Apellido,
                busquedaVM.Legajo
                );

            return View(busquedaVM);
        }

        //
        // GET: /Usuario/Details/5

        public ActionResult Details(int id = 0)
        {
            var usuario = UsuariosRepo.ObtenerUsuario(id);

            if (usuario == null)
                return HttpNotFound();

            UsuarioVM usuarioVM = ConversorUsuario.CrearViewModel(usuario);

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
                if (ModelState.IsValid && ValidarUsuarioAuxiliar(usuarioVM))
                {
                    var usuario = ConversorUsuario.CrearUsuario(usuarioVM);

                    UsuariosRepo.Agregar(usuario);

                    uow.Commit();

                    return RedirectToAction("Index");
                }

                ModelStateHelper.CopyErrors(ValidadorDeUsuarios.ObtenerErrores(), ModelState);
                return View(usuarioVM);
            }
        }

        public bool ValidarUsuarioAuxiliar(UsuarioVM usuarioVM)
        {
            return ValidadorDeUsuarios.Validar(usuarioVM.NombreUsuario, usuarioVM.Email, usuarioVM.DNI, usuarioVM.Legajo);
        }

        //
        // GET: /Usuario/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var usuario = UsuariosRepo.ObtenerUsuario(id);

            if (usuario == null)
                return HttpNotFound();

            if (usuario.IsActive())
                return View(ConversorUsuario.CrearViewModel(usuario));

            return RedirectToAction("Edit");
        }

        //
        // POST: /Usuario/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioVM usuarioVM)
        {
            using (var uow = UowFactory.Actual)
            {
                if (CustomModelValidation() && 
                    ValidadorDeUsuarios.Validar(usuarioVM.NombreUsuario, usuarioVM.Email, usuarioVM.DNI, usuarioVM.Legajo, usuarioVM.id))
                {
                    var usuario = ConversorUsuario.ActualizarUsuario(usuarioVM);

                    UsuariosRepo.Actualizar(usuario);

                    uow.Commit();

                    return RedirectToAction("Index");
                }

                ModelStateHelper.CopyErrors(ValidadorDeUsuarios.ObtenerErrores(), ModelState);
                return View(usuarioVM);
            }
        }

        public bool CustomModelValidation()
        {
            return
                ModelState.IsValidField("Legajo") &&
                ModelState.IsValidField("Email") &&
                ModelState.IsValidField("DNI");
        }

        //bloquear usuario
        public ActionResult Lock(int id = 0)
        {
            using (var uow = UowFactory.Actual)
            {
                var usuario = UsuariosRepo.ObtenerUsuario(id);

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
            Usuario usuario = UsuariosRepo.ObtenerUsuario(id);

            if (usuario == null)
                return HttpNotFound();

            return View(ConversorUsuario.CrearViewModel(usuario));
        }

        //
        // POST: /Usuario/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var uow = UowFactory.Actual)
            {
                var usuario = UsuariosRepo.ObtenerUsuario(id);

                if (usuario == null)
                    return HttpNotFound();

                usuario.SetStateInactive();

                //codigo sacado de http://stackoverflow.com/questions/13391166/how-to-delete-a-simplemembership-user
                if (Roles.GetRolesForUser(usuario.NombreUsuario).Length > 0)
                    Roles.RemoveUserFromRoles(usuario.NombreUsuario, Roles.GetRolesForUser(usuario.NombreUsuario));

                // deletes record from webpages_Membership table
                ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(usuario.NombreUsuario);

                // deletes record from UserProfile table
                Membership.Provider.DeleteUser(usuario.NombreUsuario, true);

                UsuariosRepo.Actualizar(usuario);
                uow.Commit();

                return RedirectToAction("Index");
            }
        }

    }
}