// Contax.Application/Utils/PhoneNumberUtility.cs (Exemplo)
using System.Text.RegularExpressions;

public static class PhoneNumberUtility
{
    /// <summary>
    /// Normaliza o número de telefone para o formato E.164 (+CCDDNNNNNNNNN).
    /// </summary>
    /// <param name="rawPhone">Número de telefone como recebido.</param>
    /// <param name="defaultCountryCode">Código do país a ser adicionado se ausente.</param>
    /// <param name="defaultAreaCode">DDD padrão a ser adicionado se ausente.</param>
    public static string Normalize(
        string rawPhone,
        string defaultCountryCode = "55",
        string defaultAreaCode = "41"
    )
    {
        if (string.IsNullOrWhiteSpace(rawPhone))
            return string.Empty;

        // 1. Remove caracteres não-numéricos, exceto o '+'
        string cleaned = Regex.Replace(rawPhone, "[^0-9+]", "");

        // 2. Se o '+' não existir, assume que é um número nacional e adiciona o código completo.
        if (!cleaned.StartsWith("+"))
        {
            // Tenta garantir que o DDD e o 9º dígito estão presentes.
            // Para ser simples: se tiver 10 ou 11 dígitos, adicionamos +CC.
            if (cleaned.Length == 10 || cleaned.Length == 11) // Ex: 41988887777 ou 4188887777
            {
                cleaned = "+" + defaultCountryCode + cleaned;
            }
            else
            {
                // Se o número for muito curto (só o número local), adiciona DDD e País.
                cleaned = $"+{defaultCountryCode}{defaultAreaCode}{cleaned}";
            }
        }
        // Em um sistema mais complexo, você usaria uma biblioteca de telefonia (como LibPhoneNumber) aqui.

        return cleaned;
    }
}
