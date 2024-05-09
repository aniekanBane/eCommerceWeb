namespace SharedKernel.Utilities;

public static class GenerateRandom
{
    private readonly static Random _seed = new();

    public static string GenerateRandomString(int lenght = 8)
    {
        const string characters = "abcdefghijklmopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        int charsLenght = characters.Length;
        var buffer = new char[lenght];

        lock(_seed)
        {
            for (int i = 0; i < lenght; i++)
            {
                buffer[i] = characters[_seed.Next(charsLenght)];
            }
        }

        return new string(buffer);
    }
}
