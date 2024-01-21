using System.Diagnostics.CodeAnalysis;

namespace Application.Exception;
[ExcludeFromCodeCoverage]
public static class ExceptionStrings
{
	#region Account

	public static readonly string NotFound_Username = "O utilizador com o username fornecido não foi encontrado";
	public static readonly string NotFound_Email = "O utilizador com o Email fornecido não foi encontrado";
	public static readonly string NotFound_LoginProvider = $"Não foi encontrado o respetivo login provider: {0}";

	public static readonly string Account_TokenFailed = "Não foi possivel criar o token";

	public static readonly string Account_Register_EmailUsed = "Email já se encontra em utilização";
	public static readonly string Account_Register_UserNameUsed = "Username já se encontra em utilização";
	public static readonly string Account_Login_Email = "Email não foi encontrado na base de dados";
	public static readonly string Account_Password_Failed = "Não foi possivel atualizar a password";
	public static readonly string Account_Phone_Failed = "Não foi possivel atualizar o numero de telemovel";
	#endregion
}