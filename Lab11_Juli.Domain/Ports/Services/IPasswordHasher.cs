namespace Lab11_Juli.Domain.Ports.Services;

public interface IPasswordHasher
{
    // Crea un hash a partir de una contraseña
    string Hash(string password);

    // Verifica si la contraseña coincide con el hash
    bool Verify(string hash, string providedPassword);
}