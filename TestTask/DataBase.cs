using Npgsql;

namespace TestTask;

static public class DataBase
{
    private static string dbSetting = "Host=localhost;Username=postgres;Password=YourPassword;Database=employeesDb";

    static public void CreateTable()
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(dbSetting))
        {
            try
            {
                connection.Open();
                string createTableQuery = @"
                   CREATE TABLE employees (
                   full_name TEXT,
                   birthday DATE,
                   gender VARCHAR(6)
                );";
                using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating database: {ex.Message}");
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }

    public static void AddRecordDb(IEnumerable<EmployeeInfo> employees)
    {
        using (NpgsqlConnection connection = new NpgsqlConnection(dbSetting))
        {
            connection.Open();
            try
            {
                using (var writer = connection.BeginBinaryImport("COPY employees (full_name, birthday, gender) FROM STDIN (FORMAT BINARY)"))
                {
                    foreach (var employee in employees)
                    {
                        writer.StartRow();
                        writer.Write(employee.FullName, NpgsqlTypes.NpgsqlDbType.Text);
                        writer.Write(employee.Birthday, NpgsqlTypes.NpgsqlDbType.Date);
                        writer.Write(employee.EmployeeGender.ToString(), NpgsqlTypes.NpgsqlDbType.Varchar);
                    }
                    writer.Complete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error add record: {ex.Message}");
                return;
            }
        }
    }

    public static List<EmployeeInfo>? GetEmployeesDb(string request)
    {
        List<EmployeeInfo> records = new();
        using (NpgsqlConnection connection = new NpgsqlConnection(dbSetting))
        {
            connection.Open();
            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand(request, connection))
                {
                    var rowsAffected = command.ExecuteReader();
                    while (rowsAffected.Read())
                    {
                        string fullName = rowsAffected.GetString(0);
                        DateTime birthday = rowsAffected.GetDateTime(1);

                        if (Enum.TryParse(rowsAffected.GetString(2), out EmployeeInfo.Gender gender))
                        {
                            records.Add(new EmployeeInfo(fullName, birthday, gender));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error get record: {ex.Message}");
                return null;
            }
            return records;
        }
    }
}
