namespace HD.Security.interfaces
{
    public interface IGenerarJWT
    {
        string CrearToken(string IdUsuario,
        List<string> Roles,
        string JWT_SECRET_KEY,
        int JWT_EXPIRE_MINUTES);
    }
}