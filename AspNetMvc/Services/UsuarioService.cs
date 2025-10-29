using MongoDB.Driver;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Helpers;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Services
{
    /// <summary>
    /// Servicio para gestionar usuarios
    /// </summary>
    public class UsuarioService
    {
        private readonly IMongoCollection<UsuarioRespiCare> _usuarios;

        public UsuarioService()
        {
            _usuarios = MongoDBHelper.GetCollection<UsuarioRespiCare>("usuarios");
        }

        /// <summary>
        /// Obtiene todos los usuarios
        /// </summary>
        public async Task<List<UsuarioRespiCare>> ObtenerTodos()
        {
            return await _usuarios.Find(_ => true).ToListAsync();
        }

        /// <summary>
        /// Obtiene un usuario por ID
        /// </summary>
        public async Task<UsuarioRespiCare> ObtenerPorId(string id)
        {
            return await _usuarios.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Obtiene un usuario por email
        /// </summary>
        public async Task<UsuarioRespiCare> ObtenerPorEmail(string email)
        {
            return await _usuarios.Find(u => u.Email == email.ToLower()).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        public async Task<UsuarioRespiCare> Crear(UsuarioRespiCare usuario)
        {
            // Validar que el email no exista
            var existente = await ObtenerPorEmail(usuario.Email);
            if (existente != null)
            {
                throw new Exception("El email ya está registrado");
            }

            // Encriptar contraseña
            usuario.Password = HashPassword(usuario.Password);
            usuario.Email = usuario.Email.ToLower();
            usuario.CreatedAt = DateTime.Now;
            usuario.UpdatedAt = DateTime.Now;

            await _usuarios.InsertOneAsync(usuario);
            return usuario;
        }

        /// <summary>
        /// Actualiza un usuario
        /// </summary>
        public async Task<bool> Actualizar(UsuarioRespiCare usuario)
        {
            usuario.UpdatedAt = DateTime.Now;
            var result = await _usuarios.ReplaceOneAsync(u => u.Id == usuario.Id, usuario);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Elimina un usuario (soft delete)
        /// </summary>
        public async Task<bool> Eliminar(string id)
        {
            var usuario = await ObtenerPorId(id);
            if (usuario == null) return false;

            usuario.IsActive = false;
            usuario.UpdatedAt = DateTime.Now;

            return await Actualizar(usuario);
        }

        /// <summary>
        /// Autentica un usuario
        /// </summary>
        public async Task<UsuarioRespiCare> Autenticar(string email, string password)
        {
            var usuario = await ObtenerPorEmail(email);
            if (usuario == null || !usuario.IsActive)
            {
                return null;
            }

            // Verificar contraseña
            if (!VerifyPassword(password, usuario.Password))
            {
                return null;
            }

            // Actualizar último login
            usuario.LastLogin = DateTime.Now;
            await Actualizar(usuario);

            return usuario;
        }

        /// <summary>
        /// Obtiene usuarios por rol
        /// </summary>
        public async Task<List<UsuarioRespiCare>> ObtenerPorRol(UserRole rol)
        {
            return await _usuarios.Find(u => u.Role == rol && u.IsActive).ToListAsync();
        }

        /// <summary>
        /// Obtiene estadísticas de usuarios
        /// </summary>
        public async Task<Dictionary<string, int>> ObtenerEstadisticas()
        {
            var usuarios = await ObtenerTodos();
            return new Dictionary<string, int>
            {
                { "Total", usuarios.Count },
                { "Activos", usuarios.Count(u => u.IsActive) },
                { "Pacientes", usuarios.Count(u => u.Role == UserRole.Patient) },
                { "Doctores", usuarios.Count(u => u.Role == UserRole.Doctor) },
                { "Administradores", usuarios.Count(u => u.Role == UserRole.Admin) }
            };
        }

        /// <summary>
        /// Cambia la contraseña de un usuario
        /// </summary>
        public async Task<bool> CambiarPassword(string id, string passwordActual, string passwordNuevo)
        {
            var usuario = await _usuarios.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (usuario == null || !VerifyPassword(passwordActual, usuario.Password))
            {
                return false;
            }

            usuario.Password = HashPassword(passwordNuevo);
            usuario.UpdatedAt = DateTime.Now;

            return await Actualizar(usuario);
        }

        /// <summary>
        /// Encripta una contraseña usando SHA256
        /// </summary>
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Verifica una contraseña contra su hash
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            string passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }
}

