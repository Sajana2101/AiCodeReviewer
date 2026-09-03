
using System.Data.SqlClient;
public class BadExample
{
    private string password = "admin123";

    public void Login(string username, string inputPassword)
    {
        if (inputPassword == password)
        {
            Console.WriteLine("Login successful");
            Console.WriteLine($"User password: {password}");
        }
    }

    public void ProcessNumbers(List<int> numbers)
    {
        for (int i = 0; i < numbers.Count; i++)
        {
            for (int j = 0; j < numbers.Count; j++)
            {
                Console.WriteLine(numbers[i] + numbers[j]);
            }
        }
    }

    public string GetUserRole(string username)
    {
        if (username == "admin")
        {
            return "Administrator";
        }

        return "User";
    }
}