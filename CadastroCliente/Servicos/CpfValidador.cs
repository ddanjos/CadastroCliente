namespace CadastroCliente.Servicos
{
    public static class CpfValidador
    {

        public static bool CpfValido(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return false;
            Span<char> numbers = stackalloc char[11];
            int count = 0;
            foreach (char c in cpf)
            {
                if (char.IsDigit(c))
                {
                    if (count >= 11) return false; 
                    numbers[count++] = c;
                }
            }

            if (count != 11)
                return false;

            if (IsAllSameDigits(numbers))
                return false;

            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += (numbers[i] - '0') * (10 - i);
            }

            int remainder = sum % 11;
            int firstDigit = remainder < 2 ? 0 : 11 - remainder;

            if (firstDigit != (numbers[9] - '0'))
                return false;

            sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += (numbers[i] - '0') * (11 - i);
            }

            remainder = sum % 11;
            int secondDigit = remainder < 2 ? 0 : 11 - remainder;

            return secondDigit == (numbers[10] - '0');
        }

        // digitos iguais
        private static bool IsAllSameDigits(ReadOnlySpan<char> numbers)
        {
            char first = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] != first)
                    return false;
            }
            return true;
        }
    }
}
