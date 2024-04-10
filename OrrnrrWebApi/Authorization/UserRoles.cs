namespace OrrnrrWebApi.Authorization
{
    [Flags]
    public enum UserRoles
    {
        None = 0,
        Manager = 0x1,
        Developer = 0x2,
        User = 0x4,
    }
}
