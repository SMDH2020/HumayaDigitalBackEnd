using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HD.Generales.MCP.Services
{
    public class McpDataService
    {
        private readonly string _conn;

        public McpDataService(IConfiguration config)
        {
            _conn = config.GetConnectionString("Servicio");
        }

        public McpUsuarioDto BuscarPorEmail(string email)
        {
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("HumayaDigital_Usuarios..sp_McpUsuarios_BuscarPorEmail", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email is null ? "" : email);
            conn.Open();
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;
            return new McpUsuarioDto
            {
                McpUsuarioId = r.GetGuid(r.GetOrdinal("McpUsuarioId")),
                Nombre = r["Nombre"].ToString(),
                Email = r["Email"].ToString(),
                PasswordHash = r["PasswordHash"].ToString()
            };
        }

        //public void GuardarCodigoMfa(Guid usuarioId, string codigo)
        //{
        //    using var conn = new SqlConnection(_conn);
        //    using var cmd = new SqlCommand("HumayaDigital_Usuarios..sp_McpMfaCodigos_Guardar", conn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@CodigoId", Guid.NewGuid());
        //    cmd.Parameters.AddWithValue("@McpUsuarioId", usuarioId);
        //    cmd.Parameters.AddWithValue("@Codigo", codigo);
        //    conn.Open();
        //    cmd.ExecuteNonQuery();
        //}
        public McpSesionMfaDto BuscarSesionMfa(Guid sesionId)
        {
            using var conn = new SqlConnection(_conn);
            return conn.QueryFirstOrDefault<McpSesionMfaDto>(
                "HumayaDigital_Usuarios..sp_McpMfaCodigos_BuscarSesion",
                new { CodigoId = sesionId },
                commandType: CommandType.StoredProcedure);
        }
        public void GuardarMfaCodigo(Guid codigoId, Guid mcpUsuarioId, string codigo,
                              string redirectUri, string state)
        {
            using var conn = new SqlConnection(_conn);
            conn.Execute("HumayaDigital_Usuarios..sp_McpMfaCodigos_Guardar", new
            {
                CodigoId = codigoId,
                McpUsuarioId = mcpUsuarioId,
                Codigo = codigo,
                RedirectUri = redirectUri,
                State = state
            }, commandType: CommandType.StoredProcedure);
        }

        public bool ValidarCodigoMfa(Guid usuarioId, string codigo)
        {
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("HumayaDigital_Usuarios..sp_McpMfaCodigos_Validar", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@McpUsuarioId", usuarioId);
            cmd.Parameters.AddWithValue("@Codigo", codigo);
            conn.Open();
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return false;
            return r.GetBoolean(r.GetOrdinal("Valido"));
        }

        public void GuardarAuthCode(Guid usuarioId, string codigo, string redirectUri)
        {
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("HumayaDigital_Usuarios..sp_McpAuthCodes_Guardar", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AuthCodeId", Guid.NewGuid());
            cmd.Parameters.AddWithValue("@McpUsuarioId", usuarioId);
            cmd.Parameters.AddWithValue("@Codigo", codigo);
            cmd.Parameters.AddWithValue("@RedirectUri", redirectUri);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public (bool valido, Guid? usuarioId) ValidarAuthCode(string codigo, string redirectUri)
        {
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("HumayaDigital_Usuarios..sp_McpAuthCodes_Validar", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Codigo", codigo);
            cmd.Parameters.AddWithValue("@RedirectUri", redirectUri);
            conn.Open();
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return (false, null);
            if (!r.GetBoolean(r.GetOrdinal("Valido"))) return (false, null);
            return (true, r.GetGuid(r.GetOrdinal("McpUsuarioId")));
        }

        public void RegistrarToken(Guid usuarioId, string tokenHash, int horas)
        {
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("HumayaDigital_Usuarios..sp_McpTokens_Guardar", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TokenId", Guid.NewGuid());
            cmd.Parameters.AddWithValue("@McpUsuarioId", usuarioId);
            cmd.Parameters.AddWithValue("@TokenHash", tokenHash);
            cmd.Parameters.AddWithValue("@HorasVigencia", horas);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public McpTokenInfoDto ValidarToken(string tokenHash)
        {
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("HumayaDigital_Usuarios..sp_McpTokens_Validar", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TokenHash", tokenHash);
            conn.Open();
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;
            return new McpTokenInfoDto
            {
                McpUsuarioId = r.GetGuid(r.GetOrdinal("McpUsuarioId")),
                Nombre = r["Nombre"].ToString(),
                Email = r["Email"].ToString()
            };
        }
    }

    public class McpUsuarioDto
    {
        public Guid McpUsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

    public class McpTokenInfoDto
    {
        public Guid McpUsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
    }
    public class McpSesionMfaDto
    {
        public Guid McpUsuarioId { get; set; }
        public string RedirectUri { get; set; }
        public string State { get; set; }
    }
}
