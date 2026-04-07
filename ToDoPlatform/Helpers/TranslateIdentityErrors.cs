public static class TranslateIdentityErrors
{
    public static string TranslateErrorMessage(string code)
    {
        return code switch
        {
            "DuplicateUserName" => "Este e-mail já está em uso.",
            "DuplicateEmail" => "Este e-mail já está cadastrado.",
            "InvalidEmail" => "E-mail inválido.",
            "PasswordTooShort" => "A senha deve ter no mínimo 6 caracteres.",
            "PasswordRequiresNonAlphanumeric" => "A senha deve conter pelo menos um caractere especial.",
            "PasswordRequiresDigit" => "A senha deve conter pelo menos um número.",
            "PasswordRequiresUpper" => "A senha deve conter pelo menos uma letra maiúscula.",
            _ => "Erro desconhecido ao criar o usuário."
        };
    }
}