namespace chatmvc.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введите имя пользователя")]
    [Display(Name = "Имя пользователя")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Введите email")]
    [EmailAddress(ErrorMessage = "Некорректный email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Укажите дату рождения")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата рождения")]
    public DateTime DateOfBirth { get; set; }

    [Display(Name = "Аватар")]
    public IFormFile Avatar { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Подтвердите пароль")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }
}

public class LoginViewModel
{
    [Required(ErrorMessage = "Введите имя пользователя или email")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}

public class EditProfileViewModel
{
    public string Id { get; set; }

    [Required(ErrorMessage = "Введите имя пользователя")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Введите email")]
    [EmailAddress(ErrorMessage = "Некорректный email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Укажите дату рождения")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    public IFormFile Avatar { get; set; }
    public string AvatarPath { get; set; }
}

public class AdminCreateUserViewModel : RegisterViewModel
{
    public string Role { get; set; } = "user";
}