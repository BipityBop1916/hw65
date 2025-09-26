namespace chatmvc.Data;
using Microsoft.AspNetCore.Identity;

public class ErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DefaultError() => new() { Code = nameof(DefaultError), Description = "Произошла ошибка." };
    public override IdentityError ConcurrencyFailure() => new() { Code = nameof(ConcurrencyFailure), Description = "Ошибка конкуренции при обновлении." };
    public override IdentityError PasswordTooShort(int length) => new() { Code = nameof(PasswordTooShort), Description = $"Пароль должен содержать минимум {length} символов." };
    public override IdentityError PasswordRequiresNonAlphanumeric() => new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Пароль должен содержать хотя бы один не буквенно-цифровой символ." };
    public override IdentityError PasswordRequiresLower() => new() { Code = nameof(PasswordRequiresLower), Description = "Пароль должен содержать хотя бы одну строчную букву." };
    public override IdentityError PasswordRequiresUpper() => new() { Code = nameof(PasswordRequiresUpper), Description = "Пароль должен содержать хотя бы одну заглавную букву." };
    public override IdentityError PasswordRequiresDigit() => new() { Code = nameof(PasswordRequiresDigit), Description = "Пароль должен содержать хотя бы одну цифру." };
    public override IdentityError DuplicateUserName(string userName) => new() { Code = nameof(DuplicateUserName), Description = $"Имя пользователя \"{userName}\" уже занято." };
    public override IdentityError DuplicateEmail(string email) => new() { Code = nameof(DuplicateEmail), Description = $"Email \"{email}\" уже зарегистрирован." };
    public override IdentityError InvalidEmail(string email) => new() { Code = nameof(InvalidEmail), Description = $"Email \"{email}\" некорректен." };
    public override IdentityError InvalidUserName(string userName) => new() { Code = nameof(InvalidUserName), Description = $"Имя пользователя \"{userName}\" некорректно." };
    // Add more overrides as needed
}