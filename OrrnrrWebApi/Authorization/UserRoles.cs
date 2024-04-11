namespace OrrnrrWebApi.Authorization
{
    [Flags]
    public enum UserRoles
    {
        None = 0,
        User = 0x1,
        Manager = 0x2,
        Developer = 0x4,
    }
}
