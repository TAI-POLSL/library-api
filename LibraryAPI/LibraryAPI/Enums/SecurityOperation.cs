namespace LibraryAPI.Enums
{
    public enum SecurityOperation
    {
        LOGIN_ATTEMPT_SUCCESS = 0,
        LOGIN_ATTEMPT_FAILS = 1,
        LOGOUT_ATTEMPT_SUCCESS = 2,
        LOGOUT_ATTEMPT_FAILS = 3,
        USER_ACCOUNT_CREATED = 10,
        USER_ACCOUNT_DELETED = 11,
        USER_ACCOUNT_LOCK = 12,
        USER_EMAIL_CHANGE = 100,
        USER_PASSWORD_CHANGE = 101,
        UNAUTHORIZED_ATTEMPT_READ = 201,
    }
}
