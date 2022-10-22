namespace LibraryAPI.Enums
{
    public enum SecurityOperation
    {
        LOGIN = 0,
        LOGOUT = 1,
        USER_CREATED = 10,
        USER_DELETED = 11,
        EMAIL_CHANGE = 100,
        PASSWORD_CHANGE = 101,
        AUTHORIZED_READ = 200,
        UNAUTHORIZED_READ = 201,
    }
}
