namespace Lab11_Juli.Application.DTOs;

public class RoleDto
{
    public string RoleName { get; set; } = null!;
}
public class RoleGetDto
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
}