using DotCruz.Tenants.Domain.Enums.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using System.Text.RegularExpressions;

namespace DotCruz.Tenants.Domain.ValueObjects.Fiscal;

public partial record class FiscalDocument
{
    public string Number { get; }
    public DocumentType Type { get; }

    private FiscalDocument(string number, DocumentType type)
    {
        Number = number;
        Type = type;
    }

    public static FiscalDocument Create(string number, DocumentType type)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ErrorOnValidationException(ResourceMessagesException.DOCUMENT_EMPTY);

        return type switch
        {
            DocumentType.CPF => CreateCpf(number),
            DocumentType.CNPJ => CreateCnpj(number),
            _ => throw new ErrorOnValidationException(ResourceMessagesException.DOCUMENT_INVALID)
        };
    }

    private static FiscalDocument CreateCpf(string number)
    {
        var digitsOnly = OnlyDigitsRegex().Replace(number, "");

        if (digitsOnly.Length != 11 || !ValidateCpf(digitsOnly))
            throw new ErrorOnValidationException(ResourceMessagesException.DOCUMENT_INVALID);

        return new FiscalDocument(digitsOnly, DocumentType.CPF);
    }

    private static FiscalDocument CreateCnpj(string number)
    {
        var normalized = SeparatorsRegex().Replace(number, "").ToUpperInvariant();

        if (!CnpjFormatRegex().IsMatch(normalized) || !ValidateCnpj(normalized))
            throw new ErrorOnValidationException(ResourceMessagesException.DOCUMENT_INVALID);

        return new FiscalDocument(normalized, DocumentType.CNPJ);
    }

    public string FormattedValue => Type == DocumentType.CPF
        ? $"{Number[..3]}.{Number[3..6]}.{Number[6..9]}-{Number[9..]}"
        : $"{Number[..2]}.{Number[2..5]}.{Number[5..8]}/{Number[8..12]}-{Number[12..]}";

    public override string ToString() => FormattedValue;

    public static implicit operator string(FiscalDocument document) => document.Number;

    private static bool ValidateCpf(string cpf)
    {
        if (new string(cpf[0], 11) == cpf)
            return false;

        var tempCpf = cpf[..9];
        var sum = 0;

        var multiplier1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        for (var i = 0; i < 9; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        tempCpf += digit1;
        sum = 0;
        var multiplier2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        for (var i = 0; i < 10; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return cpf.EndsWith(digit1.ToString() + digit2.ToString());
    }

    private static bool ValidateCnpj(string cnpj)
    {
        if (new string(cnpj[0], 14) == cnpj)
            return false;

        var multiplier1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplier2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var sum = 0;
        for (var i = 0; i < 12; i++)
            sum += CharValue(cnpj[i]) * multiplier1[i];

        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        var tempCnpj = cnpj[..12] + digit1;
        sum = 0;
        for (var i = 0; i < 13; i++)
            sum += CharValue(tempCnpj[i]) * multiplier2[i];

        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return cnpj.EndsWith(digit1.ToString() + digit2.ToString());
    }

    private static int CharValue(char c) => c - '0';

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex OnlyDigitsRegex();

    [GeneratedRegex(@"[.\-/\s]")]
    private static partial Regex SeparatorsRegex();

    [GeneratedRegex(@"^[0-9A-Z]{12}[0-9]{2}$")]
    private static partial Regex CnpjFormatRegex();
}
